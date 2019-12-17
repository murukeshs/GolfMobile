using System;
using System.Collections.Generic;
using System.ComponentModel;
using Golf.Controls;
using Golf.iOS.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NullableDatePicker), typeof(NullableDatePickerRenderer))]
namespace Golf.iOS.CustomRenderer
{
    public class NullableDatePickerRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null && Control != null)
            {
                AddClearButton();

                Control.BorderStyle = UITextBorderStyle.Line;
                Control.Layer.BorderColor = UIColor.White.CGColor;
                Control.Layer.BorderWidth = 1;
                //Control.TextColor = UIColor.Black;

                var entry = (NullableDatePicker)this.Element;
                if (!entry.NullableDate.HasValue)
                {
                    Control.Text = entry.PlaceHolder;
                    Control.TextColor = UIColor.LightGray; ;
                }
                else
                {
                    Control.TextColor = entry.TextColor.ToUIColor();
                }

                //if (Device.Idiom == TargetIdiom.Tablet)
                //{
                //    Control.Font = UIFont.SystemFontOfSize(25);
                //}
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Check if the property we are updating is the format property
            if (e.PropertyName == DatePicker.DateProperty.PropertyName || e.PropertyName == DatePicker.FormatProperty.PropertyName)
            {
                var entry = (NullableDatePicker)Element;

                // If we are updating the format to the placeholder then just update the text and return
                if (Element.Format == entry.PlaceHolder)
                {
                    Control.Text = entry.PlaceHolder;
                    Control.TextColor = entry.TextColor.ToUIColor();
                    return;
                }
                else
                {
                    Control.TextColor = entry.TextColor.ToUIColor();
                }
            }

            base.OnElementPropertyChanged(sender, e);
        }

        private void AddClearButton()
        {
            var originalToolbar = Control.InputAccessoryView as UIToolbar;

            if (originalToolbar != null && originalToolbar.Items.Length <= 2)
            {
                var clearButton = new UIBarButtonItem("Clear", UIBarButtonItemStyle.Plain, ((sender, ev) =>
                {
                    NullableDatePicker baseDatePicker = Element as NullableDatePicker;
                    Element.Unfocus();
                    //Element.TextColor = Element.TextColor.ToUIColor();
                    Element.Date = DateTime.Now;
                    baseDatePicker.CleanDate();
                }));

                var newItems = new List<UIBarButtonItem>();
                foreach (var item in originalToolbar.Items)
                {
                    newItems.Add(item);
                }
                newItems.Insert(0, clearButton);
                originalToolbar.Items = newItems.ToArray();
                originalToolbar.SetNeedsDisplay();
            }
        }
    }
}