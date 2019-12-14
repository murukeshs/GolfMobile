using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Views;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class AddNewTeamViewModel : BaseViewModel
    {
        public AddNewTeamViewModel()
        {
            LoadTeamListAsync();
        }

        #region Property Declaration

        public ObservableCollection<RoundTeamItems> TeamItems
        {
            get { return _TeamItem; }
            set
            {
                _TeamItem = value;
                OnPropertyChanged(nameof(TeamItems));
            }
        }
        private ObservableCollection<RoundTeamItems> _TeamItem = null;

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

        #region Register Command Functionality

        public ICommand TeamListItemsTabbedCommand => new Command(TeamListItemsTabbed);

        async void TeamListItemsTabbed(object parameter)
        {
            try
            {
                var item = parameter as RoundTeamItems;
                App.User.TeamIdforPlayerListing = item.teamId;
                App.User.TeamName = item.teamName;
                var view = new ViewParticipantPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        #endregion Register Command Functionality

        #region LoadTeamList

        async void LoadTeamListAsync()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "Team/listTeam";
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        TeamItems = JsonConvert.DeserializeObject<ObservableCollection<RoundTeamItems>>(content);
                        if (TeamItems.Count > 0)
                        {
                            ListViewIsVisible = true;
                            NoRecordsFoundLabel = false;
                        }
                        else
                        {
                            ListViewIsVisible = false;
                            NoRecordsFoundLabel = true;
                        }
                        await Task.Delay(10);
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

        #endregion
    }
}
