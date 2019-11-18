using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Util;
using Golf.Controls;
using Golf.Droid.CustomRenderer;
using Golf.Enums;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace Golf.Droid.CustomRenderer
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntry CustomEntryElement => Element as CustomEntry;
        public CustomEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var view = (CustomEntry)Element;
                if (view.IsCurvedCornersEnabled)
                {
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
                }
                // Set padding for the internal text from border  
                Control.SetPadding(
                    (int)DpToPixels(this.Context, Convert.ToSingle(12)), Control.PaddingTop,
                    (int)DpToPixels(this.Context, Convert.ToSingle(12)), Control.PaddingBottom);

                var editText = this.Control;
                if (!string.IsNullOrEmpty(CustomEntryElement.Image))
                {
                    switch (CustomEntryElement.ImageAlignment)
                    {
                        case ImageAlignment.Left:
                            editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(view.Image), null, null, null);
                            break;
                        case ImageAlignment.Right:
                            editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(view.Image), null);
                            break;
                    }
                }
                editText.CompoundDrawablePadding = 25;
            }
        }
        public static float DpToPixels(Context context, float valueInDp)
        {
            DisplayMetrics metrics = context.Resources.DisplayMetrics;
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, valueInDp, metrics);
        }

        private BitmapDrawable GetDrawable(string imageEntryImage)
        {
            try
            {
                string file = imageEntryImage;
                string imagePath = file.Split('.')[0].Trim();

                int resID = Resources.GetIdentifier(imagePath, "drawable", this.Context.PackageName);
                var drawable = ContextCompat.GetDrawable(this.Context, resID);
                var bitmap = ((BitmapDrawable)drawable).Bitmap;

                return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, CustomEntryElement.ImageWidth * 2, CustomEntryElement.ImageHeight * 2, true));
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                return null;
            }
        }
    }
}