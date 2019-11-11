using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Golf.Droid.Services;
using Golf.Services;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

[assembly: Xamarin.Forms.Dependency(typeof(AdjustScreenSize))]
namespace Golf.Droid.Services
{
    public class AdjustScreenSize : IAdjustScreenSize
    {
        public void AdjustScreen()
        {
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }

        public void UnAdjustScreen()
        {
            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
        }
    }
}