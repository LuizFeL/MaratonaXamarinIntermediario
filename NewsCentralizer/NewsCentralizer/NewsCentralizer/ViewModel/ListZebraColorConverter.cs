using System;
using System.Globalization;
using Xamarin.Forms;

namespace NewsCentralizer.ViewModel
{
    public class ListZebraColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var iValue = (int)value;
                return (iValue % 2) != 0 ? Color.Transparent : Color.FromHex("#3399ff").MultiplyAlpha(0.2);
            }
            catch
            {
                return Color.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null; //Do nothing
        }
    }
}
