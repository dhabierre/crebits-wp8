
namespace WP8.Crebits.Pages
{
    using Microsoft.Phone.Tasks;

    using System.Windows;

    using WP8.Crebits.ViewModels;

    public partial class AboutPage : Microsoft.Phone.Controls.PhoneApplicationPage
    {
        #region [ Uri ]

        public static readonly string Uri = "/Pages/AboutPage.xaml";

        #endregion

        #region [ ViewModel ]

        private AboutPageViewModel ViewModel
        {
            get { return this.DataContext as AboutPageViewModel; }
        }

        #endregion

        #region [ Constructors ]

        public AboutPage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region [ Events ]

        //private void FeedbackButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var email = new EmailComposeTask();

        //    email.Subject = string.Format("{0} v{1} (WP)",
        //        AppHelper.ApplicationTitle,
        //        AppHelper.ApplicationVersion);

        //    email.To = AppResources.AuthorEmail;

        //    email.Show();
        //}

        private void RateButton_Click(object sender, RoutedEventArgs e)
        {
            new MarketplaceReviewTask().Show();
        }

        //private void WindowsStoreButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var marketplaceDetailTask = new MarketplaceDetailTask
        //    {
        //        ContentIdentifier = AppConstants.AppGuid,
        //        ContentType = MarketplaceContentType.Applications
        //    };

        //    marketplaceDetailTask.Show();
        //}

        #endregion
    }
}
