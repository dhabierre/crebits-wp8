
namespace WP8.Crebits.Pages
{
    using Microsoft.Phone.Shell;

    using System;
    using System.Windows.Controls;

    using WP8.Crebits.Helpers;
    using WP8.Crebits.Resources;
    using WP8.Crebits.ViewModels;

    public partial class OperationPage : Microsoft.Phone.Controls.PhoneApplicationPage
    {
        #region [ Uri ]

        public static readonly string Uri = "/Pages/OperationPage.xaml";

        #endregion

        #region [ ViewModel ]

        private OperationPageViewModel ViewModel
        {
            get { return this.DataContext as OperationPageViewModel; }
        }

        #endregion

        #region [ Constructor ]

        public OperationPage()
        {
            this.InitializeComponent();
            this.BuildLocalizedApplicationBar();
        }

        #endregion

        #region [ Methods ]

        private void BuildLocalizedApplicationBar()
        {
            this.ApplicationBar = ApplicationBarHelper.GetApplicationBar();
            this.ApplicationBar.IsMenuEnabled = false;

            var appBarCheckButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/Check.png", UriKind.Relative));

            appBarCheckButton.Text = AppResources.Add;
            appBarCheckButton.Click += this.AppBarCheckButton_Click;

            this.ApplicationBar.Buttons.Add(appBarCheckButton);

            var appBarCancelButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/Cancel.png", UriKind.Relative));

            appBarCancelButton.Text = AppResources.Cancel;
            appBarCancelButton.Click += this.AppBarCancelButton_Click;

            this.ApplicationBar.Buttons.Add(appBarCancelButton);
        }

        #endregion

        #region [ Events ]

        private void AppBarCheckButton_Click(object sender, EventArgs e)
        {
            this.ValueTextBox.ForceUpdateSource(); // Force the Value TextBox update...
            this.OverrideValueTextBox.ForceUpdateSource(); // Force the OverrideValue TextBox update...

            if (this.ViewModel.Persist())
            {
                if (base.NavigationService.CanGoBack)
                    base.NavigationService.GoBack();
            }
        }

        private void AppBarCancelButton_Click(object sender, EventArgs e)
        {
            if (base.NavigationService.CanGoBack)
                base.NavigationService.GoBack();
        }

        #endregion
    }
}
