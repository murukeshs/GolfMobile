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


        #region SaveTeamButtonCLicked Command Functionality

        public ICommand SaveTeamsCommand => new AsyncCommand(SaveTeamButtonCLicked);

        private async Task SaveTeamButtonCLicked()
        {
            //ViewModelNavigation.PushAsync(new ItemPageViewModel() { Item = parameter as RssItem });
            await createRoundplayersAsync();
        }
        #endregion SaveTeamButtonCLicked Command Functionality


        public List<int> TeamIdList = new List<int>();
        public ICommand CheckBoxSelectedCommand => new Command(CheckBoxChanged);

        async void CheckBoxChanged(object parameter)
        {
            var item = parameter as RoundTeamWithPlayers;
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
                        var Items = JsonConvert.DeserializeObject<ObservableCollection<RoundTeamItems>>(content);
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


        async Task createRoundplayersAsync()
        {
            try
            {
                var teamPlayerId = string.Join(",", TeamIdList);

                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "Round/createRoundplayer";
                    Uri requestUri = new Uri(RestURL);

                    var data = new CreateRoundPlayers
                    {
                        type = "Round",
                        eventId = App.User.CreateRoundId,
                        teamId = teamPlayerId,
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        //var Item = JsonConvert.DeserializeObject<createRoundResponse>(responJsonText);
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
        private RoundTeamWithPlayers _LastSelectedItem;

        public ICommand TeamItemsTabbedCommand => new Command(HideorShowItems);

        async void HideorShowItems(object parameter)
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

        async Task UpdateItems(RoundTeamWithPlayers Items)
        {
            var index = RoundTeamsItemsList.IndexOf(Items);
            RoundTeamsItemsList.Remove(Items);
            RoundTeamsItemsList.Insert(index, Items);
            OnPropertyChanged(nameof(RoundTeamsItemsList));
        }

        #endregion Team Item Tabbed Command Functionality

    }
}
