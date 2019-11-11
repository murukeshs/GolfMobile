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
        #region Match Code Proceed Button Command Functionality
        public ICommand MatchCodeProccedCommand => new AsyncCommand(ProceedAsync);
        async Task ProceedAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                //var view = new ListofPlayersPage();
                //var navigationPage = ((NavigationPage)App.Current.MainPage);
                //await navigationPage.PushAsync(view);
                await PopupNavigation.Instance.PushAsync(new SendInvitePoppup());
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
