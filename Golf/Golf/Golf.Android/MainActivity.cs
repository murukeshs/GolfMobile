using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using Xamarin.Forms;
using Golf.Services;
using Plugin.CurrentActivity;
using Golf.Droid.Services;
using Plugin.Permissions;
using Android.Content;
using Android.Database;
using Android.Provider;
using System.Collections.Generic;

namespace Golf.Droid
{
    [Activity(Label = "Golf", Icon = "@mipmap/icon", Theme = "@style/Theme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Set the Splash Screen Image theme for android
            SetTheme(Resource.Style.MainTheme); // <-- Added splash screen for android
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            DependencyService.Register<IAdjustScreenSize, AdjustScreenSize>();
            UserDialogs.Init(this);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}