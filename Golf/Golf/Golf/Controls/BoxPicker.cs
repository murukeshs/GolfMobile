using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Golf.Controls
{
    public class BoxPicker : Picker
    {
        public static readonly BindableProperty ImageProperty =
           BindableProperty.Create(nameof(Image), typeof(string), typeof(BoxPicker), string.Empty);

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly BindableProperty BorderColorProperty =
     BindableProperty.Create(nameof(BorderColor),
         typeof(Color), typeof(BoxPicker), Color.Default);
        // Gets or sets BorderColor value  
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty BorderWidthProperty =
        BindableProperty.Create(nameof(BorderWidth), typeof(int),
#pragma warning disable CS0618 // Type or member is obsolete
            typeof(BoxPicker), Device.OnPlatform<int>(1, 2, 2));
#pragma warning restore CS0618 // Type or member is obsolete
        // Gets or sets BorderWidth value  
        public int BorderWidth
        {
            get => (int)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }
        public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius),
#pragma warning disable CS0618 // Type or member is obsolete
            typeof(double), typeof(BoxPicker), Device.OnPlatform<double>(6, 7, 7));
#pragma warning restore CS0618 // Type or member is obsolete
        // Gets or sets CornerRadius value  
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly BindableProperty IsCurvedCornersEnabledProperty =
        BindableProperty.Create(nameof(IsCurvedCornersEnabled),
            typeof(bool), typeof(BoxPicker), true);
        // Gets or sets IsCurvedCornersEnabled value  
        public bool IsCurvedCornersEnabled
        {
            get => (bool)GetValue(IsCurvedCornersEnabledProperty);
            set => SetValue(IsCurvedCornersEnabledProperty, value);
        }

        public BoxPicker()
        {
            //BorderColor = WpcStylingColors.BorderColorLightGray;
            //FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Picker));
            //TextColor = WpcStylingColors.ColorSoftBlack;
            //FontFamily = "Roboto-Regular";
            //BackgroundColor = WpcStylingColors.BackgroundColorWhite;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.Center;
        }
    }
}
