using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views.CreateMatchView;
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

namespace Golf.ViewModel.Match
{
    public class MatchTeamListPageViewModel : BaseViewModel
    {
        public MatchTeamListPageViewModel()
        {
            LoadTeamListAsync();
        }
        public ObservableCollection<MatchTeamWithPlayers> matchTeamsItemsList = new ObservableCollection<MatchTeamWithPlayers>();
        public ObservableCollection<MatchTeamWithPlayers> MatchTeamsItemsList 
        {
            get { return _MatchTeamsItemsList; }
            set
            {
                _MatchTeamsItemsList = value;
                OnPropertyChanged(nameof(MatchTeamsItemsList));
            }
        }
        private ObservableCollection<MatchTeamWithPlayers> _MatchTeamsItemsList = null;

        public List<teamplayerList> MatchTeamsPlayerList
        {
            get { return _MatchTeamsPlayerList; }
            set
            {
                _MatchTeamsPlayerList = value;
                OnPropertyChanged(nameof(MatchTeamsPlayerList));
            }
        }
        private List<teamplayerList> _MatchTeamsPlayerList = null;


        #region SaveTeamButtonCLicked Command Functionality

        public ICommand SaveTeamsCommand => new AsyncCommand(SaveTeamButtonCLicked);

        private async Task SaveTeamButtonCLicked()
        {
            //ViewModelNavigation.PushAsync(new ItemPageViewModel() { Item = parameter as RssItem });
            await createMatchplayersAsync();
        }
        #endregion SaveTeamButtonCLicked Command Functionality


        public List<int> TeamIdList = new List<int>();
        public ICommand CheckBoxSelectedCommand => new Command(CheckBoxChanged);

        async void CheckBoxChanged(object parameter)
        {
            var item = parameter as MatchTeamWithPlayers;
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
                    MatchTeamsItemsList.Where(w => w.teamId == teamId).ToList().ForEach(s => s.IsChecked = false);
                }
            }
        }


        async void LoadTeamListAsync()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    //player type is 1 to get player list
                    var RestURL = App.User.BaseUrl + "Team/listTeam";
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                   
                    //Assign the Values to Listview
                    if(response.IsSuccessStatusCode)
                    {
                        var Items = JsonConvert.DeserializeObject<ObservableCollection<MatchTeamItems>>(content);
                        foreach (var item in Items)
                        {
                            MatchTeamsPlayerList = JsonConvert.DeserializeObject<List<teamplayerList>>(item.teamplayerList);
                            matchTeamsItemsList.Add(new MatchTeamWithPlayers
                            {
                                teamId = item.teamId,
                                teamplayerList = MatchTeamsPlayerList,
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
                        MatchTeamsItemsList = matchTeamsItemsList;
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<error>(content);
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
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
                UserDialogs.Instance.HideLoading();
                DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
            }
        }


        async Task createMatchplayersAsync()
        {
            try
            {
                var teamPlayerId = string.Join(",", TeamIdList);

                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "Match/createMatchplayer";
                    Uri requestUri = new Uri(RestURL);

                    var data = new CreateMatchPlayers
                    {
                        type = "Match",
                        eventId = App.User.CreateMatchId,
                        teamId = teamPlayerId,
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        //var Item = JsonConvert.DeserializeObject<createMatchResponse>(responJsonText);
                        var view = new SendInvitePoppup();
                        await PopupNavigation.Instance.PushAsync(view);
                        //After the success full api process clear all the values
                        //Clear();
                        UserDialogs.Instance.HideLoading();
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


        #region Team Item Tabbed Command Functionality

        //To Hide and UnHide Players details Page
        private MatchTeamWithPlayers _LastSelectedItem;

        public ICommand TeamItemsTabbedCommand => new Command(HideorShowItems);

        async void HideorShowItems(object parameter)
        {
            var Item = parameter as MatchTeamWithPlayers;

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

        async Task UpdateItems(MatchTeamWithPlayers Items)
        {
            var index = MatchTeamsItemsList.IndexOf(Items);
            MatchTeamsItemsList.Remove(Items);
            MatchTeamsItemsList.Insert(index, Items);
            OnPropertyChanged(nameof(MatchTeamsItemsList));
        }

        #endregion Team Item Tabbed Command Functionality

    }
}
