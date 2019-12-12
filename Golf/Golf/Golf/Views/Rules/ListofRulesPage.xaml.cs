using Golf.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.Rules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListofRulesPage : ContentPage
    {
        public ListofRulesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((ListofRulesPageViewModel)BindingContext).SearchCommand.Execute(e.NewTextValue);
        }
    }
}