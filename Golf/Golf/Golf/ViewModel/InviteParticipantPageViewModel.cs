using Acr.UserDialogs;
using Golf.Utils;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Golf.ViewModel
{
    public class InviteParticipantPageViewModel : BaseViewModel
    {
        #region Send Invite Button Command Functionality
        public ICommand AddParticipantsCommand => new AsyncCommand(SendInviteAsync);

        async Task SendInviteAsync()
        {
            UserDialogs.Instance.ShowLoading();
            await SendInviteAPI();
            //await PopupNavigation.Instance.PushAsync(view);
            UserDialogs.Instance.HideLoading();
        }

        async Task SendInviteAPI()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    string RestURL = App.User.BaseUrl + "Team/createTeamPlayers";
                    Uri requestUri = new Uri(RestURL);

                    //var data = new TeamPlayer
                    //{
                    //    teamId = App.User.CreateTeamId,
                    //    scoreKeeperID = ScoreKeeperId,
                    //    playerId = PlayerId,
                    //};

                    //string json = JsonConvert.SerializeObject(data);
                    //var httpClient = new HttpClient();
                    //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    //var response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                    //string responJsonText = await response.Content.ReadAsStringAsync();

                    //if (response.IsSuccessStatusCode)
                    //{
                    //    var Item = JsonConvert.DeserializeObject<CreateTeamResponse>(responJsonText);
                    //    //App.User.CreateTeamId = Item.teamId;
                    //    var view = new MenuPage();
                    //    var navigationPage = ((NavigationPage)App.Current.MainPage);
                    //    await navigationPage.PushAsync(view);
                    //    ////After the success full api process clear all the values
                    //    //Clear();
                    //    UserDialogs.Instance.HideLoading();
                    //}
                    //else
                    //{
                    //    var Item = JsonConvert.DeserializeObject<CreateTeamResponse>(responJsonText);
                    //    UserDialogs.Instance.HideLoading();
                    //    DependencyService.Get<IToast>().Show(Item.ErrorMessage);
                    //}
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
        #endregion Send Invite Button Command Functionality
    }
}
