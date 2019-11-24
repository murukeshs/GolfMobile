using Acr.UserDialogs;
using Golf.Models;
using Golf.Services;
using Golf.ViewModel;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Golf.Views.ParticipantsView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ViewParticipantByIdPage : ContentPage
	{
        public int playerId;
        public string UserTypeValues;
        public ObservableCollection<UserTypes> UserTypeItems = new ObservableCollection<UserTypes>();

        public ViewParticipantByIdPage (int id)
		{
			InitializeComponent ();
            playerId = id;
            LoadPlayerListAsync();

            UserTypeItems = new ObservableCollection<UserTypes>()
            {
                new UserTypes {RoleTypeName = "Player", DefalutValue = "1", Checked = false },
                new UserTypes {RoleTypeName = "Moderator", DefalutValue = "2", Checked = false },
                new UserTypes {RoleTypeName = "Score Keeper", DefalutValue = "3", Checked = false },
                new UserTypes {RoleTypeName = "Organizer", DefalutValue = "4", Checked = false },
                new UserTypes {RoleTypeName = "Spectator", DefalutValue = "5", Checked = false },
            };
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
                    UserTypeValues = Items.userType;
                    LoadType();
                    var profile = Items.profileImage;
                    if(!string.IsNullOrEmpty(profile) || profile != null)
                    {
                        ProfileImage.Source = profile;
                    }
                    else
                    {
                        ProfileImage.Source = "profile_defalut_pic.png";
                    }
                    LoadType();
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

        void LoadType()
        {
            var list = new List<string>();
            // var MatchRuleID = UserTypeValues.Split(",", list);
            var UserTypeList = UserTypeValues.Split(',').ToList();
            //var list = UserTypeValues.Split(",");
            foreach (string item in UserTypeList)
            {
                UserTypeItems.Where(w => w.RoleTypeName == item).ToList().ForEach(s => s.Checked = true);
            }
            CheckboxList.ItemsSource = UserTypeItems;
        }
    }
}