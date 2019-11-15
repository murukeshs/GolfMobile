using Acr.UserDialogs;
using Golf.Services;
using Golf.Utils;
using Golf.Views.CreateMatchView;
using Golf.Views.MenuView;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Golf.Models;
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
