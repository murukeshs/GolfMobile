using Golf.Services;
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
            MessagingCenter.Subscribe<HomePage>((App)Application.Current, "OpenInvitePoppupPage", (sender) =>
            {
                PopupNavigation.Instance.PushAsync(new InviteParticipantPage());
            });
            //NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent ();
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<HomePage>(this, "OpenInvitePoppupPage");
        }

        //Device hardware button cliked handle
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await DisplayAlert("", "Are sure you want to Logout?.", "Yes", "No");
                if (!result)
                {
                    return;
                }
                else
                {
                    //when user click the "Yes" logout the application and also call the api to logout 
                    //the application from server
                    await ((NavigationPage)App.Current.MainPage).PopAsync();//this line navigate to previous page of your application
                }
            });
            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return true;
        }
    }
}