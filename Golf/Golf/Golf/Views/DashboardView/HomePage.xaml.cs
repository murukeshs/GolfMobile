using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
            //NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<App>((App)Application.Current, "OpenInvitePoppupPage", (sender) =>
            {
                PopupNavigation.Instance.PushAsync(new InviteParticipantPage());
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<HomePage>(this, "OpenInvitePoppupPage");
        }
    }
}