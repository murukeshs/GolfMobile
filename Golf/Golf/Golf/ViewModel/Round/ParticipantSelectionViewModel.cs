using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views;
using Golf.Views.PoppupView;
using Golf.Views.RoundDetailsView;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Round
{
    public class ParticipantSelectionViewModel : BaseViewModel
    {
       
        public ParticipantSelectionViewModel()
        {
            App.User.PlayersPreviewList.Clear();

            LoadPlayerList();

            MessagingCenter.Subscribe<App, string>(this, App.User.ISPLAYERLISTREFRESH, (sender, arg) => {
                int userId = Int32.Parse(arg);
                PlayersList.Where(x => x.userId == userId).ToList().ForEach(s => s.IsChecked = false);
            });

        }

        #region Property Declaration

        public List<int> TeamPlayersIds = new List<int>();

        public ObservableCollection<AllParticipantsResponse> PlayersList
        {
            get { return _PlayersList; }
            set
            {
                _PlayersList = value;
                OnPropertyChanged(nameof(PlayersList));
            }
        }
        public ObservableCollection<AllParticipantsResponse> _PlayersList = null;

        private ObservableCollection<AllParticipantsResponse> OriginalPlayersList = new ObservableCollection<AllParticipantsResponse>();

        #endregion

        #region Load PlayerList API Functionality

        async void LoadPlayerList()
        {
           try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "User/getPlayerList?SearchTerm=";
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();

                    //Assign the Values to Listview
                    if (response.IsSuccessStatusCode)
                    {
                        PlayersList = JsonConvert.DeserializeObject<ObservableCollection<AllParticipantsResponse>>(content);
                        OriginalPlayersList = PlayersList;
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<error>(content);
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    }
                }
                else
                {
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                UserDialogs.Instance.HideLoading();
                DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
            }
        }

        #endregion PlayerList API Functionality

        #region CheckBox Selected Command Functionality

        public ICommand CheckBoxSelectedCommand => new Command<AllParticipantsResponse>(CheckboxChangedEvent);

        void CheckboxChangedEvent(AllParticipantsResponse item)
        {
            try
            {
                var userId = item.userId;

                if (TeamPlayersIds.Count > 0)
                {
                    bool UserIdAleradyExists = TeamPlayersIds.Contains(userId);
                    if (UserIdAleradyExists)
                    {
                        TeamPlayersIds.Remove(userId);
                        PlayersList.Where(x => x.userId == userId).ToList().ForEach(s => s.IsChecked = false);
                        var itemToRemove = App.User.PlayersPreviewList.Single(r => r.UserId == userId);
                        App.User.PlayersPreviewList.Remove(itemToRemove);
                    }
                    else
                    {
                        TeamPlayersIds.Add(userId);
                        PlayersList.Where(x => x.userId == userId).ToList().ForEach(s => s.IsChecked = true);
                        var list = new AddPlayersList { UserId = item.userId, PlayerName = item.playerName, PlayerImage = item.profileImage };
                        App.User.PlayersPreviewList.Add(list);
                    }
                }
                else
                {
                    TeamPlayersIds.Add(userId);
                    PlayersList.Where(x => x.userId == userId).ToList().ForEach(s => s.IsChecked = true);
                    var list = new AddPlayersList { UserId = item.userId, PlayerName = item.playerName, PlayerImage = item.profileImage };
                    App.User.PlayersPreviewList.Add(list);
                }
            }
            catch(Exception e)
            {
                var message = e.Message;
            }
        }
        #endregion CheckBox Selected Command Functionality

        #region  Proceed Button Command Functionality
        public ICommand ProccedCommand => new AsyncCommand(ProceedAsync);
        async Task ProceedAsync()
        {
            try
            {
                var PlayerId = string.Join(",", TeamPlayersIds);
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "Round/SaveRoundPlayer";
                    Uri requestUri = new Uri(RestURL);

                    var data = new RoundPlayers
                    {
                        userId = PlayerId,
                        roundId = App.User.CreateRoundId
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        await UserDialogs.Instance.AlertAsync("Round Players Successfully Added", "Success", "Ok");
                        UserDialogs.Instance.HideLoading();
                        App.User.TeamPreviewList.Clear();
                        App.User.TeamPreviewScoreKeeperName = string.Empty;
                        App.User.TeamPreviewScoreKeeperProfilePicture = string.Empty;
                        var view = new CreateTeamPage();
                        var navigationPage = ((NavigationPage)App.Current.MainPage);
                        await navigationPage.PushAsync(view);
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<error>(responJsonText);
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    }
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                if (a == "System.Net.WebException")
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
                }
            }
        }
        #endregion Round Code Proceed Button Command Functionality

        #region InviteParticipant Button Command Functionality
        public ICommand InviteParticipantCommand => new AsyncCommand(InviteParticipantsAsync);

        async Task InviteParticipantsAsync()
        {
            UserDialogs.Instance.ShowLoading();
            var view = new InviteParticipantPage();
            await PopupNavigation.Instance.PushAsync(view);
            UserDialogs.Instance.HideLoading();
        }
        #endregion InviteParticipant Button Command Functionality

        #region PreviewPlayers
        public ICommand PlayersPreviewCommand => new AsyncCommand(PreviewPlayers);

        async Task PreviewPlayers()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                await PopupNavigation.Instance.PushAsync(new PreviewPlayersPage());
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion

        #region Serach Command

        private ObservableCollection<AllParticipantsResponse> PlayersListItems = new ObservableCollection<AllParticipantsResponse>();

        public ICommand SearchCommand => new Command<string>(Search);

        public async void Search(string keyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    PlayersList = new ObservableCollection<AllParticipantsResponse>();

                    PlayersListItems = new ObservableCollection<AllParticipantsResponse>();

                    var query = OriginalPlayersList.Where(x => x.email.StartsWith(keyword) || x.playerName.ToLower().Contains(keyword.ToLower()));

                    foreach (var item in query)
                    {
                        var value = new AllParticipantsResponse() { email = item.email, gender = item.gender, ImageIcon = item.ImageIcon, isChecked = item.isChecked, IsChecked = item.IsChecked, isPublicProfile = item.isPublicProfile, isScoreKeeper = item.isScoreKeeper, nickName = item.nickName, playerName = item.playerName, profileImage = item.profileImage, roleType = item.roleType, userId = item.userId, userType = item.userType };

                        PlayersListItems.Add(value);
                    }

                    PlayersList = PlayersListItems;
                }
                else
                {
                    PlayersList = OriginalPlayersList;
                }

            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        #endregion

        #region viewplayerdetails command

        public ICommand ViewPlayerDetailsCommand => new Command(ViewPlayerDetails);

        public async void ViewPlayerDetails(object obj)
        {
            try
            {
                var item = obj as AllParticipantsResponse;
                if (!item.isPublicProfile)
                {
                    var msg = item.nickName + " is a Private Profile!!!";
                    UserDialogs.Instance.Alert(msg, "Private Profile", "Ok");
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new RoundPlayerDetailsPopup(item));
                }

            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion

    }
}
