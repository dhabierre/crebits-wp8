
namespace WP8.Crebits.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    using WP8.Crebits.Helpers;

    public class TotalValueToForegroundConverter : IValueConverter
    {
        #region [ IValueConverter Implementation ]

        private static readonly Color _defaultForeground = GetDefaultForeground();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                if ((double)value == 0)
                {
                    return new SolidColorBrush(Colors.Gray);
                }
            }

            return new SolidColorBrush(_defaultForeground);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        private static Color GetDefaultForeground()
        {
            if (SettingsHelper.GetIsLightTheme())
            {
                return Colors.Black;
            }

            return Colors.White;
        }
    }
}