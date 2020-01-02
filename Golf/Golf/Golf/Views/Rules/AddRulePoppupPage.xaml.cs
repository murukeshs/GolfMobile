using Acr.UserDialogs;
using Golf.Services;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Golf.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.Rules
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddRulePoppupPage :  PopupPage
    {
        public string AddNewRuleText
        {
            get
            {
                return _AddNewRuleText;
            }
            set
            {
                _AddNewRuleText = value;
                OnPropertyChanged(nameof(AddNewRuleText));
            }
        }
        private string _AddNewRuleText = string.Empty;

        public AddRulePoppupPage ()
		{
            InitializeComponent ();
            BindingContext = this;
            
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


        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            base.OnBackButtonPressed();
            return true;
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            base.OnBackgroundClicked();
            return false;
        }

           

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private void RoundedButton_Clicked(object sender, EventArgs e)
        {
            AddNewRule();
        }

        async void AddNewRule()
        {
            if (!string.IsNullOrEmpty(AddNewRuleText))
            {
                try
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        UserDialogs.Instance.ShowLoading();
                       

                        var data = new Rule
                        {
                            roundRules = AddNewRuleText
                        };

                        var result = await App.ApiClient.AddRoundRule(data);

                        if (result != null)
                        {
                            AddNewRuleText = string.Empty;
                            UserDialogs.Instance.HideLoading();
                            MessagingCenter.Send<App>((App)Application.Current, "OnCategoryCreated");
                            UserDialogs.Instance.Alert("New Rule Successfully Added!", "Success", "Ok");
                            await PopupNavigation.Instance.PopAsync();
                          
                        }

                       


                        //string RestURL = App.User.BaseUrl + "Round/roundRules";
                        //Uri requestUri = new Uri(RestURL);
                        //string json = JsonConvert.SerializeObject(data);
                        //var httpClient = new HttpClient();
                        //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                        //var response = await httpClient.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json"));
                        //string responJsonText = await response.Content.ReadAsStringAsync();
                        //if (response.IsSuccessStatusCode)
                        //{
                        //    AddNewRuleText = string.Empty;                           
                        //    UserDialogs.Instance.HideLoading();
                        //    MessagingCenter.Send<App>((App)Application.Current, "OnCategoryCreated");
                        //    UserDialogs.Instance.Alert("New Rule Successfully Added!", "Success", "Ok");
                        //    await PopupNavigation.Instance.PopAsync();
                        //}
                        //else
                        //{
                        //    var error = JsonConvert.DeserializeObject<error>(responJsonText);
                        //    UserDialogs.Instance.HideLoading();
                        //    UserDialogs.Instance.Alert(error.errorMessage, "Alert", "Ok");
                        //}
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        DependencyService.Get<IToast>().Show("Please check internet connection");
                    }
                }
                catch (Exception ex)
                {
                    var a = ex.Message;
                    if (a == "System.Net.WebException")
                    {
                        UserDialogs.Instance.HideLoading();
                        DependencyService.Get<IToast>().Show("Please check internet connection");
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
                    }
                }
            }
            else
            {
                UserDialogs.Instance.Alert("Please fill the New Rule.", "Alert", "Ok");
            }
        }
    }
}