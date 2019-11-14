using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Widget;
using Golf.Controls;
using Golf.Droid.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NullableDatePicker), typeof(NullableDatePickerRenderer))]
namespace Golf.Droid.CustomRenderer
{
    public class NullableDatePickerRenderer : ViewRenderer<NullableDatePicker, EditText>
    {
        DatePickerDialog _dialog;
        public NullableDatePickerRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<NullableDatePicker> e)
        {
            base.OnElementChanged(e);

            SetNativeControl(new EditText(Context));
            if (Control == null || e.NewElement == null)
                return;

            var entry = Element;
            Control.Click += OnPickerClick;
            Control.Text = !entry.NullableDate.HasValue ? entry.PlaceHolder : Element.Date.ToString(Element.Format);
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

        void SetDate(DateTime date)
        {
            Control.Text = date.ToString(Element.Format);
            Element.Date = date;
        }

        private void ShowDatePicker()
        {
            CreateDatePickerDialog(Element.Date.Year, Element.Date.Month - 1, Element.Date.Day);
            _dialog.Show();
        }

        void CreateDatePickerDialog(int year, int month, int day)
        {
            NullableDatePicker view = Element;
            _dialog = new DatePickerDialog(Context, (o, e) =>
            {
                view.Date = e.Date;
                ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();

                _dialog = null;
            }, year, month, day);



            _dialog.SetButton("Done", (sender, e) =>
            {
                Element.Format = Element._originalFormat;
                SetDate(_dialog.DatePicker.DateTime);
                //Control.SetTextColor(Android.Graphics.Color.Black);
                Element.AssignValue();
            });
            _dialog.SetButton2("Clear", (sender, e) =>
            {
                Element.CleanDate();
                Control.Text = Element.Format;
                //Control.SetTextColor(Android.Graphics.Color.Gray);
            });
        }
    }
}