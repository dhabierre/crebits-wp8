
namespace WP8.Crebits.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    using WP8.Crebits.Helpers;

    public class MoneyToStringConverter : IValueConverter
    {
        #region [ IValueConverter Implementation ]

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double || value is double?)
            {
                return DoubleHelper.ToString((double)value);
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) // From string to double
        {
            return (value != null) ? DoubleHelper.ToDouble(value.ToString()) : null;
        }

        #endregion
    }
}