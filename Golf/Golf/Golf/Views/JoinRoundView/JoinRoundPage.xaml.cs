using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.JoinRoundView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JoinRoundPage : ContentPage
	{
		public JoinRoundPage()
		{
			InitializeComponent ();
		}

        private async void BoxPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var Item = (roundJoinlist)picker.SelectedItem;
            if(Item.isAllowRound)
            {
                ((JoinRoundPageViewModel)BindingContext).PickerSelectedCommand.Execute(Item);
            }
            else
            {
              await  UserDialogs.Instance.AlertAsync("You are not a member of any team in the Round. So Can't Join. Plz check with Moderator", "Alert", "Ok");
            }
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
    }
}