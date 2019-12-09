using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views.MenuView;
using Golf.Views.RoundDetailsView;
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
    public class JoinRoundPageViewModel : BaseViewModel
    {
        public JoinRoundPageViewModel()
        {
            JoinRoundListAPI();
        }

        public string RoundID
        {
            get { return _RoundID; }
            set
            {
                _RoundID = value;
                OnPropertyChanged(nameof(RoundID));
            }
        }
        private string _RoundID = string.Empty;

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
        private string _ParticipantName = App.User.UserName;

        public string ParticipantID
        {
            get { return _ParticipantID; }
            set
            {
                _ParticipantID = value;
                OnPropertyChanged(nameof(ParticipantID));
            }
        }
        private string _ParticipantID = App.User.UserId.ToString();



        public string RoundFee
        {
            get { return _RoundFee; }
            set
            {
                _RoundFee = value;
                OnPropertyChanged(nameof(RoundFee));
            }
        }
        private string _RoundFee = "0.00";

        public string RoundName
        {
            get { return _RoundName; }
            set
            {
                _RoundName = value;
                OnPropertyChanged(nameof(RoundName));
            }
        }
        private string _RoundName = string.Empty;



        #region JoinRound Button Command Functionality
        public ICommand JoinRoundButtonCommand => new AsyncCommand(JoinRoundAsync);
        async Task JoinRoundAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new RoundListPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        public ObservableCollection<roundJoinlist> RoundList
        {
            get { return _RoundList; }
            set
            {
                _RoundList = value;
                OnPropertyChanged(nameof(RoundList));
            }
        }
        public ObservableCollection<roundJoinlist> _RoundList = null;

        async void JoinRoundListAPI()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "Round/getRoundJoinList?roundId=0&userId=" + App.User.UserId;
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        if(content != "Please Enter RoundId")
                        { 
                          RoundList = JsonConvert.DeserializeObject<ObservableCollection<roundJoinlist>>(content);
                        }
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
        #endregion JoinRound Button Command Functionality

        #region Round Picker Selected Command Functionality

        public ICommand PickerSelectedCommand => new Command(SelectedIndexChangedEvent);

        void SelectedIndexChangedEvent(object parameter)
        {
            var Item = parameter as roundJoinlist;
            RoundID = Item.roundId.ToString();
            CompetitionType = Item.CompetitionName;
            ParticipantName = Item.ParticipantName;
            ParticipantID = Item.ParticipantId.ToString();
            RoundFee = Item.roundFee;
        }
        #endregion Round Picker Selected Command Functionality

        #region JoinRound Button Command Functionality

        public ICommand JoinRoundCommand => new AsyncCommand(JoinRoundButtonAsync);

        async Task JoinRoundButtonAsync()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "Round/acceptRoundInvitation";
                    Uri requestUri = new Uri(RestURL);

                    var data = new acceptRoundInvitation
                    {
                        roundId = Convert.ToInt32(RoundID),
                        type = "Round",
                        playerId = App.User.UserId
                    };

                    string json = JsonConvert.SerializeObject(data);
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.PutAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    string responJsonText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        await UserDialogs.Instance.AlertAsync("Round Joined Successfully.", "Join Round", "Ok");
                        var view = new MenuPage();
                        var navigationPage = ((NavigationPage)App.Current.MainPage);
                        await navigationPage.PushAsync(view);
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
        #endregion JoinRound Button Command Functionality
    }
}
