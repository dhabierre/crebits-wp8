
namespace WP8.Crebits.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using WP8.Crebits.Resources;

    public class UnCategorizedDebitsToPercentConverter : IValueConverter
    {
        #region [ IValueConverter Implementation ]

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
            {
                return string.Format(AppResources.UnCategorizedDebitsPercent, Math.Round((double)value));
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