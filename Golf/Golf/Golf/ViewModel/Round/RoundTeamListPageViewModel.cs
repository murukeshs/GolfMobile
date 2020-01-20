using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views.CreateRoundView;
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
    public class RoundTeamListPageViewModel : BaseViewModel
    {
        public RoundTeamListPageViewModel()
        {
            LoadTeamListAsync();
        }

        #region Property Declaration

        public ObservableCollection<RoundTeamWithPlayers> roundTeamsItemsList = new ObservableCollection<RoundTeamWithPlayers>();

        public ObservableCollection<RoundTeamWithPlayers> RoundTeamsItemsList 
        {
            get { return _RoundTeamsItemsList; }
            set
            {
                _RoundTeamsItemsList = value;
                OnPropertyChanged(nameof(RoundTeamsItemsList));
            }
        }
        private ObservableCollection<RoundTeamWithPlayers> _RoundTeamsItemsList = null;

        public List<teamplayerList> RoundTeamsPlayerList
        {
            get { return _RoundTeamsPlayerList; }
            set
            {
                _RoundTeamsPlayerList = value;
                OnPropertyChanged(nameof(RoundTeamsPlayerList));
            }
        }
        private List<teamplayerList> _RoundTeamsPlayerList = null;

        #endregion

        #region SaveTeamButtonCLicked Command Functionality

        public ICommand SaveTeamsCommand => new AsyncCommand(SaveTeamButtonCLicked);

        private async Task SaveTeamButtonCLicked()
        {
            await CreateRoundplayersAsync();
        }

        #endregion SaveTeamButtonCLicked Command Functionality

        #region checkbox command

        public List<int> TeamIdList = new List<int>();

        public ICommand CheckBoxSelectedCommand => new Command<RoundTeamWithPlayers>(CheckBoxChanged);

        async void CheckBoxChanged(RoundTeamWithPlayers item)
        {
            try
            {
                var teamId = item.teamId;
                if (item.noOfPlayers > 0)
                {
                    if (TeamIdList.Count > 0)
                    {
                        bool UserIdAleradyExists = TeamIdList.Contains(teamId);
                        if (UserIdAleradyExists)
                        {
                            TeamIdList.Remove(teamId);
                        }
                        else
                        {
                            TeamIdList.Add(teamId);
                        }
                    }
                    else
                    {
                        TeamIdList.Add(teamId);
                    }
                }
                else
                {
                    if (item.IsChecked == true)
                    {
                        await UserDialogs.Instance.AlertAsync("No Players available for your selected team.", "Alert", "Ok");
                        RoundTeamsItemsList.Where(w => w.teamId == teamId).ToList().ForEach(s => s.IsChecked = false);
                    }
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        #endregion

        #region LoadTeamList 

        async void LoadTeamListAsync()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();

                    var result = await App.ApiClient.GetTeamsList();

                    if (result != null)
                    {
                        var Items = result;
                        foreach (var item in Items)
                        {
                            RoundTeamsPlayerList = JsonConvert.DeserializeObject<List<teamplayerList>>(item.teamplayerList);
                            roundTeamsItemsList.Add(new RoundTeamWithPlayers
                            {
                                teamId = item.teamId,
                                teamplayerList = RoundTeamsPlayerList,
                                createdBy = item.createdBy,
                                createdName = item.createdName,
                                createdOn = item.createdOn,
                                noOfPlayers = item.noOfPlayers,
                                scoreKeeperID = item.scoreKeeperID,
                                startingHole = item.startingHole,
                                teamIcon = item.teamIcon,
                                teamName = item.teamName,
                                Expanded = false
                            });
                        }
                        RoundTeamsItemsList = roundTeamsItemsList;
                    }

                    UserDialogs.Instance.HideLoading();

                    //var RestURL = App.User.BaseUrl + "Team/listTeam";
                    //var httpClient = new HttpClient();
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    //var response = await httpClient.GetAsync(RestURL);
                    //var content = await response.Content.ReadAsStringAsync();

                    ////Assign the Values to Listview
                    //if(response.IsSuccessStatusCode)
                    //{
                    //    var Items = JsonConvert.DeserializeObject<ObservableCollection<RoundTeamItems>>(content);
                    //    foreach (var item in Items)
                    //    {
                    //        RoundTeamsPlayerList = JsonConvert.DeserializeObject<List<teamplayerList>>(item.teamplayerList);
                    //        roundTeamsItemsList.Add(new RoundTeamWithPlayers
                    //        {
                    //            teamId = item.teamId,
                    //            teamplayerList = RoundTeamsPlayerList,
                    //            createdBy = item.createdBy,
                    //            createdName = item.createdName,
                    //            createdOn = item.createdOn,
                    //            noOfPlayers = item.noOfPlayers,
                    //            scoreKeeperID = item.scoreKeeperID,
                    //            startingHole = item.startingHole,
                    //            teamIcon = item.teamIcon,
                    //            teamName = item.teamName,
                    //            Expanded = false
                    //        });
                    //    }
                    //    RoundTeamsItemsList = roundTeamsItemsList;
                    //}
                    //else
                    //{
                    //    var error = JsonConvert.DeserializeObject<error>(content);
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    //}

                    //UserDialogs.Instance.HideLoading();
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

        #endregion

        #region CreateRound Players Functionality

        async Task CreateRoundplayersAsync()
        {
            try
            {
                var teamPlayerId = string.Join(",", TeamIdList);

                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();

                    var data = new CreateRoundPlayers
                    {
                        type = "Round",
                        eventId = App.User.CreateRoundId,
                        teamId = teamPlayerId,
                    };

                    var result = await App.ApiClient.CreateRoundPlayer(data);

                    if (result != null)
                    {
                        var view = new SendInvitePoppup();
                        await PopupNavigation.Instance.PushAsync(view);
                    }

                    UserDialogs.Instance.HideLoading();

                    //string RestURL = App.User.BaseUrl + "Round/createRoundplayer";
                    //Uri requestUri = new Uri(RestURL);
                    //string json = JsonConvert.SerializeObject(data);
                    //var httpClient = new HttpClient();
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    //var response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    //string responJsonText = await response.Content.ReadAsStringAsync();

                    //if (response.IsSuccessStatusCode)
                    //{
                    //    var view = new SendInvitePoppup();
                    //    await PopupNavigation.Instance.PushAsync(view);
                    //    UserDialogs.Instance.HideLoading();
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

        #endregion

        #region Team Item Tabbed Command Functionality

        //To Hide and UnHide Players details Page
        private RoundTeamWithPlayers _LastSelectedItem;

        public ICommand TeamItemsTabbedCommand => new Command(HideorShowItems);

        async void HideorShowItems(object parameter)
        {
            try
            {
                var Item = parameter as RoundTeamWithPlayers;

                if (_LastSelectedItem == Item)
                {
                    Item.Expanded = !Item.Expanded;
                    await UpdateItems(Item);
                }
                else
                {
                    if (_LastSelectedItem != null)
                    {
                        //hide the previous selected item
                        _LastSelectedItem.Expanded = false;
                        await UpdateItems(_LastSelectedItem);
                    }
                    //Or show the selected item
                    Item.Expanded = true;
                    await UpdateItems(Item);
                }
                _LastSelectedItem = Item;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        async Task UpdateItems(RoundTeamWithPlayers Items)
        {
            try 
            { 
                var index = RoundTeamsItemsList.IndexOf(Items);
                RoundTeamsItemsList.Remove(Items);
                RoundTeamsItemsList.Insert(index, Items);
                OnPropertyChanged(nameof(RoundTeamsItemsList));
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        #endregion Team Item Tabbed Command Functionality

    }
}
