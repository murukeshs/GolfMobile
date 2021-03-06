﻿using Acr.UserDialogs;
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

        public ICommand InviteOkayButtonCommand => new AsyncCommand(InviteOkayButtonAsync);

        async Task InviteOkayButtonAsync()
        {
            UserDialogs.Instance.ShowLoading();
            try
            {
                App.User.CreateRoundId = 0;
                App.User.CreateTeamId = 0;
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
