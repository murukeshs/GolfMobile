using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Golf.Controls
{
    public class RoundedButton : Button
    {
        public new static BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(RoundedButton), default(Thickness), defaultBindingMode: BindingMode.OneWay);

        public new Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
        public RoundedButton()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                Padding = new Thickness(5, 0, 0, 5);
            }

            BackgroundColor = (Color)App.Current.Resources["LightGreenColor"];
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));
            TextColor = (Color)App.Current.Resources["WhiteColor"]; 
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.Center;

            //HeightRequest = 50;
            //BorderWidth = 2;

            if (Device.RuntimePlatform == Device.iOS)
            {
                CornerRadius = 20;
            }
            if (Device.RuntimePlatform == Device.Android)
            {
                CornerRadius = 25;
            }
        }
    }
}
