using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views;
using Golf.Views.CreateRoundView;
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

namespace Golf.ViewModel
{
    public class AddParticipantPageViewModel : BaseViewModel
    {

        public AddParticipantPageViewModel()
        {
            LoadPlayerListAsync();

            TeamName = App.User.TeamName;
            //For Clear the team preview list
            App.User.TeamPreviewList.Clear();
            App.User.ScoreKeeperId = 0;

            MessagingCenter.Subscribe<App, string>(this, App.User.ISPARTICIPANTLISTREFRESH, (sender, arg) => {
                int userId = Int32.Parse(arg);
                PlayersList.Where(x => x.userId == userId).ToList().ForEach(s => s.IsChecked = false);
                OriginalPlayersList.Where(x => x.userId == userId).ToList().ForEach(s => s.IsChecked = false);
                TeamPlayersIds.Remove(userId);
            });

        }

        #region Property Declaration

        public ObservableCollection<AllParticipantsResponse> PlayersListItems
        {
            get { return _PlayersListItems; }
            set
            {
                _PlayersListItems = value;
                OnPropertyChanged("PlayersListItems");
            }
        }
        private ObservableCollection<AllParticipantsResponse> _PlayersListItems;

        private ObservableCollection<AllParticipantsResponse> OriginalPlayersList = new ObservableCollection<AllParticipantsResponse>();

        public int ScoreKeeperId = 0; 

        public string TeamName
        {
            get { return _TeamName; }
            set
            {
                _TeamName = value;
                OnPropertyChanged("TeamName");
            }
        }
        private string _TeamName = string.Empty;

        public string ProfileImage
        {
            get { return _ProfileImage; }
            set
            {
                _ProfileImage = value;
                OnPropertyChanged("ProfileImage");
            }
        }
        private string _ProfileImage = string.Empty;

        #endregion

        #region PlayerList API Functionality

        public ObservableCollection<AllParticipantsResponse> PlayersList = new ObservableCollection<AllParticipantsResponse>();

        async void LoadPlayerListAsync()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "Round/GetRoundPlayers?roundId=" + App.User.CreateRoundId + "&action=team";
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                       PlayersListItems = JsonConvert.DeserializeObject<ObservableCollection<AllParticipantsResponse>>(content);

                        //foreach (var item in Items.Where(w => w.isChecked == true))
                        //{
                        //    var value = new AllParticipantsResponse() { email = item.email, gender = item.gender, ImageIcon = item.ImageIcon, isChecked = item.isChecked, IsChecked = item.IsChecked, isPublicProfile = item.isPublicProfile, isScoreKeeper = item.isScoreKeeper, nickName = item.nickName, playerName = item.playerName, profileImage = item.profileImage, roleType = item.roleType, userId = item.userId, userType = item.userType };
                        //    if (item.roleType == "ScoreKeeper")
                        //    {
                        //        ScoreKeeperId = item.userId;
                        //    }
                        //    PlayersList.Add(value);
                        //}

                        OriginalPlayersList = PlayersListItems;
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

        #endregion PlayerList API Functionality

        #region TeamPreview Command Functionality

        public ICommand TeamPreviewCommand => new AsyncCommand(TeamPreviewButtonAsync);

        async Task TeamPreviewButtonAsync()
        {
            UserDialogs.Instance.ShowLoading();
            try
            {
                await PopupNavigation.Instance.PushAsync(new TeamPreviewPage());
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                UserDialogs.Instance.HideLoading();
            }
        }

        #endregion TeamPreview Command Functionality

        #region CheckBox Selected Command Functionality

        public List<int> TeamPlayersIds = new List<int>();

        public ObservableCollection<AddPlayersList> TeamPreviewList = new ObservableCollection<AddPlayersList>();

        public ICommand CheckBoxSelectedCommand => new Command(CheckboxChangedEvent);

