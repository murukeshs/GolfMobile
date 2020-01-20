using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.ViewModel;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.ParticipantsView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewParticipantByIdPage : ContentPage
	{
        public int playerId;

        public ViewParticipantByIdPage (int id)
		{
			InitializeComponent ();
            playerId = id;
            LoadPlayerListAsync();
        }

        async void LoadPlayerListAsync()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading();
                    //player type is 1 to get player list
                    var RestURL = App.User.BaseUrl + "User/selectUserById/" + playerId;
                    var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.User.AccessToken);
                    var response = await httpClient.GetAsync(RestURL);
                    var content = await response.Content.ReadAsStringAsync();
                    var Items = JsonConvert.DeserializeObject<selectUserById>(content);

                    FirstName.Text = Items.firstName;
                    LastName.Text = Items.lastName;
                    Gender.Text = Items.gender;
                    Email.Text = Items.email;
                    Phone.Text = Items.phoneNumber; 
                    var profile = Items.profileImage;
                    if(!string.IsNullOrEmpty(profile) || profile != null)
                    {
                        ProfileImage.Source = profile;
                    }
                    else
                    {
                        ProfileImage.Source = "profile_defalut_pic.png";
                    }
                    //Assign the Values to Listview
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    DependencyService.Get<IToast>().Show("Please check internet connection");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                UserDialogs.Instance.HideLoading();
                DependencyService.Get<IToast>().Show("Something went wrong, please try again later");
            }
        }
    }
}