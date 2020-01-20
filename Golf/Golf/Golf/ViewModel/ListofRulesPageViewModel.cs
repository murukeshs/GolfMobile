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
        public ICommand  SearchCommand => new Command<string>(Search);
        public async void Search(string keyword)
        { 
            try
            {
                RulesItems = new ObservableCollection<RoundRules>();

                if (!string.IsNullOrEmpty(keyword))
                {
                    RulesItems = new ObservableCollection<RoundRules>(OriginalRulesList.Where(x => x.ruleName.ToLower().StartsWith(keyword.ToLower()) || x.ruleName.ToLower().Contains(keyword.ToLower())));
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
