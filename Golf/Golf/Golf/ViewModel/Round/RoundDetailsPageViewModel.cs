﻿using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views;
using Golf.Views.UpdateTeamView;
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

namespace Golf.ViewModel.Round
{
    public class RoundDetailsPageViewModel : BaseViewModel
    {
        bool IsSuccess = false;

        public List<string> SettingsList { get; set; }

        public List<string> RoundExtrasList { get; set; }

        public getRoundById RoundDetails;
        //To Hide and UnHide Math details Page
        public bool IsVisibleRoundDetails
        {
            get { return _IsVisibleRoundDetails; }
            set
            {
                _IsVisibleRoundDetails = value;
                OnPropertyChanged(nameof(IsVisibleRoundDetails));
            }
        }
        private bool _IsVisibleRoundDetails = true;

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

        public string RoundName
        {
            get { return _RoundName; }
            set
            {
                _RoundName = value;
                OnPropertyChanged(nameof(RoundName));
            }
        }
        private string _RoundName = string.Empty;

        public string RoundCode
        {
            get { return _RoundCode; }
            set
            {
                _RoundCode = value;
                OnPropertyChanged(nameof(RoundCode));
            }
        }
        private string _RoundCode = string.Empty;

        public Color RoundDetailsBackground
        {
            get { return _RoundDetailsBackground; }
            set
            {
                _RoundDetailsBackground = value;
                OnPropertyChanged(nameof(RoundDetailsBackground));
            }
        }
        private Color _RoundDetailsBackground = (Color)App.Current.Resources["LightGreenColor"];

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

        public Color RoundDetailsBorder
        {
            get { return _RoundDetailsBorder; }
            set
            {
                _RoundDetailsBorder = value;
                OnPropertyChanged(nameof(RoundDetailsBorder));
            }
        }
        private Color _RoundDetailsBorder = Color.White;

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

        public string RoundLocation
        {
            get { return _RoundLocation; }
            set
            {
                _RoundLocation = value;
                OnPropertyChanged(nameof(RoundLocation));
            }
        }
        private string _RoundLocation = string.Empty;

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

        public ObservableCollection<RoundRules> RulesItems
        {
            get { return _RulesItems; }
            set
            {
                _RulesItems = value;
                OnPropertyChanged(nameof(RulesItems));
            }
        }
        private ObservableCollection<RoundRules> _RulesItems = null;

        public string selectedItem { get; set; }
        public string RoundselectedItem { get; set; }

        public RoundDetailsPageViewModel()
        {
            SettingsList = new List<string>();
            SettingsList.Add("Carryover - if no one wins the hole");
            SettingsList.Add("N tie");
            SettingsList.Add("Greater than 1 or equal to");
            SettingsList.Add("No Carryover");

            RoundExtrasList = new List<string>();
            RoundExtrasList.Add("Greenies");
            RoundExtrasList.Add("Skins");
            RoundExtrasList.Add("Closet to the pin");
            RoundExtrasList.Add("Longest drive");

            selectedItem = "Carryover - if no one wins the hole";
            RoundselectedItem = "Greenies";
            //Get the Competition type values
            GetCompetitionType();
            //Get the Round rules
            getRoundRulesList();
            //Using THis Method to Load the Round details
            getRoundById();
            //Using THis Method to Load the Round Team And Players List From an API.
            getRoundsDetailsById();
        }

        #region Round Details Button Command Functionality
        public ICommand RoundDetailsCommand => new AsyncCommand(RoundDetailsAsync);

        async Task RoundDetailsAsync()
        {
            IsVisibleRoundDetails = true;
            IsVisibleTeamsDetails = false;
            RoundDetailsBackground = (Color)App.Current.Resources["LightGreenColor"];
            TeamListBorder = (Color)App.Current.Resources["LightGreenColor"];
            RoundDetailsBorder = Color.White;
            TeamListBackground = Color.White;
        }
        #endregion Round Details Button Command Functionality

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
            IsVisibleRoundDetails = false;
            IsVisibleTeamsDetails = true;

            TeamListBackground = (Color)App.Current.Resources["LightGreenColor"];
            TeamListBorder = Color.White;
            RoundDetailsBackground = Color.White;
            RoundDetailsBorder = (Color)App.Current.Resources["LightGreenColor"];

            UserDialogs.Instance.HideLoading();
        }

        public List<RoundDetailsListTeamList> roundTeamsItemsList = new List<RoundDetailsListTeamList>();

        public ObservableCollection<RoundDetailsListTeamList> RoundTeamsItemsList
        {
            get { return _RoundTeamsItemsList; }
            set
            {
                _RoundTeamsItemsList = value;
                OnPropertyChanged(nameof(RoundTeamsItemsList));
            }
        }
        private ObservableCollection<RoundDetailsListTeamList> _RoundTeamsItemsList = null;

