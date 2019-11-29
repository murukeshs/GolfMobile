using Acr.UserDialogs;
using Golf.Utils;
using Golf.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Round
{
    public class RoundCodePageViewModel : BaseViewModel
    {
        public string RoundCodeText { get; set; } = App.User.CompetitionType;
        public string RoundCode { get; set; } = App.User.RoundCode;
        //New round of the competition type &quot; Auto Win &quot; is successfully scheduled. Here is the Round participation code

        #region Round Code Proceed Button Command Functionality
        public ICommand RoundCodeProccedCommand => new AsyncCommand(ProceedAsync);
        async Task ProceedAsync()
        {
            try
            {
                await PopupNavigation.Instance.PopAsync();
                UserDialogs.Instance.ShowLoading();
                var view = new CreateTeamPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion Round Code Proceed Button Command Functionality
    }
}
