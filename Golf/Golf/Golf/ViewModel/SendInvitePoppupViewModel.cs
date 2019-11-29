using Acr.UserDialogs;
using Golf.Utils;
using Golf.Views.CreateRoundView;
using Golf.Views.MenuView;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
namespace Golf.ViewModel
{
    public class SendInvitePoppupViewModel : BaseViewModel
    {
        #region Invite Okay Button Command Functionality
        //Close the Popup when user clicks the "Okay" Button
        public ICommand InviteOkayButtonCommand => new AsyncCommand(InviteOkayButtonAsync);

        async Task InviteOkayButtonAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var page = new SendInvitePoppup();
                await PopupNavigation.Instance.RemovePageAsync(page);
                var view = new MenuPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion Invite Okay Button Command Functionality
    }
}
