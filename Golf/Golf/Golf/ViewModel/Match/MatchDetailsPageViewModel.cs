using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Match
{
    public class MatchDetailsPageViewModel : BaseViewModel
    {
        bool IsSuccess = false;

        public getMatchById MatchDetails;
        //To Hide and UnHide Math details Page
        public bool IsVisibleMatchDetails
        {
            get { return _IsVisibleMatchDetails; }
            set
            {
                _IsVisibleMatchDetails = value;
                OnPropertyChanged(nameof(IsVisibleMatchDetails));
            }
        }
        private bool _IsVisibleMatchDetails = true;

        //To Hide and UnHide Teams details Page
        public bool IsVisibleTeamsDetails
        {
            get { return _IsVisibleTeamsDetails; }
            set
            {
                _IsVisibleTeamsDetails = value;
                OnPropertyChanged(nameof(IsVisibleTeamsDetails));
            }
        }
        private bool _IsVisibleTeamsDetails = false;

        public string MatchName
        {
            get { return _MatchName; }
            set
            {
                _MatchName = value;
                OnPropertyChanged(nameof(MatchName));
            }
        }
        private string _MatchName = string.Empty;

        public string MatchCode
        {
            get { return _MatchCode; }
            set
            {
                _MatchCode = value;
                OnPropertyChanged(nameof(MatchCode));
            }
        }
        private string _MatchCode = string.Empty;


        public string CompetitionType
        {
            get { return _CompetitionType; }
            set
            {
                _CompetitionType = value;
                OnPropertyChanged(nameof(CompetitionType));
            }
        }
        private string _CompetitionType = string.Empty;

        public string TypeofGame
        {
            get { return _TypeofGame; }
            set
            {
                _TypeofGame = value;
                OnPropertyChanged(nameof(TypeofGame));
            }
        }
        private string _TypeofGame = string.Empty;

        public string MatchLocation
        {
            get { return _MatchLocation; }
            set
            {
                _MatchLocation = value;
                OnPropertyChanged(nameof(MatchLocation));
            }
        }
        private string _MatchLocation = string.Empty;

        public string ContextSettings
        {
            get { return _ContextSettings; }
            set
            {
                _ContextSettings = value;
                OnPropertyChanged(nameof(ContextSettings));
            }
        }
        private string _ContextSettings = "Dummy";

        public MatchDetailsPageViewModel()
        {
            //Using THis Method to Load the Match details
            getMatchById();
        }

        #region Match Details Button Command Functionality
        public ICommand MatchDetailsCommand => new AsyncCommand(MatchDetailsAsync);

        async Task MatchDetailsAsync()
        {
            IsVisibleMatchDetails = true;
            IsVisibleTeamsDetails = false;
        }
        #endregion Match Details Button Command Functionality


        #region Team Button Command Functionality

        public ICommand TeamCommand => new AsyncCommand(TeamAsync);

        async Task TeamAsync()
        {
            UserDialogs.Instance.ShowLoading();
            IsVisibleMatchDetails = false;
            //IsVisiblePlayersDetails = false;
            IsVisibleTeamsDetails = true;
            UserDialogs.Instance.HideLoading();
        }

        async Task GetTeamPlayersList()
        {

        }

        #endregion Team Button Command Functionality

        #region Team Item Tabbed Command Functionality

        //To Hide and UnHide Players details Page
        //private MatchTeamList _LastSelectedItem;

        public ICommand TeamItemsTabbedCommand => new Command(HideorShowItems);

        async void HideorShowItems(object parameter)
        {
            //var Item = parameter as MatchTeamList;

            //if(_LastSelectedItem == Item)
            //{
            //    Item.IsVisible = !Item.IsVisible;
            //    await UpdateItems(Item);
            //}
            //else
            //{
            //    if(_LastSelectedItem != null)
            //    {
            //        //hide the previous selected item
            //        _LastSelectedItem.IsVisible = false;
            //        await UpdateItems(_LastSelectedItem);
            //    }
            //    //Or show the selected item
            //    Item.IsVisible = true;
            //    await UpdateItems(Item);
            //}
            //_LastSelectedItem = Item;
        }

        async Task UpdateItems(MatchTeamItems Items)
        {
            //var index = MatchTeamsItems.IndexOf(Items);
            //MatchTeamsItems.Remove(Items);
            //MatchTeamsItems.Insert(index, Items);
            //OnPropertyChanged(nameof(MatchTeamsItems));
        }
        

        public ObservableCollection<MatchTeamItems> MatchTeamsItemsWithPlayers
        {
            get { return _MatchTeamsItemsWithPlayers; }
            set
            {
                _MatchTeamsItemsWithPlayers = value;
                OnPropertyChanged(nameof(MatchTeamsItemsWithPlayers));
            }
        }
        private ObservableCollection<MatchTeamItems> _MatchTeamsItemsWithPlayers = null;
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
                    if (response.IsSuccessStatusCode)
                    {
                        MatchTeamsItemsWithPlayers = JsonConvert.DeserializeObject<ObservableCollection<MatchTeamItems>>(content);
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

        #endregion Team Item Tabbed Command Functionality



        #region Save Button Command Functionality

        public ICommand SaveButtonCommand => new AsyncCommand(SaveButtonAsync);

        async Task SaveButtonAsync()
        {
            UserDialogs.Instance.ShowLoading();
            await updateMatch();
            UserDialogs.Instance.HideLoading();
        }


        async Task updateMatch()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "Match/updateMatch";
                    Uri requestUri = new Uri(RestURL);

                    var data = new CreateMatch
                    {
                        matchName = MatchName,
                        matchCode = MatchCode,
                        matchId = Convert.ToInt32(App.User.MatchId),
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var Item = JsonConvert.DeserializeObject<CreateTeamResponse>(responJsonText);
                        //App.User.CreateTeamId = Item.teamId;
                        //var view = new MenuPage();
                        //var navigationPage = ((NavigationPage)App.Current.MainPage);
                        //await navigationPage.PushAsync(view);
                        ////After the success full api process clear all the values
                        //Clear();
                        IsSuccess = true;
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        IsSuccess = false;
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


        #endregion Save Button Command Functionality


        #region SaveAndNotify Button Command Functionality

        public ICommand SaveAndNotifyButtonCommand => new AsyncCommand(SaveAndNotifyButtonAsync);

        async Task SaveAndNotifyButtonAsync()
        {
            UserDialogs.Instance.ShowLoading();
            await updateMatch();
            if(IsSuccess)
            {
                SendInviteAPI();
            }
            UserDialogs.Instance.HideLoading();
        }

        async void SendInviteAPI()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "Match/inviteMatch/" + App.User.MatchId;
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.Alert("Invite send to all the participants successfully.", "Success", "ok");
                        //var view = new MenuPage();
                        //var navigationPage = ((NavigationPage)App.Current.MainPage);
                        //await navigationPage.PushAsync(view);
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

        #endregion Save Button Command Functionality


        async void getMatchById()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "Match/getMatchById/" + App.User.MatchId;
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        MatchDetails = JsonConvert.DeserializeObject<getMatchById>(content);
                        MatchName = MatchDetails.matchName;
                        MatchCode = MatchDetails.matchCode;
                        CompetitionType = MatchDetails.competitionName;
                        TypeofGame = MatchDetails.ruleName;
                        MatchLocation = MatchDetails.matchLocation;
                        //UserDialogs.Instance.Alert("Invite send to all the participants successfully.", "Success", "ok");
                        //var view = new MenuPage();
                        //var navigationPage = ((NavigationPage)App.Current.MainPage);
                        //await navigationPage.PushAsync(view);
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("Something went wrong, please try again later", "ok");
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
    }
}
