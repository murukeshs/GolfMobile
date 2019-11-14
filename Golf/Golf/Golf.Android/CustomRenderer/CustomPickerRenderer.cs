using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Golf.Controls;
using Golf.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace Golf.Droid.CustomRenderer
{
    public class CustomPickerRenderer : PickerRenderer
    {
        CustomPicker element;

        public CustomPickerRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            element = (CustomPicker)this.Element;

            if (Control != null && this.Element != null && !string.IsNullOrEmpty(element.Image))
                Control.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(element.Image), null);
            //Control.Background = AddPickerStyles(element.Image);
        }

       

        private BitmapDrawable GetDrawable(string imagePath)
        {
            try
            {
                string file = imagePath;
                string imagePathNew = file.Split('.')[0].Trim();

                int resID = Resources.GetIdentifier(imagePathNew, "drawable", this.Context.PackageName);
                var drawable = ContextCompat.GetDrawable(this.Context, resID);
                var bitmap = ((BitmapDrawable)drawable).Bitmap;

                var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 70, 70, true));
                result.Gravity = Android.Views.GravityFlags.Right;

                return result;
            }
            catch(Exception ex)
            {
                var a = ex.Message;
                return null;
            }
        }

    }
}