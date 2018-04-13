
namespace WP8.Crebits.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    using WP8.Crebits.ViewModels;

    public class OverrideValueVisibilityConverter : IValueConverter
    {
        #region [ IValueConverter Implementation ]

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vm = value as OperationPageViewModel;
            if (vm != null)
            {
                return (vm.Id > 0 && vm.IsMonthly) ?
                    Visibility.Visible :
                    Visibility.Collapsed;
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