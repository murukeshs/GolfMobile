using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Golf.Converters
{
    public class TeamProfileConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == string.Empty || (string)value == null || (string)value == "")
                return "profile_defalut_pic.png"; //Paymet is success
            else
            {
                var image = (string)value;
                return image;//Paymet is failed
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == string.Empty || (string)value == null || (string)value == "")
                return "profile_defalut_pic.png"; //Paymet is success
            else
            {
                var image = (string)value;
                return image;//Paymet is failed
            }
        }
    }
}
