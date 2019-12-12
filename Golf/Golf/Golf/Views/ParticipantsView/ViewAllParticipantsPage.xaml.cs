using Acr.UserDialogs;
using Golf.Models;
using Golf.ViewModel;
using Golf.Views.ParticipantsView;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewAllParticipantsPage : ContentPage
	{
		public ViewAllParticipantsPage ()
		{
			InitializeComponent ();
		}

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this page when a back button is pressed
            return true;
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                
                    if (e.Item == null)
                        return;

                var item = (AllParticipantsResponse)e.Item;
                if (!item.isPublicProfile)
                {
                    UserDialogs.Instance.Alert("It is a Private Profile!!!", "Alert", "Ok");
                }
                else
                {
                    var id = item.userId;
                    var view = new ViewParticipantByIdPage(id);
                    var navigationPage = ((NavigationPage)App.Current.MainPage);
                    await navigationPage.PushAsync(view);     
                }
                    //Deselect Item
                    ((ListView)sender).SelectedItem = null;
            }
            catch(Exception ex)
            {
                var a = ex.Message;
            }
        }

        private async void InviteParticipantButton_Clicked(object sender, System.EventArgs e)
        {
            var view = new InviteParticipantPage();
            await PopupNavigation.Instance.PushAsync(view);
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
                ((ViewAllParticipantsViewModel)BindingContext).SearchCommand.Execute(e.NewTextValue);
        }
    }
}