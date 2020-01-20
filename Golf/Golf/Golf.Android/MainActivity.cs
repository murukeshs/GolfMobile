using Android.App;
using Android.Content.PM;
using Android.OS;
using Acr.UserDialogs;
using Xamarin.Forms;
using Golf.Services;
using Plugin.CurrentActivity;
using Golf.Droid.Services;
using Plugin.Permissions;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Auth.Api;
using Xamarin.Facebook;

[assembly: MetaData("com.facebook.sdk.ApplicationId", Value = "@string/facebook_app_id")]
namespace Golf.Droid
{
    
    [Activity(Label = "Golf", Icon = "@mipmap/launcher", Theme = "@style/Theme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        [System.Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Set the Splash Screen Image theme for android
            SetTheme(Resource.Style.MainTheme); // <-- Added splash screen for android
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            DependencyService.Register<IAdjustScreenSize, AdjustScreenSize>();
            DependencyService.Register<IGoogleManager, GoogleManager>();
            //Facebook Sdk Initialization
            
            base.OnCreate(savedInstanceState);
            FacebookSdk.SdkInitialize(this);
            FacebookSdk.ApplicationId = "1036432036689831";
            UserDialogs.Init(this);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            

            DependencyService.Register<IFacebookManager, FacebookManager>();
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

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 1)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                GoogleManager.Instance.OnAuthCompleted(result);
            }

            var manager = DependencyService.Get<IFacebookManager>();
            if (manager != null)
            {
                (manager as FacebookManager)._callbackManager.OnActivityResult(requestCode, (int)resultCode, data);
            }
        }
    }
}