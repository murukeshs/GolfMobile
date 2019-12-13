using Acr.UserDialogs;
using Golf.Utils;
using Golf.Views;
using Golf.Views.CreateRoundView;
using Golf.Views.JoinRoundView;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class HomePageViewModel : BaseViewModel
    {
        public HomePageViewModel()
        {
            IsModerator = App.User.IsModerator;
        }

        #region Property Declaration

        public bool IsModerator
        {
            get
            {
                return _IsModerator;
            }
            set
            {
                _IsModerator = value;
                OnPropertyChanged("IsModerator");
            }
        }
        public bool _IsModerator;

        #endregion      

        #region Join Round Command Functionality
        //Join Round Command
        public ICommand JoinRoundCommand => new AsyncCommand(JoinRoundAsync);
        async Task JoinRoundAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new JoinRoundPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch(Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion Join Round Command Functionality

        #region New Round Command Functionality
        public ICommand NewRoundCommand => new AsyncCommand(NewRoundAsync);

        async Task NewRoundAsync()
        {
            try
            {
                //UserDialogs.Instance.ShowLoading();
                var view = new CreateRoundPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                //UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion New Round Command Functionality

        #region Teams Command Functionality
        public ICommand TeamsCommand => new AsyncCommand(TeamsAsync);

        async Task TeamsAsync()
        {
            try
            {
                //UserDialogs.Instance.ShowLoading();
                var view = new ListOfTeamsPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                //UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
            
        }
        #endregion Teams Command Functionality

        #region Participants Command Functionality
        public ICommand ParticipantsCommand => new AsyncCommand(ParticipantsAsync);

        async Task ParticipantsAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new ViewAllParticipantsPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion Participants Command Functionality
    }
}
