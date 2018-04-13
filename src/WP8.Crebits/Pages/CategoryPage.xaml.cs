
namespace WP8.Crebits.Pages
{
    using Microsoft.Phone.Shell;

    using System;
    using System.Windows;
    using System.Windows.Controls;

    using WP8.Crebits.Entities;
    using WP8.Crebits.Helpers;
    using WP8.Crebits.Managers;
    using WP8.Crebits.Resources;
    using WP8.Crebits.ViewModels;
    using WP8.Toolkit.Helpers;

    public partial class CategoryPage : Microsoft.Phone.Controls.PhoneApplicationPage
    {
        #region [ Uri ]

        public static readonly string Uri = "/Pages/CategoryPage.xaml";

        #endregion

        #region [ ViewModel ]

        private CategoryPageViewModel ViewModel
        {
            get { return this.DataContext as CategoryPageViewModel; }
        }

        #endregion

        #region [ Constructors ]

        public CategoryPage()
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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var category = button.DataContext as Category;
                if (category != null)
                {
                    VibrationDeviceHelper.Vibrate(TimeSpan.FromMilliseconds(150));

                    var caption = AppResources.ContinueQuestion;
                    var message = string.Format(AppResources.DeleteCategoryMessage, category.Caption);

                    if (MessageBox.Show(message, caption, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        if (this.ViewModel.Delete(category))
                        {
                            RaiseEventsManager.RaiseRefreshMainScreen();
                        }
                    }
                }
            }

            this.Focus();
        }

        private void AppBarCheckButton_Click(object sender, EventArgs e)
        {
            this.ViewModel.Save();
        }

        private void AppBarCancelButton_Click(object sender, EventArgs e)
        {
            if (base.NavigationService.CanGoBack)
                base.NavigationService.GoBack();
        }

        #endregion
    }
}
