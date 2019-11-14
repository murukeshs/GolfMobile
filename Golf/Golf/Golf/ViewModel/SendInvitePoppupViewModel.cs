using Acr.UserDialogs;
using Golf.Services;
using Golf.Utils;
using Golf.Views.CreateMatchView;
using Golf.Views.MenuView;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
                SendInviteAPI();
                var page = new SendInvitePoppup();
                await PopupNavigation.Instance.RemovePageAsync(page);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }


        async void SendInviteAPI()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    var RestURL = App.User.BaseUrl + "Match/inviteMatch/" + App.User.CreateMatchId;
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        UserDialogs.Instance.Alert("Invite send to all the participants successfully.", "Success", "ok");
                        var view = new MenuPage();
                        var navigationPage = ((NavigationPage)App.Current.MainPage);
                        await navigationPage.PushAsync(view);
                        UserDialogs.Instance.HideLoading();
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("Something went wrong, please try again later", "ok");
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

        #endregion Invite Okay Button Command Functionality
    }
}
