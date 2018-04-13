
namespace WP8.Toolkit.Helpers
{
    using System;

    using Windows.Phone.Devices.Notification;

    public static class VibrationDeviceHelper
    {
        public static void Vibrate(TimeSpan duration)
        {
            var vibrationDevice = VibrationDevice.GetDefault();

            vibrationDevice.Vibrate(duration);
        }
    }
}
