using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Golf.Controls;
using Golf.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomTimePiker), typeof(CustomTimePikerRenderer))]
namespace Golf.Droid.CustomRenderer
{
    public class CustomTimePikerRenderer : ViewRenderer<CustomTimePiker, EditText>
    {
        TimePickerDialog _dialog;
        public CustomTimePikerRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomTimePiker> e)
        {
            base.OnElementChanged(e);

            SetNativeControl(new EditText(Context));
            if (Control == null || e.NewElement == null)
                return;

            var entry = Element;
            Control.Click += OnPickerClick;
            Control.Text = !entry.NullableTime.HasValue ? entry.PlaceHolder : Element.Time.ToString(Element.Format);
            //To check the control has contain place holder text if its have set the Gray color or set date to black color
            Control.SetTextColor(entry.TextColor.ToAndroid());
            Control.KeyListener = null;
            Control.FocusChange += OnPickerFocusChange;
            Control.Enabled = Element.IsEnabled;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Xamarin.Forms.DatePicker.DateProperty.PropertyName || e.PropertyName == Xamarin.Forms.DatePicker.FormatProperty.PropertyName)
            {
                var entry = Element;

                if (Element.Format == entry.PlaceHolder)
                {
                    Control.Text = entry.PlaceHolder;
                    Control.SetTextColor(entry.TextColor.ToAndroid());
                    return;
                }
            }

            base.OnElementPropertyChanged(sender, e);
        }

        void OnPickerFocusChange(object sender, FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                ShowDatePicker();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                Control.Click -= OnPickerClick;
                Control.FocusChange -= OnPickerFocusChange;

                if (_dialog != null)
                {
                    _dialog.Hide();
                    _dialog.Dispose();
                    _dialog = null;
                }
            }

            base.Dispose(disposing);
        }

        void OnPickerClick(object sender, EventArgs e)
        {
            ShowDatePicker();
        }

        void SetDate(TimeSpan date)
        {
            Control.Text = date.ToString(Element.Format);
            Element.Time = date;
        }

        private void ShowDatePicker()
        {
            CreateDatePickerDialog(Element.Time);
            _dialog.Show();
        }

        void CreateDatePickerDialog(TimeSpan time)
        {
            CustomTimePiker view = Element;
            _dialog = new TimePickerDialog(Context, (o, e) =>
            {
                view.SetValue(Xamarin.Forms.TimePicker.TimeProperty, time);
                //view.Time = e.Date;
                ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();

                _dialog = null;
            }, time.Hours, time.Minutes, false);

            _dialog.SetButton("Done", (sender, e) =>
            {
                Element.Format = Element._originalFormat;
                //_dialog.UpdateTime(Element.Time.Hours, Element.Time.Minutes);
                SetDate(Element.Time);
                //Control.SetTextColor(Android.Graphics.Color.Black);
                Element.AssignValue();
            });
            _dialog.SetButton2("Clear", (sender, e) =>
            {
                Element.CleanTime();
                Control.Text = Element.Format;
                //Control.SetTextColor(Android.Graphics.Color.Gray);
            });
        }
    }
}