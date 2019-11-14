using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views.CreateMatchView;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Match
{
    public class CreateMatchPageViewModel : BaseViewModel
    {

        public CreateMatchPageViewModel()
        {
            //Get the Competition type values
            GetCompetitionType();
            //Get the Match rules
            getMatchRulesList();
        }
        public bool IsValid { get; set; }
        public string MatchNameText
        {
            get
            {
                return _MatchNameText;
            }
            set
            {
                _MatchNameText = value;
                OnPropertyChanged(nameof(MatchNameText));
            }
        }
        private string _MatchNameText = string.Empty;

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

        public string MatchRuleID
        {
            get
            {
                return _MatchRuleID;
            }
            set
            {
                _MatchRuleID = value;
                OnPropertyChanged(nameof(MatchRuleID));
            }
        }
        private string _MatchRuleID = string.Empty;

        public DateTime? NullableStartDate
        {
            get { return _NullableStartDate; }
            set
            {
                _NullableStartDate = value;
                OnPropertyChanged(nameof(_NullableStartDate));
            }
        }
        private DateTime? _NullableStartDate = null;

        public DateTime? MatchStartDate
        {
            get
            {
                return _MatchStartDate;
            }
            set
            {
                _MatchStartDate = value;
                OnPropertyChanged(nameof(MatchStartDate));
            }
        }
        private DateTime? _MatchStartDate =null;


      



        public DateTime? NullableENdDate
        {
            get { return _NullableENdDate; }
            set
            {
                _NullableENdDate = value;
                OnPropertyChanged(nameof(NullableENdDate));
            }
        }
        private DateTime? _NullableENdDate = null;

        public DateTime? MatchEndDate
        {
            get
            {
                return _MatchEndDate;
            }
            set
            {
                _MatchEndDate = value;
                OnPropertyChanged(nameof(MatchEndDate));
            }
        }
        private DateTime? _MatchEndDate = null;


        public DateTime? NullableStartTime
        {
            get { return _NullableStartTime; }
            set
            {
                _NullableStartTime = value;
                OnPropertyChanged(nameof(NullableStartTime));
            }
        }
        private DateTime? _NullableStartTime = null;


        public TimeSpan? MatchStartTime
        {
            get
            {
                return _MatchStartTime;
            }
            set
            {
                _MatchStartTime = value;
                OnPropertyChanged(nameof(MatchStartTime));
            }
        }
        private TimeSpan? _MatchStartTime = null;


        public DateTime? NullableEndTime
        {
            get { return _NullableEndTime; }
            set
            {
                _NullableEndTime = value;
                OnPropertyChanged(nameof(NullableEndTime));
            }
        }
        private DateTime? _NullableEndTime = null;

        public TimeSpan? MatchEndTime
        {
            get
            {
                return _MatchEndTime;
            }
            set
            {
                _MatchEndTime = value;
                OnPropertyChanged(nameof(MatchEndTime));
            }
        }
        private TimeSpan? _MatchEndTime  = null;


        public string MatchFee
        {
            get
            {
                return _MatchFee;
            }
            set
            {
                _MatchFee = value;
                OnPropertyChanged(nameof(MatchFee));
            }
        }
        private string _MatchFee = string.Empty;


        #region Proceed Button Command Functionality
        public ICommand ProceedCommand => new AsyncCommand(ProceedAsync);
        async Task ProceedAsync()
        {
            IsValid = Validate();
            if (IsValid)
            {
                await CreateMatchAsync();
                try
                {
                   
                }
                catch (Exception ex)
                {
                    var a = ex.Message;
                }
            }
        }

        bool Validate()
        {
            if(string.IsNullOrEmpty(MatchNameText))
            {
                UserDialogs.Instance.AlertAsync("Match Name should not be empty.", "Alert", "Ok");
                return false;
            }
            else if (CompetitionTypeId == 0)
            {
                UserDialogs.Instance.AlertAsync("Please Select the Match Competition Type.", "Alert", "Ok");
                return false;
            }
            else if (NullableStartDate == null)
            {
                UserDialogs.Instance.AlertAsync("Please Select Match Start Date.", "Alert", "Ok");
                MatchStartDate = null;
                return false;
            }
            else if(NullableENdDate == null)
            {
                UserDialogs.Instance.AlertAsync("Please Select Match End Date.", "Alert", "Ok");
                MatchEndDate = null;
                return false;
            }
            //else if (NullableStartTime == null)
            //{
            //    UserDialogs.Instance.AlertAsync("Please Select Match Start Time.","Alert", "Ok");
            //    MatchStartTime = null;
            //    return false;
            //}
            //else if (NullableEndTime == null)
            //{
            //    UserDialogs.Instance.AlertAsync("Please Select Match Start Time.", "Alert", "Ok");
            //    NullableEndTime = null;
            //    return false;
            //}
            else if (string.IsNullOrEmpty(MatchFee))
            {
                UserDialogs.Instance.AlertAsync("Match Fee should not be empty.", "Alert", "Ok");
                return false;
            }
            else
            {
                return true;
            }
        }   


        async Task CreateMatchAsync()
        {
            try
            {
                MatchRuleID = string.Join(",", ListofRules);
                var matchStartDateTime = MatchStartDate.Value.ToShortDateString() + " " + MatchStartTime;
                var matchEndDateTime = MatchStartDate.Value.ToShortDateString() + " " + MatchEndTime;
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "Match/createMatch";
                    Uri requestUri = new Uri(RestURL);

                    var data = new CreateMatch
                    {
                        matchName = MatchNameText,
                        matchStartDate = matchStartDateTime,
                        matchEndDate = matchEndDateTime,
                        createdBy = App.User.UserWithTypeId,
                        matchFee = Convert.ToInt32(MatchFee),
                        matchRuleId = MatchRuleID,
                        competitionTypeId = CompetitionTypeId,
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var Item = JsonConvert.DeserializeObject<createMatchResponse>(responJsonText);
                        App.User.CreateMatchId = Item.matchId;
                        App.User.MatchCode = Item.matchCode;
                        App.User.CompetitionType = Item.competitionTypeName;
                        var view = new MatchContextSettingsPage();
                        var navigationPage = ((NavigationPage)App.Current.MainPage);
                        await navigationPage.PushAsync(view);
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

        void Clear()
        {
            MatchNameText = string.Empty;
            MatchStartDate = null;
            MatchEndDate = null;
            MatchFee = string.Empty;
            NullableStartDate = null;
            NullableENdDate = null;
            MatchRuleID = string.Empty;
            CompetitionTypeId = 0;
        }
        #endregion Proceed Button Command Functionality



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


        #region GetMatchRulesList Command Functionality


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
                    var Items = JsonConvert.DeserializeObject<ObservableCollection<MatchRules>>(content);
                    RulesItems = Items;
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

        public ICommand CheckBoxSelectedCommand => new Command(CheckboxChangedEvent);
        public List<string> ListofRules = new List<string>();
        void CheckboxChangedEvent(object parameter)
        {
            var item = parameter as MatchRules;
            var matchRuleId = item.matchRuleId;
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


        #region CompetitionType SelectedIndex Changes Command Functionality

        public ICommand CompetitionTypeSelectedCommand => new Command(CompetitionTypeChangedEvent);
        void CompetitionTypeChangedEvent(object parameter)
        {
            var item = parameter as CompetitionType;
            CompetitionTypeId = item.competitionTypeId;
        }
        #endregion CompetitionType SelectedIndex Changes Command Functionality

    }
}
