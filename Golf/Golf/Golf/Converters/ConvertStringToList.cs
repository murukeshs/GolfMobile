using Golf.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Golf.Converters
{
    class ConvertStringToList : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value != null)
            {
                try
                {
                    var item = (string)value;
                    List<string> roles = item.Split(',').ToList<string>();
                    return roles;
                }
                catch(Exception ex)
                {
                    return ex;
                }
            }
            else
                return null;//Paymet is failed
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((List<string>)value != null)
            {
                try
                {
                    var item = (List<string>)value;
                    //List<string> roles = item.Split(',').ToList<string>();
                    return item;
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
            else
                return null;//Paymet is failed
        }
    }
}
