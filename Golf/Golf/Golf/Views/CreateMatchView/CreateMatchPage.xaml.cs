using Golf.Models;
using Golf.Services;
using Golf.ViewModel.Match;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.CreateMatchView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateMatchPage : ContentPage
    {
        public CreateMatchPage()
        {
            InitializeComponent();
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var item = (sender as CheckBox).BindingContext as MatchRules;
            ((CreateMatchPageViewModel)BindingContext).CheckBoxSelectedCommand.Execute(item);
        }

        private void CustomPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var Item = (CompetitionType)picker.SelectedItem;
            ((CreateMatchPageViewModel)BindingContext).CompetitionTypeSelectedCommand.Execute(Item);
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