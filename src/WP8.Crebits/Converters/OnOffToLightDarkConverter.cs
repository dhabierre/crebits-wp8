
namespace WP8.Crebits.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    using WP8.Crebits.Resources;

    public class OnOffToLightDarkConverter : IValueConverter
    {
        #region [ IValueConverter Implementation ]

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                return (string.Compare(value.ToString(), "ON", StringComparison.InvariantCultureIgnoreCase) == 0)
                    ? AppResources.LightTheme
                    : AppResources.DarkTheme;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}