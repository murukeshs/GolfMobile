﻿using Golf.Models;
using Golf.Services;
using Golf.ViewModel.Round;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.CreateRoundView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateRoundPage : ContentPage
    {
        public DateTime? StartDate;
        public DateTime? EndDate;

        public CreateRoundPage()
        {
            InitializeComponent();
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var item = (sender as CheckBox).BindingContext as RoundRules;
            ((CreateRoundPageViewModel)BindingContext).CheckBoxSelectedCommand.Execute(item);
        }

        private void CustomPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            var Item = (CompetitionType)picker.SelectedItem;
            ((CreateRoundPageViewModel)BindingContext).CompetitionTypeSelectedCommand.Execute(Item);
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

        private void NullableDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            StartDate = RoundStartDatePicker.Date;
            if (StartDate < DateTime.Now.Date)
            {
                DisplayAlert("Start date", "Please select current or future date to proceed", "Ok");
            }
            else
            {
                var date = RoundStartDatePicker.Date.ToString("yyyy/MM/dd");
                ((CreateRoundPageViewModel)BindingContext).RoundStartCommand.Execute(date);
            }
        }

        private void NullableDatePicker_DateSelected_1(object sender, DateChangedEventArgs e)
        {
            StartDate = RoundStartDatePicker.Date;
            EndDate = RoundEndDatePicker.Date;
            if (EndDate < StartDate)
            {
                DisplayAlert("End date", "End date can't be less then start date.", "Ok");
            }
            else
            {
                var date = RoundEndDatePicker.Date.ToString("yyyy/MM/dd");
                ((CreateRoundPageViewModel)BindingContext).RoundStartEndCommand.Execute(date);
            }
        }
        
    }
}