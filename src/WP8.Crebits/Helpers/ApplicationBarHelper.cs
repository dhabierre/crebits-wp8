
namespace WP8.Crebits.Helpers
{
    using Microsoft.Phone.Shell;

    using WP8.Crebits.Converters;
    using WP8.Toolkit.Helpers;

    public static class ApplicationBarHelper
    {
        public static IApplicationBar GetApplicationBar()
        {
            var applicationBar = new ApplicationBar();

            string hexaColor = new ThemeSwitchConverter().Convert(null, null, "ApplicationBarBackgroundColor", null) as string;
            applicationBar.BackgroundColor = ColorHelper.ConvertStringToColor(hexaColor);

            return applicationBar;
        }
    }
}
