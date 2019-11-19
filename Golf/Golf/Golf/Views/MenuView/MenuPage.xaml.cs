using Golf.Services;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.MenuView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : MasterDetailPage
    {
       

        public MenuPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = e.SelectedItem as MenuPageMenuItem;
                if (item == null)
                    return;
                
                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;
                if (item.Title == "Invite a Participant")
                {
                    DependencyService.Get<IShowPoppup>().ShowPoppupPage(); 
                    IsPresented = false;
                }
                else if (item.Title == "Logout")
                {
                    IsPresented = false;
                    var result = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
                    if (result)
                    {
                        var LoginView = new LoginPage();
                        var navigationPage = ((NavigationPage)App.Current.MainPage);
                        await navigationPage.PushAsync(LoginView);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    Detail = new NavigationPage(page);
                    IsPresented = false;
                }

                MasterPage.ListView.SelectedItem = null;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }

        public async Task PushRootView(Page newRootView)
        {
            Detail = new NavigationPage(newRootView);
        }

        public async Task ReplaceCurrentPage(Page currentPage, Page newPage)
        {
            Navigation.InsertPageBefore(currentPage, newPage);
            await PopPage();
        }

        public async Task PushAsync(Page newPage)
        {
            await ((NavigationPage)Detail).PushAsync(newPage);
        }

        public async Task PopPage()
        {
            await ((NavigationPage)Detail).PopAsync();
        }
    }
}