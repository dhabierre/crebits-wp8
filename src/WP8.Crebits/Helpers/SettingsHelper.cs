
namespace WP8.Crebits.Helpers
{
    using System.Windows;

    using WP8.Toolkit.Helpers;

    public static class SettingsHelper
    {
        #region [ IsLightTheme ]

        private static readonly string IsLightTheme = "IsLightTheme";

        public static bool GetIsLightTheme()
        {
            bool isLightTheme = false;

            if ((Visibility)Application.Current.Resources["PhoneLightThemeVisibility"] == Visibility.Visible)
            {
                isLightTheme = true;
            }

            return IsolatedStorageSettingsHelper.GetValue<bool>(IsLightTheme, isLightTheme);
        }

        public static void SetIsLightTheme(bool value)
        {
            IsolatedStorageSettingsHelper.SetValue(IsLightTheme, value);
        }

        #endregion

        #region [ WithLiveTileDecimalDigits ]

        private static readonly string WithLiveTileDecimalDigits = "WithLiveTileDecimalDigits";

        public static bool GetWithLiveTileDecimalDigits()
        {
            return IsolatedStorageSettingsHelper.GetValue<bool>(WithLiveTileDecimalDigits, false);
        }

        public static void SetWithLiveTileDecimalDigits(bool value)
        {
            IsolatedStorageSettingsHelper.SetValue(WithLiveTileDecimalDigits, value);
        }

        #endregion

        #region [ ExportDataEmail ]

        private static readonly string ExportDataEmail = "ExportDataEmail";

        public static string GetExportDataEmail()
        {
            return IsolatedStorageSettingsHelper.GetValue<string>(ExportDataEmail, string.Empty);
        }

        public static void SetExportDataEmail(string value)
        {
            IsolatedStorageSettingsHelper.SetValue(ExportDataEmail, value);
        }

        #endregion

        #region [ MinCashLimitValue ]

        private static readonly string MinCashLimitValue = "MinCashLimitValue";

        public static readonly int DefaultMinCashLimitValue = 250;

        public static int? GetMinCashLimitValue()
        {
            int? value = IsolatedStorageSettingsHelper.GetValue<int?>(MinCashLimitValue, 250);
            return (value != null) ? value : DefaultMinCashLimitValue;
        }

        public static void SetMinCashLimitValue(int? value)
        {
            IsolatedStorageSettingsHelper.SetValue(MinCashLimitValue, value);
        }

        #endregion

        #region [ MinCashLimitColor ]

        private static readonly string MinCashLimitColor = "MinCashLimitColor";

        public static string GetMinCashLimitColor()
        {
            return IsolatedStorageSettingsHelper.GetValue<string>(MinCashLimitColor, "Orange");
        }

        public static void SetMinCashLimitColor(string value)
        {
            IsolatedStorageSettingsHelper.SetValue(MinCashLimitColor, value);
        }

        #endregion

        #region [ Notes ]

        private static readonly string Notes = "Notes";

        public static string GetNotes()
        {
            return IsolatedStorageSettingsHelper.GetValue<string>(Notes, string.Empty);
        }

        public static void SetNotes(string value)
        {
            IsolatedStorageSettingsHelper.SetValue(Notes, value);
        }

        #endregion

        #region [ ExecutionCounter ]

        private static readonly string ExecutionCounter = "ExecutionCounter";

        public static int GetExecutionCounter()
        {
            return IsolatedStorageSettingsHelper.GetValue(ExecutionCounter, 1);
        }

        public static void IncreaseExecutionCounter()
        {
            var value = GetExecutionCounter() + 1;

            SetExecutionCounter(value);
        }

        public static void SetExecutionCounter(int value)
        {
            IsolatedStorageSettingsHelper.SetValue(ExecutionCounter, value);
        }

        #endregion

        #region [ HasFeedback ]

        private static readonly string HasFeedback = "HasFeedback";

        public static bool GetHasFeedback()
        {
            return IsolatedStorageSettingsHelper.GetValue<bool>(HasFeedback, false);
        }

        public static void SetHasFeedback(bool value)
        {
            IsolatedStorageSettingsHelper.SetValue(HasFeedback, value);
        }

        #endregion
    }
}
