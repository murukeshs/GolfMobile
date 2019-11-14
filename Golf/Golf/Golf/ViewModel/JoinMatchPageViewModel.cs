using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views.MatchDetailsView;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class JoinMatchPageViewModel : BaseViewModel
    {
        public string MatchID
        {
            get { return _MatchID; }
            set
            {
                _MatchID = value;
                OnPropertyChanged(nameof(MatchID));
            }
        }
        private string _MatchID = string.Empty;

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

        public string ParticipantName
        {
            get { return _ParticipantName;}
            set
            {
                _ParticipantName = value;
                OnPropertyChanged(nameof(ParticipantName));
            }
        }
        private string _ParticipantName = string.Empty;

        public string ParticipantID
        {
            get { return _ParticipantID; }
            set
            {
                _ParticipantID = value;
                OnPropertyChanged(nameof(ParticipantID));
            }
        }
        private string _ParticipantID = string.Empty;

        public string MatchName
        {
            get { return _MatchName; }
            set
            {
                _MatchName = value;
                OnPropertyChanged(nameof(MatchName));
            }
        }
        private string _MatchName = string.Empty;


        public JoinMatchPageViewModel()
        {
            //JoinMatchListAPI();
        }

        #region JoinMatch Button Command Functionality
        public ICommand JoinMatchButtonCommand => new AsyncCommand(JoinMatchAsync);
        async Task JoinMatchAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new MatchListPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        async void JoinMatchListAPI()
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
                    if (response.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.Alert("Invite send to all the participants successfully.", "Success", "ok");
                        //var view = new MenuPage();
                        //var navigationPage = ((NavigationPage)App.Current.MainPage);
                        //await navigationPage.PushAsync(view);
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
        #endregion JoinMatch Button Command Functionality
    }
}
