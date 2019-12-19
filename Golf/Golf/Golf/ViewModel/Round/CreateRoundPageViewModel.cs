using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views.CreateRoundView;
using Golf.Views.Rules;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Round
{
    public class CreateRoundPageViewModel : BaseViewModel
    {

        public CreateRoundPageViewModel()
        {
            MessagingCenter.Subscribe<App>((App)Application.Current, "OnCategoryCreated", (sender) => {
                GetRoundRulesList();
            });
            //Get the Competition type values
            GetCompetitionType();
            //Get the Round rules
            GetRoundRulesList();
        }

        #region Property Declaration

        public string RoundNameText
        {
            get
            {
                return _RoundNameText;
            }
            set
            {
                _RoundNameText = value;
                OnPropertyChanged(nameof(RoundNameText));
            }
        }
        private string _RoundNameText = string.Empty;

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

        public string RoundRuleID
        {
            get
            {
                return _RoundRuleID;
            }
            set
            {
                _RoundRuleID = value;
                OnPropertyChanged(nameof(RoundRuleID));
            }
        }
        private string _RoundRuleID = string.Empty;

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

        public DateTime? RoundStartDate
        {
            get
            {
                return _RoundStartDate;
            }
            set
            {
                _RoundStartDate = value;
                OnPropertyChanged(nameof(RoundStartDate));
            }
        }
        private DateTime? _RoundStartDate =null;

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

        public DateTime? RoundEndDate
        {
            get
            {
                return _RoundEndDate;
            }
            set
            {
                _RoundEndDate = value;
                OnPropertyChanged(nameof(RoundEndDate));
            }
        }
        private DateTime? _RoundEndDate = null;


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


        public TimeSpan? RoundStartTime
        {
            get
            {
                return _RoundStartTime;
            }
            set
            {
                _RoundStartTime = value;
                OnPropertyChanged(nameof(RoundStartTime));
            }
        }
        private TimeSpan? _RoundStartTime = null;


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

        public TimeSpan? RoundEndTime
        {
            get
            {
                return _RoundEndTime;
            }
            set
            {
                _RoundEndTime = value;
                OnPropertyChanged(nameof(RoundEndTime));
            }
        }
        private TimeSpan? _RoundEndTime  = null;


        public string RoundFee
        {
            get
            {
                return _RoundFee;
            }
            set
            {
                _RoundFee = value;
                OnPropertyChanged(nameof(RoundFee));
            }
        }
        private string _RoundFee = string.Empty;

        #endregion

        #region Proceed Button Command Functionality

        public bool IsValid { get; set; }

        public ICommand ProceedCommand => new AsyncCommand(ProceedAsync);

        async Task ProceedAsync()
        {
                IsValid = Validate();
                if (IsValid)
                {
                    await CreateRoundAsync();
                }
        }

        bool Validate()
        {
            //For Current Date Selection 
            if(NullableStartDate != null)
            {
                RoundStartDate = NullableStartDate;
            }
            if (NullableENdDate != null)
            {
                RoundEndDate = NullableENdDate;
            }
            if (string.IsNullOrEmpty(RoundNameText))
            {
                UserDialogs.Instance.AlertAsync("Round Name should not be empty.", "Alert", "Ok");
                return false;
            }
            else if (CompetitionTypeId == 0)
            {
                UserDialogs.Instance.AlertAsync("Please Select the Round Competition Type.", "Alert", "Ok");
                return false;
            }
            else if (RoundStartDate == null)
            {
                UserDialogs.Instance.AlertAsync("Please Select Round Start Date.", "Alert", "Ok");
                RoundStartDate = null;
                return false;
            }
            else if(RoundEndDate == null)
            {
                UserDialogs.Instance.AlertAsync("Please Select Round End Date.", "Alert", "Ok");
                RoundEndDate = null;
                return false;
            }
            else if (RoundEndDate < RoundStartDate)
            {
                UserDialogs.Instance.AlertAsync("End date can't be less then start date.", "Alert", "Ok");
                RoundEndDate = null;
                return false;
            }
            else if (string.IsNullOrEmpty(RoundFee))
            {
                UserDialogs.Instance.AlertAsync("Round Fee should not be empty.", "Alert", "Ok");
                return false;
            }
            else
            {
                return true;
            }
        }   


        async Task CreateRoundAsync()
        {
            try
            {
                RoundRuleID = string.Join(",", ListofRules);
                var RoundSdate = RoundStartDate.Value.ToString("yyyy/MM/dd");
                var RoundEdate = RoundEndDate.Value.ToString("yyyy/MM/dd");
                var RoundStartDateTime = RoundSdate + " " + RoundStartTime;
                var RoundEndDateTime = RoundEdate + " " + RoundEndTime;
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "Round/createRound";
                    Uri requestUri = new Uri(RestURL);

                    var data = new CreateRound
                    {
                        roundName = RoundNameText,
                        roundStartDate = RoundStartDateTime,
                        roundEndDate = RoundEndDateTime,
                        createdBy = App.User.UserId,
                        roundFee = Convert.ToInt32(RoundFee),
                        roundRuleId = RoundRuleID,
                        competitionTypeId = CompetitionTypeId,
                        isSaveAndNotify = false,
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var Item = JsonConvert.DeserializeObject<createRoundResponse>(responJsonText);
                        App.User.CreateRoundId = Item.roundId;
                        App.User.RoundCode = Item.roundCode;
                        App.User.CompetitionType = Item.competitionTypeName;
                        var view = new RoundContextSettingsPage();
                        var navigationPage = ((NavigationPage)App.Current.MainPage);
                        await navigationPage.PushAsync(view);
                        UserDialogs.Instance.HideLoading();
                        Clear();
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
            RoundNameText = string.Empty;
            RoundStartDate = null;
            RoundEndDate = null;
            RoundFee = string.Empty;
            NullableStartDate = null;
            NullableENdDate = null;
            RoundRuleID = string.Empty;
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
                    var RestURL = App.User.BaseUrl + "Round/getCompetitionType";
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var Items = JsonConvert.DeserializeObject<List<CompetitionType>>(content);
                        CompetitionTypeItems = Items;
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

        #endregion CompetitionType Command Functionality

        #region GetRoundRulesList Command Functionality


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

        async void GetRoundRulesList()
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
                    if (response.IsSuccessStatusCode)
                    {
                        var Items = JsonConvert.DeserializeObject<ObservableCollection<RoundRules>>(content);
                        RulesItems = Items;
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

        #endregion GetRoundRulesList Command Functionality

        #region RoundRule CheckBox Clicked Command Functionality

        public ICommand CheckBoxSelectedCommand => new Command<RoundRules>(CheckboxChangedEvent);

        public List<string> ListofRules = new List<string>();
        void CheckboxChangedEvent(RoundRules item)
        {
            try 
            { 
                var RoundRuleId = item.roundRuleId;
                if (ListofRules.Count > 0)
                {
                    bool UserIdAleradyExists = ListofRules.Contains(RoundRuleId);
                    if (UserIdAleradyExists)
                    {
                        ListofRules.Remove(RoundRuleId);
                    }
                    else
                    {
                        ListofRules.Add(RoundRuleId);
                    }
                }
                else
                {
                    ListofRules.Add(RoundRuleId);
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        #endregion RoundRule CheckBox Clicked Command Functionality

        #region CompetitionType SelectedIndex Changes Command Functionality

        public ICommand CompetitionTypeSelectedCommand => new Command<CompetitionType>(CompetitionTypeChangedEvent);
        void CompetitionTypeChangedEvent(CompetitionType item)
        {
                CompetitionTypeId = item.competitionTypeId;
        }

        #endregion CompetitionType SelectedIndex Changes Command Functionality      

        #region Round Start Date
        public ICommand RoundStartCommand => new Command<string>(RoundStartCommandChangedEvent);

        public string Roundstartdate;
        void RoundStartCommandChangedEvent(string parameter)
        {
            Roundstartdate = NullableStartDate.ToString();
        }

        #endregion

        #region Round StartEnd Date
        public ICommand RoundStartEndCommand => new Command<string>(RoundStartENdCommandChangedEvent);

        public string RoundstartEnddate;
        void RoundStartENdCommandChangedEvent(string parameter)
        {
                RoundstartEnddate = NullableENdDate.ToString();
        }
        #endregion

        #region Add Rule Popup
        public ICommand AddRuleCommand => new AsyncCommand(AddRuleCommandAsync);
        async Task AddRuleCommandAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new AddRulePoppupPage();
                await PopupNavigation.Instance.PushAsync(view);
                UserDialogs.Instance.HideLoading();
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
