
namespace WP8.Crebits.Pages
{
    using System;
    using System.Windows;

    using WP8.Crebits.Managers;
    using WP8.Crebits.Resources;
    using WP8.Crebits.ViewModels;
    using WP8.Toolkit.Helpers;

    public partial class OptionsPage : Microsoft.Phone.Controls.PhoneApplicationPage
    {
        #region [ Uri ]

        public static readonly string Uri = "/Pages/OptionsPage.xaml";

        #endregion

        #region [ ViewModel ]

        private OptionsPageViewModel ViewModel
        {
            get { return this.DataContext as OptionsPageViewModel; }
        }

        #endregion

        #region [ Constructor ]

        public OptionsPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region [ Methods ]

        #endregion

        #region [ Events ]

        private void Purge_Click(object sender, RoutedEventArgs e)
        {
            string caption = AppResources.ContinueQuestion;
            string message = AppResources.PurgeMessage;

            VibrationDeviceHelper.Vibrate(TimeSpan.FromMilliseconds(500));

            if (MessageBox.Show(message, caption, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                this.ViewModel.Purge();

                RaiseEventsManager.RaiseRefreshMainScreen();

                if (base.NavigationService.CanGoBack)
                    base.NavigationService.GoBack();
            }
        }

        private void AppBarCloseButton_Click(object sender, EventArgs e)
        {
            if (base.NavigationService.CanGoBack)
                base.NavigationService.GoBack();
        }

        #endregion
    }
}
