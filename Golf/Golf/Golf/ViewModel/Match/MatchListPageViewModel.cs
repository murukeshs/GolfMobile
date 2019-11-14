using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Views.MatchDetailsView;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Match
{
    public class MatchListPageViewModel : BaseViewModel
    {
        public ObservableCollection<MatchList> MatchListItems
        {
            get { return _MatchListItems; }
            set
            {
                _MatchListItems = value;
                OnPropertyChanged(nameof(MatchListItems));
            }
        }
        private ObservableCollection<MatchList> _MatchListItems = null;

        public MatchListPageViewModel()
        {
            //Load the match details list using this api.
            getMatchList();
        }

        async void getMatchList()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "Match/getMatchList";
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    MatchListItems = JsonConvert.DeserializeObject<ObservableCollection<MatchList>>(content);
                    if (response.IsSuccessStatusCode)
                    {
                        //UserDialogs.Instance.Alert("Invite send to all the participants successfully.", "Success", "ok");
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

        #region List ItemTabbed Command Functionality
        public ICommand ListItemTabbedCommand => new Command(HandleItemSelected);

        private async void HandleItemSelected(object parameter)
        {
            //ViewModelNavigation.PushAsync(new ItemPageViewModel() { Item = parameter as RssItem });
            UserDialogs.Instance.ShowLoading();
            var Item = parameter as MatchList;
            var name = Item.matchName;
            App.User.MatchId = Item.matchId;
            var view = new MatchDetailsPage();
            var navigationPage = ((NavigationPage)App.Current.MainPage);
            await navigationPage.PushAsync(view);
            UserDialogs.Instance.HideLoading();
        }
        #endregion List ItemTabbed Command Functionality
    }
}
