using Golf.Models;
using Golf.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.UpdateTeamView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateTeam : ContentPage
    {
        public UpdateTeam()
        {
            InitializeComponent();
        }

        private void CustomPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var item = (int)picker.SelectedIndex;
            ((UpdateTeamViewModel)BindingContext).PickerSelectedCommand.Execute(item);
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var item = (sender as CheckBox).BindingContext as user;
            ((UpdateTeamViewModel)BindingContext).CheckBoxSelectedCommand.Execute(item);
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            var item = (sender as ImageButton).BindingContext as user;
            ((UpdateTeamViewModel)BindingContext).ToggleSelectedCommand.Execute(item);
        }

        private void RemoveParticipantClicked(object sender, EventArgs e)
        {
            var item = (sender as ImageButton).BindingContext as TeamPlayerDetails;
            ((UpdateTeamViewModel)BindingContext).RemoveParticipantCommand.Execute(item.teamPlayerListId);
        }
    }
}