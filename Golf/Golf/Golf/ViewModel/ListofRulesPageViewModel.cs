using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views.Rules;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class ListofRulesPageViewModel : BaseViewModel
    {
        public ListofRulesPageViewModel()
        {
            MessagingCenter.Subscribe<App>((App)Application.Current, "OnCategoryCreated", (sender) => {
                getRoundRulesList();
            });

            getRoundRulesList();
        }

        #region Property Declaration

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

        private ObservableCollection<RoundRules> OriginalRulesList = new ObservableCollection<RoundRules>();

        #endregion

        #region LoadRulesCommand

        public async void getRoundRulesList()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();

                    var result = await App.ApiClient.GetRulesList();

                    if (result != null)
                    {
                        RulesItems = result;
                        OriginalRulesList = RulesItems;
                    }

                    UserDialogs.Instance.HideLoading();


                    //var RestURL = App.User.BaseUrl + "Round/getRoundRulesList";
                    //var httpClient = new HttpClient();
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    //var response = await httpClient.GetAsync(RestURL);
                    //var content = await response.Content.ReadAsStringAsync();
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    RulesItems = JsonConvert.DeserializeObject<ObservableCollection<RoundRules>>(content);
                    //    OriginalRulesList = RulesItems;
                    //    UserDialogs.Instance.HideLoading();
                    //}
                    //else
                    //{
                    //    var error = JsonConvert.DeserializeObject<error>(content);
                    //    UserDialogs.Instance.HideLoading();
                    //    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                    //}
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

        #region AddRule Command

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
                UserDialogs.Instance.HideLoading();
                var a = ex.Message;
            }
        }

        #endregion

        #region Serach Command

        private ObservableCollection<RoundRules> RulesListItems = new ObservableCollection<RoundRules>();

        public ICommand SearchCommand => new Command<string>(Search);

        public async void Search(string keyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    RulesItems = new ObservableCollection<RoundRules>();

                    RulesListItems = new ObservableCollection<RoundRules>();

                    var query = OriginalRulesList.Where(x => x.ruleName.ToLower().StartsWith(keyword.ToLower()) || x.ruleName.ToLower().Contains(keyword.ToLower()));

                    foreach (var item in query)
                    {
                        var value = new RoundRules() { roundRuleId = item.roundRuleId, ruleName = item.ruleName, Checked = item.Checked };

                        RulesListItems.Add(value);
                    }

                    RulesItems = RulesListItems;
                }
                else
                {
                    RulesItems = OriginalRulesList;
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
