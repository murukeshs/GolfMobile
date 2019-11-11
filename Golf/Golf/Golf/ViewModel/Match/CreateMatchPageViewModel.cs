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
    public class CreateMatchPageViewModel : BaseViewModel
    {
        #region Proceed Button Command Functionality
        public ICommand ProceedCommand => new AsyncCommand(ProceedAsync);
        async Task ProceedAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new MatchContextSettingsPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion Proceed Button Command Functionality
    }
}
