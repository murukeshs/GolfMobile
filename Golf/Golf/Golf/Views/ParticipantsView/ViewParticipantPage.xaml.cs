using Golf.Services;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewParticipantPage : ContentPage
	{
       
        public ViewParticipantPage ()
		{
			InitializeComponent ();
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void InviteParticipantButton_Clicked(object sender, System.EventArgs e)
        {
            var view = new InviteParticipantPage();
            await PopupNavigation.Instance.PushAsync(view);
        }
    }
}