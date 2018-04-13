
namespace WP8.Toolkit.Converters
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Windows.Data;

    public class IsNullOrEmptyConverter : IValueConverter
    {
        #region [ IValueConverter Implementation ]

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }

            if (value is string && ((string)value).Length == 0)
            {
                return true;
            }

            if (value is ICollection)
            {
                return ((ICollection)value).Count == 0;
            }

            return false;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
