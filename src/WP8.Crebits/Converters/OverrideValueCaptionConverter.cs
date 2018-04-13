
namespace WP8.Crebits.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using WP8.Crebits.Resources;

    public class OverrideValueCaptionConverter : IValueConverter
    {
        #region [ IValueConverter Implementation ]

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format(AppResources.OverrideCaption, DateTime.Today.ToString("MMMM", culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}