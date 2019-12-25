using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Views.RoundDetailsView;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Round
{
    public class RoundListPageViewModel : BaseViewModel
    {
        public RoundListPageViewModel()
        {
            //Load the match details list using this api.
            getRoundList();
        }

        #region Property Declaration

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

        private ObservableCollection<RoundList> OriginalRoundsList = new ObservableCollection<RoundList>();

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

        #endregion

        #region GetRoundList

        async void getRoundList()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();

                    var result = await App.ApiClient.GetRoundsList();

                    if(result != null)
                    {
                        RoundListItems = result;
                        OriginalRoundsList = RoundListItems;
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
                        UserDialogs.Instance.HideLoading();
                    }

                    //var RestURL = App.User.BaseUrl + "Round/getRoundList";
                    //var httpClient = new HttpClient();
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    //var response = await httpClient.GetAsync(RestURL);
                    //var content = await response.Content.ReadAsStringAsync();
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    RoundListItems = JsonConvert.DeserializeObject<ObservableCollection<RoundList>>(content);
                    //    OriginalRoundsList = RoundListItems;
                    //    if (RoundListItems.Count > 0)
                    //    {
                    //        ListViewIsVisible = true;
                    //        NoRecordsFoundLabel = false;
                    //    }
                    //    else
                    //    {
                    //        ListViewIsVisible = false;
                    //        NoRecordsFoundLabel = true;
                    //    }
                //}
                //    else
                //    {
                //        var error = JsonConvert.DeserializeObject<error>(content);
                //        UserDialogs.Instance.HideLoading();
                //        UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                //    }
                //    UserDialogs.Instance.HideLoading();
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

        #region List ItemTabbed Command Functionality

        public ICommand ListItemTabbedCommand => new Command(HandleItemSelected);

        private async void HandleItemSelected(object parameter)
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var Item = parameter as RoundList;
                var name = Item.roundName;
                App.User.CreateRoundId = Item.roundId;
                var view = new RoundDetailsPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                var a = ex.Message;
            }
        }

        #endregion List ItemTabbed Command Functionality

        #region Serach Command

        private ObservableCollection<RoundList> RoundsList = new ObservableCollection<RoundList>();

        public ICommand SearchCommand => new Command<string>(Search);

        public async void Search(string keyword)
        {
            try
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    RoundListItems = new ObservableCollection<RoundList>();

                    RoundsList = new ObservableCollection<RoundList>();

                    var query = OriginalRoundsList.Where(x => x.roundName.StartsWith(keyword));

                    foreach (var item in query)
                    {
                        var value = new RoundList() { roundCode = item.roundCode, roundName = item.roundName, roundStartDate = item.roundStartDate, StartDate = item.StartDate, StartTime = item.StartTime, roundFee = item.roundFee, roundId = item.roundId };

                        RoundsList.Add(value);
                    }

                    RoundListItems = RoundsList;
                }
                else
                {
                    RoundListItems = OriginalRoundsList;
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
