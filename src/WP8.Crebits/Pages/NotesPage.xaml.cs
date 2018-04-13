
namespace WP8.Crebits.Pages
{
    using Microsoft.Phone.Shell;

    using System;

    using WP8.Crebits.Helpers;
    using WP8.Crebits.Resources;
    using WP8.Crebits.ViewModels;

    public partial class NotesPage : Microsoft.Phone.Controls.PhoneApplicationPage
    {
        #region [ Uri ]

        public static readonly string Uri = "/Pages/NotesPage.xaml";

        #endregion

        #region [ ViewModel ]

        private NotesPageViewModel ViewModel
        {
            get { return this.DataContext as NotesPageViewModel; }
        }

        #endregion

        #region [ Constructor ]

        public NotesPage()
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

            var appBarApplyButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/Check.png", UriKind.Relative));

            appBarApplyButton.Text = AppResources.Apply;
            appBarApplyButton.Click += this.AppBarApplyButton_Click;

            this.ApplicationBar.Buttons.Add(appBarApplyButton);

            var appBarCloseButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/Cancel.png", UriKind.Relative));

            appBarCloseButton.Text = AppResources.Close;
            appBarCloseButton.Click += this.AppBarCloseButton_Click;

            this.ApplicationBar.Buttons.Add(appBarCloseButton);
        }

        #endregion

        #region [ Events ]

        private void AppBarApplyButton_Click(object sender, EventArgs e)
        {
            if (this.ViewModel.Save())
            {
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
