
namespace WP8.Crebits.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    public class OperationForegroundConverter : IValueConverter
    {
        #region [ IValueConverter Implementation ]

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var operation = value as IOperation;
            if (operation != null)
            {
                if (operation.IsDisabled)
                {
                    return new SolidColorBrush(Colors.Gray);
                }

                string p = parameter.ToString();

                if (p == "Debit")
                {
                    return new SolidColorBrush(
                        operation.Caption.EndsWith("(+)")
                            ? Colors.Orange
                            :Colors.Red);
                }

                if (p == "Credit")
                {
                    return new SolidColorBrush(Colors.Green);
                }
            }

            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
