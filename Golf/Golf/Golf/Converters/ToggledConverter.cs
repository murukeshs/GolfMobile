using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Golf.Converters
{
    public class ToggledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
            {
                 return "unchecked_icon.png";
            }
            else
                return false;//Paymet is failed
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value == true)
            {
                bool result = App.User.PlayersList.All(x => x.IsToggled == x.IsToggled ? x.IsToggled = true : x.IsToggled = false);
                //App.User.PlayersList.Where(x => x.IsToggled == true).ToList().ForEach(s => s.IsToggled = true);
                return result; //Paymet is success
            }
            else
                return false;//Paymet is failed
        }
    }
}
