using Golf.Models;
using Golf.ViewModel.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.CreateMatchView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MatchTeamListPage : ContentPage
	{
		public MatchTeamListPage ()
		{
			InitializeComponent ();
		}

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var item = (sender as CheckBox).BindingContext as MatchTeamItems;

            ((MatchTeamListPageViewModel)BindingContext).CheckBoxSelectedCommand.Execute(item);
        }
    }
}