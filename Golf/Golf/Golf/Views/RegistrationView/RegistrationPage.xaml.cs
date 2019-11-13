﻿using Acr.UserDialogs;
using Golf.ViewModel;
using Golf.Views.MenuView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegistrationPage : ContentPage
	{
        RegistrationPageViewModel vm;
        public RegistrationPage ()
		{
			InitializeComponent ();
            vm = BindingContext as RegistrationPageViewModel;
        }



        private async void RegisterAdminButton_Clicked(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();
            var view = new RegistrationPageAdmin();
            var navigationPage = ((NavigationPage)App.Current.MainPage);
            await navigationPage.PushAsync(view);
            UserDialogs.Instance.HideLoading();
        }

        private void GenderOnchange(object sender, EventArgs e)
        {
            Picker picker = sender as Picker;
            vm.GenderText = picker.SelectedItem.ToString();
        }

        private void DobPickerSelected(object sender, DateChangedEventArgs e)
        {
            vm.dob = DobPicker.Date;

        }

        private void UserTypeChanged(object sender, Xamarin.Forms.Internals.EventArg<bool> e)
        {
            var value = PlayerCheckbox.DefaultValue; 
            //viewmodel.UserTypeID = 
        }
    }
}