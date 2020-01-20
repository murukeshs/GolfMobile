using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.Utils;
using Golf.Views.MenuView;
using Golf.Views.RoundDetailsView;
using Plugin.Connectivity;
using System;
using System.Collections.ObjectModel;
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

        #region Property Declaration
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
            get { return _ParticipantName; }
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

        public string TeamName
        {
            get { return _TeamName; }
            set
            {
                _TeamName = value;
                OnPropertyChanged(nameof(TeamName));
            }
        }
        private string _TeamName = string.Empty;
        #endregion

        #region GetJoinRoundList Command Functionality
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
                UserDialogs.Instance.HideLoading();
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

                    var result = await App.ApiClient.GetJoinRoundsList(0, App.User.UserId);
                    if (result != null)
                    {
                        RoundList = result;
                        UserDialogs.Instance.HideLoading();
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

        #region Round Picker Selected Command Functionality
        public ICommand PickerSelectedCommand => new Command(SelectedIndexChangedEvent);

        void SelectedIndexChangedEvent(object parameter)
        {
            try
            {
                var Item = parameter as roundJoinlist;
                RoundID = Item.roundId.ToString();
                CompetitionType = Item.CompetitionName;
                ParticipantName = Item.ParticipantName;
                ParticipantID = Item.ParticipantId.ToString();
                TeamName = Item.teamName;
                RoundFee = Item.roundFee;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
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
                    if (string.IsNullOrEmpty(RoundID))
                    {
                        UserDialogs.Instance.Alert("Please Select Round!", "Alert", "Ok");
                    }
                    else
                    {
                        UserDialogs.Instance.ShowLoading();
                        var data = new acceptRoundInvitation
                        {
                            roundId = Convert.ToInt32(RoundID),
                            type = "Round",
                            playerId = App.User.UserId
                        };

                        var result = await App.ApiClient.JoinRound(data);

                        if (result != null)
                        {
                            await UserDialogs.Instance.AlertAsync("Player Joined the Round Successfully.", "Success", "Ok");
                            var view = new MenuPage();
                            var navigationPage = ((NavigationPage)App.Current.MainPage);
                            await navigationPage.PushAsync(view);
                        }
                        UserDialogs.Instance.HideLoading();

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
