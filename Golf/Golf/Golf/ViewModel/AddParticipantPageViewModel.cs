﻿using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views;
using Golf.Views.CreateRoundView;
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

namespace Golf.ViewModel
{
    public class AddParticipantPageViewModel : BaseViewModel
    {
        public AddParticipantPageViewModel()
        {
            LoadPlayerListAsync();

            TeamName = App.User.TeamName;
            App.User.ScoreKeeperId = 0;

            MessagingCenter.Subscribe<App, string>(this, App.User.ISPARTICIPANTLISTREFRESH, (sender, arg) => {
                int userId = Int32.Parse(arg);

                var item = PlayersListItems.FirstOrDefault(i => i.userId == userId);
                if (item != null)
                {
                    item.IsChecked = false;
                }
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

        async void LoadPlayerListAsync()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var result = await App.ApiClient.GetRoundPlayersList(App.User.CreateRoundId, "team");
                    if (result != null)
                    {
                        PlayersListItems = result;
                        OriginalPlayersList = PlayersListItems;
                    }
                    UserDialogs.Instance.HideLoading();
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
                await PopupNavigation.Instance.PushAsync(new TeamPreviewPage(new ObservableCollection<AllParticipantsResponse>(OriginalPlayersList.Where(p => p.IsChecked == true))));
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                UserDialogs.Instance.HideLoading();
            }
        }

        #endregion TeamPreview Command Functionality

        #region Toggle Selected Command Functionality

        public ICommand ToggleSelectedCommand => new Command(ToggleChangedEvent);

        void ToggleChangedEvent(object parameter)
        {
            try
            {
                var Item = parameter as AllParticipantsResponse;
                var userId = Item.userId;
                bool UserIdAleradyExists = (PlayersListItems.Where(x => x.IsChecked == true && x.userId == userId)).Count() > 0;
                if (UserIdAleradyExists)
                {
                    UserDialogs.Instance.Alert("Player can't be added as a score keeper.","Alert","Ok");
                }
                else
                {
                    PlayersListItems.Where(x => x.userId == userId).ToList().ForEach(s => s.ImageIcon = "checked_icon.png");
                    PlayersListItems.Where(x => x.userId != userId).ToList().ForEach(s => s.ImageIcon = "unchecked_icon.png");
                    App.User.ScoreKeeperId = userId;
                    App.User.TeamPreviewScoreKeeperName = Item.nickName;
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
                    var PlayerId = string.Join(",", PlayersListItems.Where(p => p.IsChecked == true)
                                .Select(p => p.userId.ToString()));
                if (CrossConnectivity.Current.IsConnected)
                    {
                            if (App.User.ScoreKeeperId != 0)
                            {
                                UserDialogs.Instance.ShowLoading();

                                var data = new TeamPlayer
                                {
                                    teamId = App.User.CreateTeamId,
                                    scoreKeeperID = App.User.ScoreKeeperId,
                                    playerId = PlayerId,
                                    roundId = App.User.CreateRoundId
                                };

                                var result = await App.ApiClient.CreateTeamPlayers(data);

                                if (result != null)
                                {
                                    await UserDialogs.Instance.AlertAsync("Team Players Successfully Added", "Success", "Ok");
                                    App.User.TeamPreviewList.Clear();
                                    App.User.TeamPreviewScoreKeeperName = string.Empty;
                                    App.User.TeamPreviewScoreKeeperProfilePicture = string.Empty;
                                    var view = new FinalTeamsForRound();
                                    var navigationPage = ((NavigationPage)App.Current.MainPage);
                                    await navigationPage.PushAsync(view);
                                }

                                UserDialogs.Instance.HideLoading();

                        //string RestURL = App.User.BaseUrl + "Team/createTeamPlayers";
                        //Uri requestUri = new Uri(RestURL);
                        //string json = JsonConvert.SerializeObject(data);
                        //var httpClient = new HttpClient();
                        //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                        //var response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                        //string responJsonText = await response.Content.ReadAsStringAsync();

                        //if (response.IsSuccessStatusCode)
                        //{
                        //    await UserDialogs.Instance.AlertAsync("Team Players Successfully Added", "Success", "Ok");
                        //    UserDialogs.Instance.HideLoading();
                        //    App.User.TeamPreviewList.Clear();
                        //    App.User.TeamPreviewScoreKeeperName = string.Empty;
                        //    App.User.TeamPreviewScoreKeeperProfilePicture = string.Empty;
                        //    var view = new FinalTeamsForRound();
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
                var view = new InviteParticipantPage("addparticipantpage");
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

        public ICommand SearchCommand => new Command<string>(Search);

        public async void Search(string keyword)
        {
            try
            {
                PlayersListItems = new ObservableCollection<AllParticipantsResponse>();

                if (!string.IsNullOrEmpty(keyword))
                {
                    PlayersListItems = new ObservableCollection<AllParticipantsResponse>(OriginalPlayersList.Where(x => x.email.StartsWith(keyword) || x.playerName.ToLower().Contains(keyword.ToLower())));
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


