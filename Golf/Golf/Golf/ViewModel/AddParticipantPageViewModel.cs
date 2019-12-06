﻿using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views;
using Golf.Views.CreateRoundView;
using Golf.Views.MenuView;
using Golf.Views.PoppupView;
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
        public int ScoreKeeperId = 0;
        public AddParticipantPageViewModel()
        {
            LoadPlayerListAsync();
            TeamName = App.User.TeamName;
            //For Clear the team preview list
            App.User.TeamPreviewList.Clear();

            MessagingCenter.Subscribe<App, string>(this, App.User.ISPARTICIPANTLISTREFRESH, (sender, arg) => {
                int userId = Int32.Parse(arg);
                PlayersList.Where(x => x.userId == userId).ToList().ForEach(s => s.IsChecked = false);
            });
        }

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

        #region PlayerList API Functionality
        public ObservableCollection<AllParticipantsResponse> PlayersList
        {
            get { return _PlayersList;}
            set
            {
                _PlayersList = value;
                OnPropertyChanged(nameof(PlayersList));
            }
        }
        public ObservableCollection<AllParticipantsResponse> _PlayersList = null;

        async void LoadPlayerListAsync()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "Round/GetRoundPlayers?roundId=" + App.User.CreateRoundId;
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var Items = JsonConvert.DeserializeObject<ObservableCollection<AllParticipantsResponse>>(content);
                        //Assign the Values to Listview
                        PlayersList = Items;
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

        #region TeamPreview Command Functionality
        public ICommand TeamPreviewCommand => new AsyncCommand(TeamPreviewButtonAsync);
        async Task TeamPreviewButtonAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                await PopupNavigation.Instance.PushAsync(new TeamPreviewPage());
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion TeamPreview Command Functionality

        #region CheckBox Selected Command Functionality

        public List<int> TeamPlayersIds = new List<int>();

        public ObservableCollection<AddPlayersList> TeamPreviewList = new ObservableCollection<AddPlayersList>();
        public ICommand CheckBoxSelectedCommand => new Command(CheckboxChangedEvent);

        async void CheckboxChangedEvent(object parameter)
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
                        var itemToRemove = App.User.TeamPreviewList.Single(r => r.UserId == userId);
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
                    ScoreKeeperId = Item.userId;
                    App.User.ScoreKeeperId = ScoreKeeperId;
                    App.User.TeamPreviewScoreKeeperName = Item.playerName;
                    App.User.TeamPreviewScoreKeeperProfilePicture = Item.profileImage;
                }
            }
            catch (Exception ex)
            {

            }
        }


        //To Hide and UnHide Players details Page
        async void UpdateItems(AllParticipantsResponse Items)
        {
            var index = PlayersList.IndexOf(Items);
            PlayersList.Remove(Items);
            PlayersList.Insert(index, Items);
            OnPropertyChanged(nameof(PlayersList));
        }
        #endregion Toggle Selected Command Functionality

        #region CreateTeamPlayers Button Command Functionality

        public ICommand CreateTeamButtonCommand => new AsyncCommand(CreateTeamPlayersAsync);
        //Create Team Players API Function
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
            var view = new InviteParticipantPage();
            await PopupNavigation.Instance.PushAsync(view);
            UserDialogs.Instance.HideLoading();
        }
        #endregion AddParticipants Button Command Functionality

    }

}


