
namespace WP8.Crebits.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class ToPercentStringConverter : IValueConverter
    {
        #region [ IValueConverter Implementation ]

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                return string.Format("{0}%", Math.Round((double)value, System.Convert.ToInt32(parameter)));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}