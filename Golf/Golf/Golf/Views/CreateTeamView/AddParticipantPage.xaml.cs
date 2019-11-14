using Golf.Models;
using Golf.ViewModel;
using Golf.Views.PoppupView;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddParticipantPage : ContentPage
	{
        public AddParticipantPage ()
		{
			InitializeComponent ();
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var item = (sender as CheckBox).BindingContext as user;
            ((AddParticipantPageViewModel)BindingContext).CheckBoxSelectedCommand.Execute(item);
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var item = (sender as Switch).BindingContext as user;
            ((AddParticipantPageViewModel)BindingContext).ToggleSelectedCommand.Execute(item);
        }
    }
}