        async void CheckboxChangedEvent(object parameter)
        {
            try
            {
                var item = parameter as AllParticipantsResponse;
                var userId = item.userId;
                if (App.User.ScoreKeeperId != userId)
                {
                    if (TeamPlayersIds.Count > 0)
                    {
                        bool UserIdAleradyExists = TeamPlayersIds.Contains(userId);
                        if (UserIdAleradyExists)
                        {
                            TeamPlayersIds.Remove(userId);
                            var itemToRemove = App.User.TeamPreviewList.SingleOrDefault(r => r.UserId == userId);
                            App.User.TeamPreviewList.Remove(itemToRemove);
                        }
                        else
                        {
                            TeamPlayersIds.Add(userId);
                            var list = new AddPlayersList { UserId = item.userId, PlayerName = item.playerName, PlayerHCP = "5", PlayerType = item.userType, IsStoreKeeper = true, PlayerImage = item.profileImage };
                            App.User.TeamPreviewList.Add(list);
                        }
                    }
                    else
                    {
                        TeamPlayersIds.Add(userId);
                        var list = new AddPlayersList { UserId = item.userId, PlayerName = item.playerName, PlayerHCP = "5", PlayerType = item.userType, IsStoreKeeper = true, PlayerImage = item.profileImage };
                        App.User.TeamPreviewList.Add(list);
                    }
                }
                else
                {
                    UserDialogs.Instance.Alert("score keeper can't be a Player .", "Alert", "Ok");
                    PlayersList.Where(x => x.userId == userId).ToList().ForEach(s => s.IsChecked = false);
                    OriginalPlayersList.Where(x => x.userId == userId).ToList().ForEach(s => s.IsChecked = false);
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion CheckBox Selected Command Functionality

        #region Toggle Selected Command Functionality

        public ICommand ToggleSelectedCommand => new Command(ToggleChangedEvent);

        void ToggleChangedEvent(object parameter)
        {
            try
            {
                var Item = parameter as AllParticipantsResponse;

                bool UserIdAleradyExists = TeamPlayersIds.Contains(Item.userId);
                if (UserIdAleradyExists)
                {
                    UserDialogs.Instance.Alert("Player can't be added as a score keeper.","Alert","Ok");
                }
                else
                {
                    PlayersList.Where(x => x.userId == Item.userId).ToList().ForEach(s => s.ImageIcon = "checked_icon.png");
                    PlayersList.Where(x => x.userId != Item.userId).ToList().ForEach(s => s.ImageIcon = "unchecked_icon.png");
                    OriginalPlayersList.Where(x => x.userId == Item.userId).ToList().ForEach(s => s.ImageIcon = "checked_icon.png");
                    OriginalPlayersList.Where(x => x.userId != Item.userId).ToList().ForEach(s => s.ImageIcon = "unchecked_icon.png");
                    ScoreKeeperId = Item.userId;
                    App.User.ScoreKeeperId = ScoreKeeperId;
                    App.User.TeamPreviewScoreKeeperName = Item.playerName;
                    App.User.TeamPreviewScoreKeeperProfilePicture = Item.profileImage;
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        #endregion Toggle Selected Command Functionality

        #region CreateTeamPlayers Button Command Functionality

        public ICommand CreateTeamButtonCommand => new AsyncCommand(CreateTeamPlayersAsync);

        async Task CreateTeamPlayersAsync()
        {
            try
            {
                    var PlayerId = string.Join(",", TeamPlayersIds);
                    if (CrossConnectivity.Current.IsConnected)
                    {
                            if (ScoreKeeperId != 0)
                            {
                                UserDialogs.Instance.ShowLoading();
                                string RestURL = App.User.BaseUrl + "Team/createTeamPlayers";
                                Uri requestUri = new Uri(RestURL);

                                var data = new TeamPlayer
                                {
                                    teamId = App.User.CreateTeamId,
                                    scoreKeeperID = ScoreKeeperId,
                                    playerId = PlayerId,
                                    roundId = App.User.CreateRoundId
                                };

                                string json = JsonConvert.SerializeObject(data);
                                var httpClient = new HttpClient();
                                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                                var response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                                string responJsonText = await response.Content.ReadAsStringAsync();

                                if (response.IsSuccessStatusCode)
                                {
                                    await UserDialogs.Instance.AlertAsync("Team Players Successfully Added", "Success", "Ok");
                                    UserDialogs.Instance.HideLoading();
                                    App.User.TeamPreviewList.Clear();
                                    App.User.TeamPreviewScoreKeeperName = string.Empty;
                                    App.User.TeamPreviewScoreKeeperProfilePicture = string.Empty;
                                    var view = new FinalTeamsForRound();
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
                                UserDialogs.Instance.Alert("Please Select ScoreKeeper !", "Alert", "Ok");
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
        #endregion CreateTeamPlayers Button Command Functionality

        #region AddParticipants Button Selected Command Functionality

        public ICommand AddParticipantsCommand => new AsyncCommand(AddParticipantsAsync);

        async Task AddParticipantsAsync()
        {
            UserDialogs.Instance.ShowLoading();
            try
            {
                var view = new InviteParticipantPage();
                await PopupNavigation.Instance.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                UserDialogs.Instance.HideLoading();
            }
        }

        #endregion AddParticipants Button Command Functionality

        #region Serach Command

        private ObservableCollection<AllParticipantsResponse> TempPlayersList = new ObservableCollection<AllParticipantsResponse>();

        public ICommand SearchCommand => new Command<string>(Search);

        public async void Search(string keyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    TempPlayersList = new ObservableCollection<AllParticipantsResponse>();

                    PlayersListItems = new ObservableCollection<AllParticipantsResponse>();

                    var query = OriginalPlayersList.Where(x => x.email.StartsWith(keyword) || x.playerName.ToLower().Contains(keyword.ToLower()));

                    foreach (var item in query)
                    {
                        var value = new AllParticipantsResponse() { email = item.email, gender = item.gender, ImageIcon = item.ImageIcon, isChecked = item.isChecked, IsChecked = item.IsChecked, isPublicProfile = item.isPublicProfile, isScoreKeeper = item.isScoreKeeper, nickName = item.nickName, playerName = item.playerName, profileImage = item.profileImage, roleType = item.roleType, userId = item.userId, userType = item.userType };

                        TempPlayersList.Add(value);
                    }

                    PlayersListItems = TempPlayersList;
                }
                else
                {
                    PlayersListItems = OriginalPlayersList;
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


