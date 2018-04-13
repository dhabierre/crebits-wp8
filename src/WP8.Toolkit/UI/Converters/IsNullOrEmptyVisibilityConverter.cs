
namespace WP8.Toolkit.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;

    public class IsNullOrEmptyVisibilityConverter : IsNullOrEmptyConverter
    {
        public bool Not { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bValue = (bool)base.Convert(value, targetType, parameter, culture);
            return (bValue) ^ Not ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
