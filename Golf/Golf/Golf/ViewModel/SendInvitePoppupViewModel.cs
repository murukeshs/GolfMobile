using Acr.UserDialogs;
using Golf.Utils;
using Golf.Views.CreateMatchView;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        #endregion Invite Okay Button Command Functionality
    }
}
