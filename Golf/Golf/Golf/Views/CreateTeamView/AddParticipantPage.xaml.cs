﻿using Acr.UserDialogs;
using Golf.Controls;
using Golf.Models;
using Golf.Services;
using Golf.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;
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

        private void ImageButton_Clicked(object sender, System.EventArgs e)
        {
            var item = (sender as ImageButton).BindingContext as AllParticipantsResponse;
            ((AddParticipantPageViewModel)BindingContext).ToggleSelectedCommand.Execute(item);
        }

        private void CheckBox_CheckedChanged(object sender, bool e)
        {
            var item = (sender as CustomCheckBox).BindingContext as AllParticipantsResponse;
            if (App.User.ScoreKeeperId == item.userId)
            {
                UserDialogs.Instance.Alert("score keeper can't be a Player .", "Alert", "Ok");
                item.IsChecked = false;
            }
        //((AddParticipantPageViewModel)BindingContext).CheckBoxSelectedCommand.Execute(item);
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((AddParticipantPageViewModel)BindingContext).SearchCommand.Execute(e.NewTextValue);
        }
    }
}