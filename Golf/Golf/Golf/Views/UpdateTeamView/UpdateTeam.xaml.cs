using Acr.UserDialogs;
using Golf.Models;
using Golf.ViewModel;
using Golf.Views.RoundDetailsView;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.UpdateTeamView
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateTeam : ContentPage
    {
        public UpdateTeam()
        {
            InitializeComponent();
        }

        private void CustomPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var item = (int)picker.SelectedItem;
            ((UpdateTeamViewModel)BindingContext).PickerSelectedCommand.Execute(item);
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var item = (sender as CheckBox).BindingContext as AllParticipantsResponse;
            ((UpdateTeamViewModel)BindingContext).CheckBoxSelectedCommand.Execute(item);
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            var item = (sender as ImageButton).BindingContext as AllParticipantsResponse;
            ((UpdateTeamViewModel)BindingContext).ToggleSelectedCommand.Execute(item);
        }

        private void RemoveParticipantClicked(object sender, EventArgs e)
        {
            var item = (sender as ImageButton).BindingContext as AllParticipantsResponse;
            ((UpdateTeamViewModel)BindingContext).RemoveParticipantCommand.Execute(item);
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (e.Item == null)
                    return;
                //Deselect Item
                ((ListView)sender).SelectedItem = null;
                var item = (AllParticipantsResponse)e.Item;
                if (!item.isPublicProfile)
                {
                    var msg = item.nickName + " is a Private Profile!!!";
                    UserDialogs.Instance.Alert(msg, "Private Profile", "Ok");
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new RoundPlayerDetailsPopup(item));
                }

            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
    }
}