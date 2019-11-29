using Golf.Controls;
using Golf.ViewModel;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.RegistrationView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPassword : ContentPage
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }
        private void Checkbox_CheckedChanged(object sender, bool e)
        {
            CustomCheckBox check = sender as CustomCheckBox;
            ((ForgotPasswordViewModel)BindingContext).OtpViaCheckedChangedCommand.Execute(check);
        }
    }
}