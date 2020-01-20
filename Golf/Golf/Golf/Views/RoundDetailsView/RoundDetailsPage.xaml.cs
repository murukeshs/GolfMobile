using Acr.UserDialogs;
using Golf.Controls;
using Golf.Models;
using Golf.Services;
using Golf.ViewModel.Round;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.RoundDetailsView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoundDetailsPage : ContentPage
    {

        public RoundDetailsPage()
        {
            try
            {
                InitializeComponent();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void TeamListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            ((RoundDetailsPageViewModel)BindingContext).TeamItemsTabbedCommand.Execute(e.Item as RoundDetailsListTeamList);

            //Deselect the selected Item in the listview
            ((ListView)sender).SelectedItem = null;
        }

        #region screen adjusting
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IAdjustScreenSize>().AdjustScreen();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IAdjustScreenSize>().UnAdjustScreen();
            }
        }
        #endregion

        private void CustomPicker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Picker picker = sender as Picker;
            var Item = (CompetitionType)picker.SelectedItem;
            ((RoundDetailsPageViewModel)BindingContext).CompetitionTypeSelectedCommand.Execute(Item);
        }

        private void EditTeam_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                var item = (sender as ImageButton).BindingContext as RoundDetailsListTeamList;
                ((RoundDetailsPageViewModel)BindingContext).EditTeamCommand.Execute(item);
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var item = (sender as CheckBox).BindingContext as RoundRules;
            ((RoundDetailsPageViewModel)BindingContext).CheckBoxSelectedCommand.Execute(item);
        }

        private void RemoveParticipantClicked(object sender, EventArgs e)
        {
            var item = (sender as ImageButton).BindingContext as AllParticipantsResponse;
            ((RoundDetailsPageViewModel)BindingContext).RemoveParticipantCommand.Execute(item);
        }

        private void UserCheckBox_CheckedChanged(object sender, bool e)
        {
            var item = (sender as CustomCheckBox).BindingContext as AllParticipantsResponse;
            ((RoundDetailsPageViewModel)BindingContext).UserCheckBoxSelectedCommand.Execute(item);
        }

        private async void RoundPlayers_ItemTapped(object sender, ItemTappedEventArgs e)
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