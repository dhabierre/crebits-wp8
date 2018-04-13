
namespace WP8.Crebits.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class ThemeSwitchConverter : IValueConverter
    {
        #region [ IValueConverter Implementation ]

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string p = parameter as string;

            if (p != null)
            {
                if (p == "HeaderBackgroundColor")
                {
                    return App.IsDarkTheme ?
                        "#282828" :
                        "#E0E0E0";
                }

                if (p == "HeaderBorderColor")
                {
                    return App.IsDarkTheme ?
                        "#303030" :
                        "#BBBBBB";
                }

                if (p == "ApplicationBarBackgroundColor")
                {
                    return App.IsDarkTheme ?
                        "#00000000" :
                        "#E0E0E0";
                }

                if (p == "BackgroundIcon")
                {
                    return App.IsDarkTheme ?
                        "/Assets/BackgroundIcons/BackgroundIconDarkTheme.png" :
                        "/Assets/BackgroundIcons/BackgroundIconLightTheme.png";
                }
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
