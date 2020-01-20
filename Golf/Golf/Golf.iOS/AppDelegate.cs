using System;
using System.Collections.Generic;
using System.Linq;
using Facebook.CoreKit;
using Foundation;
using Golf.Services;
using Google.SignIn;
using UIKit;
using Xamarin.Forms;

namespace Golf.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        [Obsolete]
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();
            global::Xamarin.Forms.Forms.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            //For Google external login 
            DependencyService.Register<IGoogleManager, GoogleManager>();
           // DependencyService.Register<IFacebookManager, FacebookManager>();
            //You need to set the "BuildAction" to "BundleResources" in the GoogleService-Info.plist file properties other wise you cant able to acccess this file
            //For debug mode to run the google signin ios refre this below link
            //https://stackoverflow.com/questions/38812761/gidsignin-keychain-error-ios-10-xcode-8
            var googleServiceDictionary = NSDictionary.FromFile("GoogleService-Info.plist");
            SignIn.SharedInstance.ClientID = googleServiceDictionary["CLIENT_ID"].ToString();
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            base.OnActivated(uiApplication);
            AppEvents.ActivateApp();
        }

        [Obsolete]
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            var openUrlOptions = new UIApplicationOpenUrlOptions(options);
            return SignIn.SharedInstance.HandleUrl(url, openUrlOptions.SourceApplication, openUrlOptions.Annotation);
        }
    }
}
