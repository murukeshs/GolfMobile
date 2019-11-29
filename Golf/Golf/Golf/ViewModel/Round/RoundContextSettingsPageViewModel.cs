using Acr.UserDialogs;
using Golf.Utils;
using Golf.Views.CreateRoundView;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Golf.ViewModel.Round
{
    public class RoundContextSettingsPageViewModel : BaseViewModel
    {
        public List<string> SettingsList { get; set; }

        public List<string> RoundExtrasList { get; set; }

        public RoundContextSettingsPageViewModel()
        {
            SettingsList = new List<string>();
            SettingsList.Add("Carryover - if no one wins the hole");
            SettingsList.Add("N tie");
            SettingsList.Add("Greater than 1 or equal to");
            SettingsList.Add("No Carryover");

            RoundExtrasList = new List<string>();
            RoundExtrasList.Add("Greenies");
            RoundExtrasList.Add("Skins");
            RoundExtrasList.Add("Closet to the pin");
            RoundExtrasList.Add("Longest drive");
        }

        #region Submit Button Command Functionality
        public ICommand SubmitCommand => new AsyncCommand(SubmitAsync);
        async Task SubmitAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new RoundCodePoppup();
                //var navigationPage = ((NavigationPage)App.Current.MainPage);
                await PopupNavigation.Instance.PushAsync(view);
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
        }
        #endregion Submit Button Command Functionality

    }
}
