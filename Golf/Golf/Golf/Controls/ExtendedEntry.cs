using Golf.Enums;
using Golf.Styles;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Golf.Controls
{
    public class ExtendedEntry : Entry
    {
        public static readonly BindableProperty LineColorProperty =
            BindableProperty.Create("LineColor", typeof(Color), typeof(ExtendedEntry), Color.Default);

        public Color LineColor
        {
            get => (Color)GetValue(LineColorProperty);
            set => SetValue(LineColorProperty, value);
        }

        public static readonly BindableProperty ImageProperty =
           BindableProperty.Create(nameof(Image), typeof(string), typeof(ExtendedEntry), string.Empty);

        public static readonly BindableProperty ImageHeightProperty =
           BindableProperty.Create(nameof(ImageHeight), typeof(int), typeof(ExtendedEntry), 40);

        public static readonly BindableProperty ImageWidthProperty =
          BindableProperty.Create(nameof(ImageWidth), typeof(int), typeof(ExtendedEntry), 40);

        public static readonly BindableProperty ImageAlignmentProperty =
           BindableProperty.Create(nameof(ImageAlignment), typeof(ImageAlignment), typeof(ExtendedEntry), ImageAlignment.Left);

        public int ImageWidth
        {
            get { return (int)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public int ImageHeight
        {
            get { return (int)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public ImageAlignment ImageAlignment
        {
            get { return (ImageAlignment)GetValue(ImageAlignmentProperty); }
            set { SetValue(ImageAlignmentProperty, value); }
        }

        //public ExtendedEntry()
        //{
        //    //Acces the static styles like this (Color)App.Current.Resources["WhiteColor"]; to declar the colors
        //    TextColor = (Color)App.Current.Resources["WhiteColor"];
        //    LineColor = (Color)App.Current.Resources["WhiteColor"];
        //    PlaceholderColor = (Color)App.Current.Resources["WhiteColor"];
        //}
    }

    //Enum assigned for Image Alignment
   
}
