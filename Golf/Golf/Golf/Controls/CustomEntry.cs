using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Golf.Controls
{
    public class CustomEntry : Entry
    {
        public static readonly BindableProperty BorderColorProperty =
       BindableProperty.Create(nameof(BorderColor),
           typeof(Color), typeof(CustomEntry), Color.Gray);
        // Gets or sets BorderColor value  
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty DisableColorProperty =
      BindableProperty.Create(nameof(DisableColor),
          typeof(Color), typeof(CustomEntry), Color.LightGray);
        // Gets or sets BorderColor value  
        public Color DisableColor
        {
            get => (Color)GetValue(DisableColorProperty);
            set => SetValue(DisableColorProperty, value);
        }

        public static readonly BindableProperty BorderWidthProperty =
        BindableProperty.Create(nameof(BorderWidth), typeof(int),
#pragma warning disable CS0618 // Type or member is obsolete
            typeof(CustomEntry), Device.OnPlatform<int>(1, 2, 2));
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
            typeof(double), typeof(CustomEntry), Device.OnPlatform<double>(6, 7, 7));
#pragma warning restore CS0618 // Type or member is obsolete
        // Gets or sets CornerRadius value  
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly BindableProperty IsCurvedCornersEnabledProperty =
        BindableProperty.Create(nameof(IsCurvedCornersEnabled),
            typeof(bool), typeof(CustomEntry), true);
        // Gets or sets IsCurvedCornersEnabled value  
        public bool IsCurvedCornersEnabled
        {
            get => (bool)GetValue(IsCurvedCornersEnabledProperty);
            set => SetValue(IsCurvedCornersEnabledProperty, value);
        }

        public CustomEntry()

        {
            //CornerRadius = 10;
            //BorderColor = WpcStylingColors.BorderColorLightGray;
            BorderWidth = 1;
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.Center;
           // FontFamily = "Roboto-Regular";
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Entry));
            //TextColor = WpcStylingColors.ColorSoftBlack;
            //BackgroundColor = WpcStylingColors.BackgroundColorWhite;
            // HeightRequest = 40;
        }
    }
}
