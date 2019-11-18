using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.JoinMatchView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JoinMatchPage : ContentPage
	{
		public JoinMatchPage()
		{
			InitializeComponent ();
		}

        private void BoxPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var Item = (matchJoinlist)picker.SelectedItem;
            if(Item.isAllowMatch)
            {
                ((JoinMatchPageViewModel)BindingContext).PickerSelectedCommand.Execute(Item);
            }
            else
            {
                UserDialogs.Instance.Alert("You are not a member of any team in the Match. So Can't Join. Plz check with Moderator", "Alert", "Ok");
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