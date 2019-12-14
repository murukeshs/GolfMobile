using Acr.UserDialogs;
using Golf.Utils;
using Golf.Views;
using Golf.Views.CreateRoundView;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Round
{
    public class RoundCodePageViewModel : BaseViewModel
    {
        #region Property Declaration

        public string RoundCodeText { get; set; } = App.User.CompetitionType;

        public string RoundCode { get; set; } = App.User.RoundCode;

        #endregion

        #region Round Code Proceed Button Command Functionality

        public ICommand RoundCodeProccedCommand => new AsyncCommand(ProceedAsync);

        async Task ProceedAsync()
        {
            try
            {
                await PopupNavigation.Instance.PopAsync();
                UserDialogs.Instance.ShowLoading();
                var view = new ParticipantSelection();
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
