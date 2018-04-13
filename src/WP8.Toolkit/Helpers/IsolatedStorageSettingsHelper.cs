
namespace WP8.Toolkit.Helpers
{
    using System.IO.IsolatedStorage;

    public static class IsolatedStorageSettingsHelper
    {
        public static T GetValue<T>(string key, T defaultValue = default(T))
        {
            T value = defaultValue;

            var store = IsolatedStorageSettings.ApplicationSettings;
            if (store.Contains(key))
            {
                value = (T)store[key];
            }

            return value;
        }

        public static void SetValue(string key, object value, bool withAutoSave = true)
        {
            IsolatedStorageSettings.ApplicationSettings[key] = value;

            if (withAutoSave)
            {
                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        public static void Save()
        {
            IsolatedStorageSettings.ApplicationSettings.Save();
        }
    }
}
