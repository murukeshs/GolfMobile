using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.JoinMatchView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JoinMatchPage : ContentPage
	{
		public JoinMatchPage ()
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
    }

        private void BoxPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var Item = (matchJoinlist)picker.SelectedItem;
            ((JoinMatchPageViewModel)BindingContext).PickerSelectedCommand.Execute(Item);
        }
    }
}