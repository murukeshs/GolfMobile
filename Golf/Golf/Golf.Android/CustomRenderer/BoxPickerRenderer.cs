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
using Android.Util;
using Android.Views;
using Android.Widget;
using Golf.Controls;
using Golf.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BoxPicker), typeof(BoxPickerRenderer))]
namespace Golf.Droid.CustomRenderer
{
    public class BoxPickerRenderer : PickerRenderer
    {
        public BoxPickerRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var view = (BoxPicker)Element;
                // creating gradient drawable for the curved background  
                var _gradientBackground = new GradientDrawable();
                _gradientBackground.SetShape(ShapeType.Rectangle);
                _gradientBackground.SetColor(view.BackgroundColor.ToAndroid());

                // Thickness of the stroke line  
                _gradientBackground.SetStroke(view.BorderWidth, view.BorderColor.ToAndroid());

                // Radius for the curves  
                _gradientBackground.SetCornerRadius(
                    DpToPixels(this.Context, Convert.ToSingle(view.CornerRadius)));

                // set the background of the   
                Control.SetBackground(_gradientBackground);
                Control.SetSingleLine(false);
                // Set padding for the internal text from border  
                Control.SetPadding(
                    (int)DpToPixels(Context, Convert.ToSingle(12)), Control.PaddingTop,
                    (int)DpToPixels(this.Context, Convert.ToSingle(12)), Control.PaddingBottom);

                Control.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(view.Image), null);
            }

                
        }
        public static float DpToPixels(Context context, float valueInDp)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
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
            catch (Exception ex)
            {
                var a = ex.Message;
                return null;
            }
        }
    }
}