
namespace WP8.Crebits
{
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Data.Linq;
    using Microsoft.Phone.Shell;

    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Navigation;

    using WP8.Crebits.Controls;
    using WP8.Crebits.DataContexts;
    using WP8.Crebits.DataServices;
    using WP8.Crebits.Entities;
    using WP8.Crebits.Helpers;
    using WP8.Crebits.Managers;
    using WP8.Crebits.Resources;
    using WP8.Crebits.ViewModels;

    using WP8.Toolkit.Managers;

    public partial class App : Application
    {
        #region [ Constants ]

        private static int DatabaseSchemaVersion = 1310;

        #endregion

        #region [ Constructor ]

        public App()
        {
            base.UnhandledException += this.Application_UnhandledException;

            this.InitializeComponent();
            this.InitializeTheme();

            this.InitializePhoneApplication();
            this.InitializeLanguage();

            if (Debugger.IsAttached)
            {
                //Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            SettingsHelper.IncreaseExecutionCounter();

            StatisticsManager.SubmitVersion();
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame
        {
            get;
            private set;
        }

        public static bool IsDarkTheme
        {
            get
            {
                var visibility = (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"];
                return visibility == Visibility.Visible;
            }
        }

        #endregion

        #region [ Methods ]

        private void InitializeLanguage()
        {
            try
            {
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                var flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                if (Debugger.IsAttached)
                    Debugger.Break();

                throw;
            }
        }

        private void InitializeTheme()
        {
            if (SettingsHelper.GetIsLightTheme())
            {
                ThemeManager.ToLightTheme();
            }
            else
            {
                ThemeManager.ToDarkTheme();
            }
        }

        #endregion

        #region [ Tiles ]

        public static void UpdateTile(double? cash = null, bool hasData = true)
        {
            if (cash == null) // load data...
            {
                using (var dataService = new DataService())
                {
                    var summaryViewModel = new SummaryViewModel();

                    summaryViewModel.Update(dataService.GetAllCredits(true), dataService.GetAllDebits(true));

                    cash = summaryViewModel.Cash;
                    hasData = summaryViewModel.HasData;
                }
            }

            if (!SettingsHelper.GetWithLiveTileDecimalDigits())
            {
                cash = (int)cash; // Remove the coma without rounding
            }

            var mediumTile = new MediumTileControl(
                "/Assets/Tiles/FlipCycleTileMedium.png" /* Transparent image with icon */,
                cash.ToString(),
                hasData);

            mediumTile.Update();
        }

        #endregion

        #region [ Events ]

        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            using (var db = new AppDataContext())
            {
                if (!db.DatabaseExists())
                {
                    db.CreateDatabase();

                    var dbUpdater = db.CreateDatabaseSchemaUpdater();

                    dbUpdater.DatabaseSchemaVersion = DatabaseSchemaVersion;
                    dbUpdater.Execute();
                }
                else
                {
                    // Update database...
                    // http://msdn.microsoft.com/en-us/library/windowsphone/develop/hh394022(v=vs.105).aspx

                    var dbUpdater = db.CreateDatabaseSchemaUpdater();

                    if (dbUpdater.DatabaseSchemaVersion < DatabaseSchemaVersion)
                    {
                        dbUpdater.AddColumn<Debit>("OverrideValue");
                        dbUpdater.AddColumn<Credit>("OverrideValue");

                        dbUpdater.DatabaseSchemaVersion = DatabaseSchemaVersion;
                        dbUpdater.Execute();
                    }
                }
            }
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            RaiseEventsManager.RaiseRefreshMainScreen(withForceLoad: false);
        }

        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            // Ensure that required application state is persisted here.
        }

        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            ErrorReportManager.StoreException("Navigation Failed", e.Exception);
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            ErrorReportManager.StoreException("Unhandled Exception", e.ExceptionObject);
        }

        #endregion

        #region [ Phone Application Initialization ]

        private bool _phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (_phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += this.CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += this.RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += this.CheckForResetNavigation;

            // Ensure we don't initialize again
            _phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += this.ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= this.ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion
    }
}