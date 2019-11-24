using Golf.Models;
using Golf.ViewModel;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewParticipantPage : ContentPage
    {
        public ViewParticipantPage()
        {
            InitializeComponent();
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

        private void UpdateParticipantButton_Clicked(object sender, System.EventArgs e)
        {
            UpdateParticipantButton.BackgroundColor = (Color)App.Current.Resources["LightGreenColor"];
            UpdateParticipantButton.TextColor = (Color)App.Current.Resources["WhiteColor"];

            ViewParticipantButton.BackgroundColor = (Color)App.Current.Resources["WhiteColor"];
            ViewParticipantButton.TextColor = (Color)App.Current.Resources["LightGreenColor"];

            ViewParticipantsStackLayout.IsVisible = false;
            UpdateParticpantsStackLayout.IsVisible = true;
        }

        private void ViewParticipantButton_Clicked(object sender, System.EventArgs e)
        {
            ViewParticipantButton.BackgroundColor = (Color)App.Current.Resources["LightGreenColor"];
            ViewParticipantButton.TextColor = (Color)App.Current.Resources["WhiteColor"];

            UpdateParticipantButton.BackgroundColor = (Color)App.Current.Resources["WhiteColor"];
            UpdateParticipantButton.TextColor = (Color)App.Current.Resources["LightGreenColor"];

            ViewParticipantsStackLayout.IsVisible = true;
            UpdateParticpantsStackLayout.IsVisible = false;
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var item = (sender as CheckBox).BindingContext as user;
            ((ViewParticipantsViewModel)BindingContext).CheckBoxSelectedCommand.Execute(item);
        }

        private void ImageButton_Clicked(object sender, System.EventArgs e)
        {
            var item = (sender as ImageButton).BindingContext as user;
            ((ViewParticipantsViewModel)BindingContext).ToggleSelectedCommand.Execute(item);
        }
    }
}