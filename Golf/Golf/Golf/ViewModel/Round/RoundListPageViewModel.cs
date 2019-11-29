using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Views.RoundDetailsView;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Round
{
    public class RoundListPageViewModel : BaseViewModel
    {
        public ObservableCollection<RoundList> RoundListItems
        {
            get { return _RoundListItems; }
            set
            {
                _RoundListItems = value;
                OnPropertyChanged(nameof(RoundListItems));
            }
        }
        private ObservableCollection<RoundList> _RoundListItems = null;

        public bool NoRecordsFoundLabel
        {
            get { return _NoRecordsFoundLabel; }
            set
            {
                _NoRecordsFoundLabel = value;
                OnPropertyChanged("NoRecordsFoundLabel");
            }
        }
        private bool _NoRecordsFoundLabel = false;

        public bool ListViewIsVisible
        {
            get { return _ListViewIsVisible; }
            set
            {
                _ListViewIsVisible = value;
                OnPropertyChanged("ListViewIsVisible");
            }
        }
        private bool _ListViewIsVisible = false;

        public RoundListPageViewModel()
        {
            //Load the match details list using this api.
            getRoundList();
        }

        async void getRoundList()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "Round/getRoundList";
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        RoundListItems = JsonConvert.DeserializeObject<ObservableCollection<RoundList>>(content);
                        if (RoundListItems.Count > 0)
                        {
                            ListViewIsVisible = true;
                            NoRecordsFoundLabel = false;
                        }
                        else
                        {
                            ListViewIsVisible = false;
                            NoRecordsFoundLabel = true;
                        }
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
            var Item = parameter as RoundList;
            var name = Item.roundName;
            App.User.RoundId = Item.roundId;
            var view = new RoundDetailsPage();
            var navigationPage = ((NavigationPage)App.Current.MainPage);
            await navigationPage.PushAsync(view);
            UserDialogs.Instance.HideLoading();
        }
        #endregion List ItemTabbed Command Functionality
    }
}
