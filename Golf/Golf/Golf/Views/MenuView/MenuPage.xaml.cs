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

                Page page;

                if (item.TargetType == typeof(InviteParticipantPage))
                {
                     page = (Page)Activator.CreateInstance(item.TargetType, "sidebar");
                }
                else
                {
                     page = (Page)Activator.CreateInstance(item.TargetType);
                }
               
                page.Title = item.Title;
                
                if (item.Title == "Home")
                {
                    IsPresented = false;
                    //Detail =  new NavigationPage(page);
                }
                else if (item.Title == "Invite a Participant")
                {
                    var view = new InviteParticipantPage("viewallparticipantspage");
                    await PopupNavigation.Instance.PushAsync(view);
                    // DependencyService.Get<IShowPoppup>().ShowPoppupPage();
                    IsPresented = false;
                }
                else if (item.Title == "Logout")
                {
                    IsPresented = false;
                    MasterPage.ListView.SelectedItem = null;
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
                else if(item.TargetType == null)
                {
                    IsPresented = false;
                    MasterPage.ListView.SelectedItem = null;
                }
                else
                {
                    IsPresented = false;
                    //Detail =  new NavigationPage(page);
                    var navigationPage = ((NavigationPage)App.Current.MainPage);
                    await navigationPage.PushAsync(page);
                }

                MasterPage.ListView.SelectedItem = null;
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
    }
}