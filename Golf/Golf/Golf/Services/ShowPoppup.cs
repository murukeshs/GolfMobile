using Golf.Services;
using Golf.Views;
using Rg.Plugins.Popup.Services;

[assembly: Xamarin.Forms.Dependency(typeof(ShowPoppup))]
namespace Golf.Services
{
    public class ShowPoppup : IShowPoppup
    {
        public void ShowPoppupPage()
        {
            PopupNavigation.Instance.PushAsync(new InviteParticipantPage(null));
        }
    }
}
