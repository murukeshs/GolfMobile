using Acr.UserDialogs;
using Golf.Utils;
using Golf.Views.CreateMatchView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Match
{
    public class MatchContextSettingsPageViewModel : BaseViewModel
    {
        #region Submit Button Command Functionality
        public ICommand SubmitCommand => new AsyncCommand(SubmitAsync);
        async Task SubmitAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new MatchCodePoppup();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion Submit Button Command Functionality
    }
}
