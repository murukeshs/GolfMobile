using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views.MatchDetailsView;
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

        public ObservableCollection<matchJoinlist> MatchList
        {
            get { return _MatchList; }
            set
            {
                _MatchList = value;
                OnPropertyChanged(nameof(MatchList));
            }
        }
        public ObservableCollection<matchJoinlist> _MatchList = null;

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
                        MatchList = JsonConvert.DeserializeObject<ObservableCollection<matchJoinlist>>(content);
                        //UserDialogs.Instance.Alert("Invite send to all the participants successfully.", "Success", "ok");
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

        #region Match Picker Selected Command Functionality

        public ICommand PickerSelectedCommand => new Command(SelectedIndexChangedEvent);

        void SelectedIndexChangedEvent(object parameter)
        {
            var Item = parameter as matchJoinlist;
            MatchID = Item.matchId.ToString();
            CompetitionType = Item.CompetitionName;
            ParticipantName = Item.ParticipantName;
            ParticipantID = Item.ParticipantId.ToString();
        }
        #endregion Match Picker Selected Command Functionality

        #region JoinMatch Button Command Functionality

        public ICommand JoinMatchCommand => new AsyncCommand(JoinMatchButtonAsync);

        async Task JoinMatchButtonAsync()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "Match/acceptMatchInvitation";
                    Uri requestUri = new Uri(RestURL);

                    var data = new acceptMatchInvitation
                    {
                       matchId = Convert.ToInt32(MatchID),
                       type = "Match",
                       playerId = App.User.UserId
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        //var Item = JsonConvert.DeserializeObject<CreateTeamResponse>(responJsonText);
                        //App.User.CreateTeamId = Item.teamId;
                        //var view = new MenuPage();
                        //var navigationPage = ((NavigationPage)App.Current.MainPage);
                        //await navigationPage.PushAsync(view);
                        ////After the success full api process clear all the values
                        //Clear();
                        UserDialogs.Instance.Alert("Match Joined Successfully.","Join Match","Ok");
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
        #endregion JoinMatch Button Command Functionality
    }
}
