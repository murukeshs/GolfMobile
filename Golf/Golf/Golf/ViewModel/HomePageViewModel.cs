using Acr.UserDialogs;
using Golf.Utils;
using Golf.Views;
using Golf.Views.CreateMatchView;
using Golf.Views.JoinMatchView;
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

        }

        #region Join Match Command Functionality
        //Join Match Command
        public ICommand JoinMatchCommand => new AsyncCommand(JoinMatchAsync);
        async Task JoinMatchAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new JoinMatchPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch(Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion Join Match Command Functionality


        #region New Match Command Functionality
        public ICommand NewMatchCommand => new AsyncCommand(NewMatchAsync);

        async Task NewMatchAsync()
        {
            try
            {
                //UserDialogs.Instance.ShowLoading();
                var view = new CreateMatchPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                //UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion New Match Command Functionality

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
