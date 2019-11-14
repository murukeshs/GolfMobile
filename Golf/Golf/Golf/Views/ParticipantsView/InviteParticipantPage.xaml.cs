using Golf.Controls;
using Golf.Services;
using Golf.ViewModel;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InviteParticipantPage : PopupPage
    {
		public InviteParticipantPage ()
		{
			InitializeComponent ();
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

        private void UserTypeChanged(object sender, Xamarin.Forms.Internals.EventArg<bool> e)
        {
            var value = (CustomCheckBox)sender;
            var item = value.DefaultValue;
            ((RegistrationPageViewModel)BindingContext).UserTypeCheckBoxCommand.Execute(item);
        }
    }
}