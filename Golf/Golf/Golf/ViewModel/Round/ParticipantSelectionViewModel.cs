using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views;
using Golf.Views.PoppupView;
using Golf.Views.RoundDetailsView;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Round
{
    public class ParticipantSelectionViewModel : BaseViewModel
    {
       
        public ParticipantSelectionViewModel()
        {

            LoadPlayerList();

            MessagingCenter.Subscribe<App, string>(this, App.User.ISPLAYERLISTREFRESH, (sender, arg) => {

                int userId = Int32.Parse(arg);

                var item = PlayersList.FirstOrDefault(i => i.userId == userId);
                if (item != null)
                {
                    item.IsChecked = false;
                }
            }); 

        }

        #region Property Declaration

        public ObservableCollection<AllParticipantsResponse> PlayersList
        {
            get { return _PlayersList; }
            set
            {
                _PlayersList = value;
                OnPropertyChanged(nameof(PlayersList));
            }
        }
        public ObservableCollection<AllParticipantsResponse> _PlayersList;

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

                    var result = await App.ApiClient.GetParticipantsList();

                    if (result != null)
                    {
                        PlayersList = result;
                        OriginalPlayersList = PlayersList;
                    }

                    UserDialogs.Instance.HideLoading();

                    //var RestURL = App.User.BaseUrl + "User/getPlayerList?SearchTerm=";
                    //var httpClient = new HttpClient();
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    //var response = await httpClient.GetAsync(RestURL);
                    //var content = await response.Content.ReadAsStringAsync();

                    ////Assign the Values to Listview
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    PlayersList = JsonConvert.DeserializeObject<ObservableCollection<AllParticipantsResponse>>(content);
                    //    OriginalPlayersList = PlayersList;
                    //    UserDialogs.Instance.HideLoading();
                    //}
                    //else
                    //{
                    //    var error = JsonConvert.DeserializeObject<error>(content);
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    //}
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

        //#region CheckBox Selected Command Functionality

        //public ICommand CheckBoxSelectedCommand => new AsyncCommand(CheckboxChangedEvent);

        //public async Task CheckboxChangedEvent(object item)
        //{
        //    try
        //    {
        //        AllParticipantsResponse user = item as AllParticipantsResponse;
        //        int userId = user.userId;

        //        if (PlayersList.Where(p => p.IsChecked == true).Count() > 0)
        //        {
        //            bool UserIdAleradyExists = (PlayersList.Where(x => x.IsChecked == true && x.teamPlayerListId == userId)).Count() > 0;
        //            if (UserIdAleradyExists)
        //            {
        //                PlayersList.Where(x => x.userId == userId).ToList().ForEach(s => s.IsChecked = false);
        //            }
        //            else
        //            {
        //                PlayersList.Where(x => x.userId == userId).ToList().ForEach(s => s.IsChecked = true);
        //            }
        //        }
        //        else
        //        {
        //            PlayersList.Where(x => x.userId == userId).ToList().ForEach(s => s.IsChecked = true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var a = ex.Message;
        //    }
        //}
        //#endregion CheckBox Selected Command Functionality

        #region  Proceed Button Command Functionality
        public ICommand ProccedCommand => new AsyncCommand(ProceedAsync);
        async Task ProceedAsync()
        {
            try
            {                          
                var PlayerId = string.Join(",", OriginalPlayersList.Where(p => p.IsChecked == true)
                                .Select(p => p.userId.ToString()));
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    

                    var data = new RoundPlayers
                    {
                        userId = PlayerId,
                        roundId = App.User.CreateRoundId
                    };

                    var result = await App.ApiClient.AddRoundPlayer(data);

                    if (result != null)
                    {
                        await UserDialogs.Instance.AlertAsync("Round Players Successfully Added", "Success", "Ok");
                        App.User.TeamPreviewList.Clear();
                        App.User.TeamPreviewScoreKeeperName = string.Empty;
                        App.User.TeamPreviewScoreKeeperProfilePicture = string.Empty;
                        var view = new CreateTeamPage();
                        var navigationPage = ((NavigationPage)App.Current.MainPage);
                        await navigationPage.PushAsync(view);
                    }

                    UserDialogs.Instance.HideLoading();

                    //string RestURL = App.User.BaseUrl + "Round/SaveRoundPlayer";
                    //Uri requestUri = new Uri(RestURL);
                    //string json = JsonConvert.SerializeObject(data);
                    //var httpClient = new HttpClient();
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    //var response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    //string responJsonText = await response.Content.ReadAsStringAsync();

                    //if (response.IsSuccessStatusCode)
                    //{
                    //    await UserDialogs.Instance.AlertAsync("Round Players Successfully Added", "Success", "Ok");
                    //    UserDialogs.Instance.HideLoading();
                    //    App.User.TeamPreviewList.Clear();
                    //    App.User.TeamPreviewScoreKeeperName = string.Empty;
                    //    App.User.TeamPreviewScoreKeeperProfilePicture = string.Empty;
                    //    var view = new CreateTeamPage();
                    //    var navigationPage = ((NavigationPage)App.Current.MainPage);
                    //    await navigationPage.PushAsync(view);
                    //}
                    //else
                    //{
                    //    var error = JsonConvert.DeserializeObject<error>(responJsonText);
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    //}
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
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new InviteParticipantPage("participantselectionpage");
                await PopupNavigation.Instance.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion InviteParticipant Button Command Functionality

        #region PreviewPlayers
        public ICommand PlayersPreviewCommand => new AsyncCommand(PreviewPlayers);

        async Task PreviewPlayers()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                await PopupNavigation.Instance.PushAsync(new PreviewPlayersPage(new ObservableCollection<AllParticipantsResponse>(OriginalPlayersList.Where(p => p.IsChecked == true))));
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion

        #region Serach Command

        public ICommand SearchCommand => new Command<string>(Search);

        public async void Search(string keyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    PlayersList = new ObservableCollection<AllParticipantsResponse>();

                    PlayersList = new ObservableCollection<AllParticipantsResponse>(OriginalPlayersList.Where(x => x.email.StartsWith(keyword) || x.playerName.ToLower().Contains(keyword.ToLower())));
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
