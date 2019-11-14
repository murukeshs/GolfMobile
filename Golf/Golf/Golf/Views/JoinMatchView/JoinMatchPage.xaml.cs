using Golf.Models;
using Golf.ViewModel;
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

        private void BoxPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var Item = (matchJoinlist)picker.SelectedItem;
            ((JoinMatchPageViewModel)BindingContext).PickerSelectedCommand.Execute(Item);
        }
    }
}