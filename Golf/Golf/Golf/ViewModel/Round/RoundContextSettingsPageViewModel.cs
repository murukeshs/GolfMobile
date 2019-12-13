using Acr.UserDialogs;
using Golf.Utils;
using Golf.Views.CreateRoundView;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Golf.ViewModel.Round
{
    public class RoundContextSettingsPageViewModel : BaseViewModel
    {

        public RoundContextSettingsPageViewModel()
        {
            SettingsList = new List<string>(new[] { "Carryover - if no one wins the hole", "N tie", "Greater than 1 or equal to", "No Carryover" });

            RoundExtrasList = new List<string>(new[] { "Greenies", "Skins","Closet to the pin", "Longest drive" });
        }

        #region Property Declaration

        public List<string> SettingsList { get; set; }

        public List<string> RoundExtrasList { get; set; }

        #endregion

        #region Submit Button Command Functionality

        public ICommand SubmitCommand => new AsyncCommand(SubmitAsync);

        async Task SubmitAsync()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var view = new RoundCodePoppup();
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
