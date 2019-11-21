using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Newtonsoft.Json;
using Plugin.Connectivity;
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
    public class MatchDetailsPageViewModel : BaseViewModel
    {
        bool IsSuccess = false;

        public List<string> SettingsList { get; set; } 

        public List<string> MatchExtrasList { get; set; }

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

        public int CompetitionTypeId
        {
            get
            {
                return _CompetitionTypeId;
            }
            set
            {
                _CompetitionTypeId = value;
                OnPropertyChanged(nameof(CompetitionTypeId));
            }
        }
        private int _CompetitionTypeId = 0;

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

        public Color MatchDetailsBackground
        {
            get { return _MatchDetailsBackground; }
            set
            {
                _MatchDetailsBackground = value;
                OnPropertyChanged(nameof(MatchDetailsBackground));
            }
        }
        private Color _MatchDetailsBackground = (Color)App.Current.Resources["LightGreenColor"];

        public Color TeamListBackground
        {
            get { return _TeamListBackground; }
            set
            {
                _TeamListBackground = value;
                OnPropertyChanged(nameof(TeamListBackground));
            }
        }
        private Color _TeamListBackground = Color.White;

        public Color MatchDetailsBorder
        {
            get { return _MatchDetailsBorder; }
            set
            {
                _MatchDetailsBorder = value;
                OnPropertyChanged(nameof(MatchDetailsBorder));
            }
        }
        private Color _MatchDetailsBorder = Color.White;

        public Color TeamListBorder
        {
            get { return _TeamListBorder; }
            set
            {
                _TeamListBorder = value;
                OnPropertyChanged(nameof(TeamListBorder));
            }
        }
        private Color _TeamListBorder = (Color)App.Current.Resources["LightGreenColor"];


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

        public int CompetitionTypeID
        {
            get { return _CompetitionTypeID; }
            set
            {
                _CompetitionTypeID = value;
                OnPropertyChanged(nameof(CompetitionTypeID));
            }
        }
        private int _CompetitionTypeID = 0;

        public ObservableCollection<MatchRules> RulesItems
        {
            get { return _RulesItems; }
            set
            {
                _RulesItems = value;
                OnPropertyChanged(nameof(RulesItems));
            }
        }
        private ObservableCollection<MatchRules> _RulesItems = null;

        public MatchDetailsPageViewModel()
        {
            SettingsList = new List<string>();
            SettingsList.Add("Carryover - if no one wins the hole");
            SettingsList.Add("N tie");
            SettingsList.Add("Greater than 1 or equal to");
            SettingsList.Add("No Carryover");

            MatchExtrasList = new List<string>();
            MatchExtrasList.Add("Greenies");
            MatchExtrasList.Add("Skins");
            MatchExtrasList.Add("Closet to the pin");
            MatchExtrasList.Add("Longest drive");

            //Get the Competition type values
            GetCompetitionType();
            //Get the Match rules
            getMatchRulesList();
            //Using THis Method to Load the Match details
            getMatchById();
            //Using THis Method to Load the Match Team And Players List From an API.
            getMatchesDetailsById();
        }

        #region Match Details Button Command Functionality
        public ICommand MatchDetailsCommand => new AsyncCommand(MatchDetailsAsync);

        async Task MatchDetailsAsync()
        {
            IsVisibleMatchDetails = true;
            IsVisibleTeamsDetails = false;
            MatchDetailsBackground = (Color)App.Current.Resources["LightGreenColor"];
            TeamListBorder = (Color)App.Current.Resources["LightGreenColor"];
            MatchDetailsBorder =  Color.White;
            TeamListBackground = Color.White;
        }
        #endregion Match Details Button Command Functionality

        #region CompetitionType SelectedIndex Changes Command Functionality
        public ICommand CompetitionTypeSelectedCommand => new Command(CompetitionTypeChangedEvent);
        void CompetitionTypeChangedEvent(object parameter)
        {
            var item = parameter as CompetitionType;
            CompetitionTypeId = item.competitionTypeId;
        }
        #endregion CompetitionType SelectedIndex Changes Command Functionality

        #region Team Button Command Functionality

        public ICommand TeamCommand => new AsyncCommand(TeamAsync);

        async Task TeamAsync()
        {
            UserDialogs.Instance.ShowLoading();
            IsVisibleMatchDetails = false;
            IsVisibleTeamsDetails = true;

            TeamListBackground = (Color)App.Current.Resources["LightGreenColor"];
            TeamListBorder = Color.White;
            MatchDetailsBackground = Color.White;
            MatchDetailsBorder = (Color)App.Current.Resources["LightGreenColor"];
           
            UserDialogs.Instance.HideLoading();
        }

        public List<MatchDetailsListTeamList> matchTeamsItemsList = new List<MatchDetailsListTeamList>();

        public ObservableCollection<MatchDetailsListTeamList> MatchTeamsItemsList
        {
            get { return _MatchTeamsItemsList; }
            set
            {
                _MatchTeamsItemsList = value;
                OnPropertyChanged(nameof(MatchTeamsItemsList));
            }
        }
        private ObservableCollection<MatchDetailsListTeamList> _MatchTeamsItemsList = null;

        async void getMatchesDetailsById()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    //player type is 1 to get player list
                    var RestURL = App.User.BaseUrl + "Match/getMatchesDetailsById/" +App.User.MatchId;
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    //List<MatchDetailsListTeamList> matchTeamsItemsListssss = new List<MatchDetailsListTeamList>();
                    //Assign the Values to Listview
                    if (response.IsSuccessStatusCode)
                    {
                        var Items = JsonConvert.DeserializeObject<ObservableCollection<getMatchesDetailsById>>(content).ToList();


                        matchTeamsItemsList.AddRange((from item in Items
                                           select new MatchDetailsListTeamList
                                           {
                                               teamId = item.teamId,
                                               matchPlayerList = JsonConvert.DeserializeObject<List<matchPlayerList>>(item.matchPlayerList),
                                               createdByName = item.createdByName,
                                               NoOfPlayers = item.NoOfPlayers,
                                               teamIcon = item.teamIcon,
                                               teamName = item.teamName,
                                               Expanded = false
                                           }).ToList());

                        //foreach (var item in Items)
                        //{
                        //    var MatchTeamsPlayerList = JsonConvert.DeserializeObject<List<matchPlayerList>>(item.matchPlayerList);
                        //    matchTeamsItemsList.Add(new MatchDetailsListTeamList
                        //    {
                        //        teamId = item.teamId,
                        //        matchPlayerList = MatchTeamsPlayerList,
                        //        createdByName = item.createdByName,
                        //        NoOfPlayers = item.NoOfPlayers,
                        //        teamIcon = item.teamIcon,
                        //        teamName = item.teamName,
                        //        Expanded = false
                        //    });
                        //}
                        ObservableCollection<MatchDetailsListTeamList> myCollection = new ObservableCollection<MatchDetailsListTeamList>(matchTeamsItemsList as List<MatchDetailsListTeamList>);
                        MatchTeamsItemsList = myCollection;
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

        #endregion Team Button Command Functionality

        #region Team Item Tabbed Command Functionality

        //To Hide and UnHide Players details Page
        private MatchDetailsListTeamList _LastSelectedItem;

        public ICommand TeamItemsTabbedCommand => new Command(HideorShowItems);

        async void HideorShowItems(object parameter)
        {
            var Item = parameter as MatchDetailsListTeamList;

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

        async Task UpdateItems(MatchDetailsListTeamList Items)
        {
            var index = MatchTeamsItemsList.IndexOf(Items);
            MatchTeamsItemsList.Remove(Items);
            MatchTeamsItemsList.Insert(index, Items);
            OnPropertyChanged(nameof(MatchTeamsItemsList));
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

        #region CompetitionType Command Functionality

        public List<CompetitionType> CompetitionTypeItems
        {
            get { return _CompetitionTypeItems; }
            set
            {
                _CompetitionTypeItems = value;
                OnPropertyChanged(nameof(CompetitionTypeItems));
            }
        }
        private List<CompetitionType> _CompetitionTypeItems = null;

        async void GetCompetitionType()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "Match/getCompetitionType";
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<List<CompetitionType>>(content);
                    CompetitionTypeItems = Items;
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

        #endregion CompetitionType Command Functionality



        #region Save Button Command Functionality

        public ICommand SaveButtonCommand => new AsyncCommand(SaveButtonAsync);

        async Task SaveButtonAsync()
        {
            await updateMatch(false);
        }

        async Task updateMatch(bool IsSaveAndNotify)
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
                        matchId = Convert.ToInt32(App.User.MatchId),
                        competitionTypeId = CompetitionTypeID,
                        matchRuleId = string.Join(",", ListofRules),
                        isSaveAndNotify = IsSaveAndNotify
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        //var Item = JsonConvert.DeserializeObject<CreateTeamResponse>(responJsonText);
                        //App.User.CreateTeamId = Item.teamId;
                        //var view = new MenuPage();
                        //var navigationPage = ((NavigationPage)App.Current.MainPage);
                        //await navigationPage.PushAsync(view);
                        ////After the success full api process clear all the values
                        //Clear();
                        UserDialogs.Instance.Alert("Match details updated successfully.", "Match Details", "Ok");
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
            await updateMatch(true);
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
                        MatchLocation = MatchDetails.matchLocation;
                        CompetitionTypeID = MatchDetails.competitionTypeId;
                        await LoadRulesType(MatchDetails.matchRuleId);
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

        public async Task LoadRulesType(string Rules)
        {
            List<string> result = Rules.Split(',').ToList();
            foreach (string item in result)
            {
                RulesItems.Where(w => w.matchRuleId == item).ToList().ForEach(s => s.Checked = true);
            }
        }
        #region GetMatchRulesList Command Functionality

        async void getMatchRulesList()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "Match/getMatchRulesList";
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    RulesItems = JsonConvert.DeserializeObject<ObservableCollection<MatchRules>>(content);
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

        #endregion GetMatchRulesList Command Functionality

        #region MatchRule CheckBox Clicked Command Functionality

        public ICommand CheckBoxSelectedCommand => new Command<string>(CheckboxChangedEvent);
        public List<string> ListofRules = new List<string>();
        void CheckboxChangedEvent(string matchRuleId)
        {
            if (ListofRules.Count > 0)
            {
                bool UserIdAleradyExists = ListofRules.Contains(matchRuleId);
                if (UserIdAleradyExists)
                {
                    ListofRules.Remove(matchRuleId);
                }
                else
                {
                    ListofRules.Add(matchRuleId);
                }
            }
            else
            {
                ListofRules.Add(matchRuleId);
            }
        }

        #endregion MatchRule CheckBox Clicked Command Functionality
    }
}
