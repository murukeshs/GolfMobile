using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Golf.Converters
{
    public class GenderToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "Male")
                return "man_green.png"; 
            else
            {
                return "woman_green.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "Male")
                return "man_green.png";
            else
            {
                return "woman_green.png";
            }
        }
    }
}