        async void getRoundsDetailsById()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    //player type is 1 to get player list
                    var RestURL = App.User.BaseUrl + "Round/getRoundDetailsById/" + App.User.RoundId;
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    //List<RoundDetailsListTeamList> roundTeamsItemsListssss = new List<RoundDetailsListTeamList>();
                    //Assign the Values to Listview
                    if (response.IsSuccessStatusCode)
                    {
                        var Items = JsonConvert.DeserializeObject<ObservableCollection<getRoundsDetailsById>>(content).ToList();


                        roundTeamsItemsList.AddRange((from item in Items
                                                      select new RoundDetailsListTeamList
                                                      {
                                                          teamId = item.teamId,
                                                          roundPlayerList = JsonConvert.DeserializeObject<List<roundPlayerList>>(item.roundPlayerList),
                                                          createdByName = item.createdByName,
                                                          NoOfPlayers = item.NoOfPlayers,
                                                          teamIcon = item.teamIcon,
                                                          teamName = item.teamName,
                                                          Expanded = false
                                                      }).ToList());

                        //foreach (var item in Items)
                        //{
                        //    var RoundTeamsPlayerList = JsonConvert.DeserializeObject<List<roundPlayerList>>(item.roundPlayerList);
                        //    roundTeamsItemsList.Add(new RoundDetailsListTeamList
                        //    {
                        //        teamId = item.teamId,
                        //        roundPlayerList = RoundTeamsPlayerList,
                        //        createdByName = item.createdByName,
                        //        NoOfPlayers = item.NoOfPlayers,
                        //        teamIcon = item.teamIcon,
                        //        teamName = item.teamName,
                        //        Expanded = false
                        //    });
                        //}
                        ObservableCollection<RoundDetailsListTeamList> myCollection = new ObservableCollection<RoundDetailsListTeamList>(roundTeamsItemsList as List<RoundDetailsListTeamList>);
                        RoundTeamsItemsList = myCollection;
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
        private RoundDetailsListTeamList _LastSelectedItem;

        public ICommand TeamItemsTabbedCommand => new Command(HideorShowItems);

        async void HideorShowItems(object parameter)
        {
            var Item = parameter as RoundDetailsListTeamList;

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

        async Task UpdateItems(RoundDetailsListTeamList Items)
        {
            var index = RoundTeamsItemsList.IndexOf(Items);
            RoundTeamsItemsList.Remove(Items);
            RoundTeamsItemsList.Insert(index, Items);
            OnPropertyChanged(nameof(RoundTeamsItemsList));
        }


        public ObservableCollection<RoundTeamItems> RoundTeamsItemsWithPlayers
        {
            get { return _RoundTeamsItemsWithPlayers; }
            set
            {
                _RoundTeamsItemsWithPlayers = value;
                OnPropertyChanged(nameof(RoundTeamsItemsWithPlayers));
            }
        }
        private ObservableCollection<RoundTeamItems> _RoundTeamsItemsWithPlayers = null;
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
                        RoundTeamsItemsWithPlayers = JsonConvert.DeserializeObject<ObservableCollection<RoundTeamItems>>(content);
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
                    var RestURL = App.User.BaseUrl + "Round/getCompetitionType";
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
            await updateRound(false);
        }

        async Task updateRound(bool IsSaveAndNotify)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "Round/updateRound";
                    Uri requestUri = new Uri(RestURL);

                    var data = new CreateRound
                    {
                        roundId = Convert.ToInt32(App.User.RoundId),
                        competitionTypeId = CompetitionTypeID,
                        roundRuleId = string.Join(",", ListofRules),
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
                        UserDialogs.Instance.Alert("Round details updated successfully.", "Round Details", "Ok");
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
            await updateRound(true);
        }

        #endregion Save Button Command Functionality

        async void getRoundById()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "Round/getRoundById/" + App.User.RoundId;
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        RoundDetails = JsonConvert.DeserializeObject<getRoundById>(content);
                        RoundName = RoundDetails.roundName;
                        RoundCode = RoundDetails.roundCode;
                        CompetitionType = RoundDetails.competitionName;
                        RoundLocation = RoundDetails.roundLocation;
                        CompetitionTypeID = RoundDetails.competitionTypeId;
                        await LoadRulesType(RoundDetails.roundRuleId);
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
                RulesItems.Where(w => w.roundRuleId == item).ToList().ForEach(s => s.Checked = true);
            }
        }

        #region GetRoundRulesList Command Functionality

        async void getRoundRulesList()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "Round/getRoundRulesList";
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    RulesItems = JsonConvert.DeserializeObject<ObservableCollection<RoundRules>>(content);
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

        #endregion GetRoundRulesList Command Functionality

        #region Rule CheckBox Clicked Command Functionality

        public ICommand CheckBoxSelectedCommand => new Command<string>(CheckboxChangedEvent);
        public List<string> ListofRules = new List<string>();
        void CheckboxChangedEvent(string roundRuleId)
        {
            try
            {
                if (ListofRules.Count > 0)
                {
                    bool UserIdAleradyExists = ListofRules.Contains(roundRuleId);
                    if (UserIdAleradyExists)
                    {
                        ListofRules.Remove(roundRuleId);
                    }
                    else
                    {
                        ListofRules.Add(roundRuleId);
                    }
                }
                else
                {
                    ListofRules.Add(roundRuleId);
                }
            }
            catch(Exception ex)
            {
                var a = ex.Message;
            }
        }

        #endregion RoundRule CheckBox Clicked Command Functionality

        #region TeamEdit Command
        public ICommand EditTeamCommand => new Command(EditTeam);

        async void EditTeam(object parameter)
        {
            try
            {
                var Item = parameter as RoundDetailsListTeamList;
                App.User.CreateTeamId = Item.teamId;
                var view = new UpdateTeam();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
            }
            catch(Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion

        #region AddNewTeam Command
        
        public ICommand AddNewTeamCommand => new Command(AddNewTeam);

        async void AddNewTeam()
        {
            try
            {
                var view = new CreateTeamPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion
    }
}
