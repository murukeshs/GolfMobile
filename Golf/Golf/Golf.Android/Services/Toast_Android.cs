using System;
using Android.App;
using Android.Widget;
using Golf.Droid.Services;
using Golf.Services;

[assembly: Xamarin.Forms.Dependency(typeof(Toast_Android))]
namespace Golf.Droid.Services
{
    public class Toast_Android : IToast
    {
        public void Show(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}