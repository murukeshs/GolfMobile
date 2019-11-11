using Acr.UserDialogs;
using Golf.Utils;
using Golf.Views.MatchDetailsView;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel
{
    public class JoinMatchPageViewModel : BaseViewModel
    {
        #region JoinMatch Button Command Functionality
        public ICommand JoinMatchButtonCommand => new AsyncCommand(JoinMatchAsync);
        async Task JoinMatchAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new MatchListPage();
                var navigationPage = ((NavigationPage)App.Current.MainPage);
                await navigationPage.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion JoinMatch Button Command Functionality
    }
}
