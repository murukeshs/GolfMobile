using Acr.UserDialogs;
using Golf.Utils;
using Golf.Views.CreateMatchView;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Match
{
    public class MatchCodePageViewModel : BaseViewModel
    {
        public string MatchCodeText { get; set; } = "New match of the competition type " + App.User.CompetitionType + " is successfully scheduled.Here is the Match participation code";
        public string MatchCode { get; set; } = App.User.MatchCode;
        //New match of the competition type &quot; Auto Win &quot; is successfully scheduled. Here is the Match participation code

        #region Match Code Proceed Button Command Functionality
        public ICommand MatchCodeProccedCommand => new AsyncCommand(ProceedAsync);
        async Task ProceedAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new MatchTeamListPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                //await PopupNavigation.Instance.PushAsync(new SendInvitePoppup());
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion Match Code Proceed Button Command Functionality
    }
}
