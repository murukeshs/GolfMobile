using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Golf.Controls;
using Golf.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace Golf.Droid.CustomRenderer
{
    class ExtendedEntryRenderer : EntryRenderer
    {
        public ExtendedEntry ExtendedEntryElement => Element as ExtendedEntry;

        public ExtendedEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.InputType |= Android.Text.InputTypes.TextFlagNoSuggestions;
                UpdateLineColor();

                //ExtendedEntryElement = (ExtendedEntry)this.Element;

                var editText = this.Control;
                if (!string.IsNullOrEmpty(ExtendedEntryElement.Image))
                {
                    switch (ExtendedEntryElement.ImageAlignment)
                    {
                        case ImageAlignment.Left:
                            editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(ExtendedEntryElement.Image), null, null, null);
                            break;
                        case ImageAlignment.Right:
                            editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(ExtendedEntryElement.Image), null);
                            break;
                    }
                }
                editText.CompoundDrawablePadding = 25;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals(nameof(ExtendedEntry.LineColor)))
            {
                UpdateLineColor();
            }
        }

        private void UpdateLineColor()
        {
            Control?.Background?.SetColorFilter(ExtendedEntryElement.LineColor.ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcAtop);
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

                return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, ExtendedEntryElement.ImageWidth * 2, ExtendedEntryElement.ImageHeight * 2, true));
            }
            catch (Exception ex)
            {
                var a = ex.Message;
                return null;
            }
        }
    }
}