using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views.Rules;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
                RefreshData();
            });
            getMatchRulesList();
        }

        void RefreshData()
        {
            getMatchRulesList();
        }

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

        public async void getMatchRulesList()
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

        //public ICommand AddRuleCancelCommand => new AsyncCommand(AddRuleCancelCommandAsync);
        //async Task AddRuleCancelCommandAsync()
        //{
        //    try
        //    {
        //        UserDialogs.Instance.ShowLoading();
        //        await PopupNavigation.Instance.PopAsync(true);
        //        UserDialogs.Instance.HideLoading();
        //    }
        //    catch (Exception ex)
        //    {
        //        var a = ex.Message;
        //        UserDialogs.Instance.HideLoading();
        //    }
        //}
    }
}
