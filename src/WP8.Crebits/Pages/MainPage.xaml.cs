
namespace WP8.Crebits.Pages
{
    using Microsoft.Phone.Shell;
    using Microsoft.Phone.Tasks;

    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;

    using WP8.Crebits.Entities;
    using WP8.Crebits.Enums;
    using WP8.Crebits.Helpers;
    using WP8.Crebits.Managers;
    using WP8.Crebits.Resources;
    using WP8.Crebits.ViewModels;
    using WP8.Toolkit.Helpers;
    using WP8.Toolkit.Managers;

    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage
    {
        #region [ Uri ]

        public static readonly string Uri = "/Pages/MainPage.xaml";

        #endregion

        #region [ ViewModel ]

        public MainPageViewModel ViewModel
        {
            get { return this.DataContext as MainPageViewModel; }
        }

        #endregion

        #region [ Constructor ]

        public MainPage()
        {
            this.InitializeComponent();
            this.BuildLocalizedApplicationBar();

            RaiseEventsManager.SetMainViewModel(this.ViewModel); // important!

            this.Loaded += this.MainPage_Loaded;
        }

        #endregion

        #region [ Methods ]

        private void BuildLocalizedApplicationBar()
        {
            this.ApplicationBar = ApplicationBarHelper.GetApplicationBar();

            var addAppBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/Add.png", UriKind.Relative));

            addAppBarButton.Text = AppResources.Add;
            addAppBarButton.Click += this.AddAppBarButton_Click;

            this.ApplicationBar.Buttons.Add(addAppBarButton);

            var optionsAppBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/Options.png", UriKind.Relative));

            optionsAppBarButton.Text = AppResources.Options;
            optionsAppBarButton.Click += this.OptionsAppBarButton_Click;

            this.ApplicationBar.Buttons.Add(optionsAppBarButton);

            //ApplicationBarMenuItem categoryMenuItem = new ApplicationBarMenuItem(AppResources.Category);
            //categoryMenuItem.Click += this.CategoryMenuItem_Click;

            //this.ApplicationBar.MenuItems.Add(categoryMenuItem);

            var exportMenuItem = new ApplicationBarMenuItem(AppResources.ExportData);
            exportMenuItem.Click += this.ExportMenuItem_Click;

            this.ApplicationBar.MenuItems.Add(exportMenuItem);

            var notesMenuItem = new ApplicationBarMenuItem(AppResources.Notes);
            notesMenuItem.Click += this.NotesMenuItem_Click;

            this.ApplicationBar.MenuItems.Add(notesMenuItem);

            var aboutMenuItem = new ApplicationBarMenuItem(AppResources.About);
            aboutMenuItem.Click += this.AboutMenuItem_Click;

            this.ApplicationBar.MenuItems.Add(aboutMenuItem);
        }

        private static bool feebackPopupDisplayed = false;

        private static void ShowFeebackPopupIfNeeded()
        {
            if (!feebackPopupDisplayed)
            {
                feebackPopupDisplayed = true;

                if (!SettingsHelper.GetHasFeedback())
                {
                    var executionCounter = SettingsHelper.GetExecutionCounter();
                    if (executionCounter % 30 == 0)
                    {
                        if (executionCounter > 999999)
                        {
                            SettingsHelper.SetExecutionCounter(0);
                        }

                        var caption = AppResources.ContinueQuestion;
                        var message = AppResources.FeedbackExplanation;

                        if (MessageBox.Show(message, caption, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            new MarketplaceReviewTask().Show();
                        }
                    }
                }
            }
        }

        #endregion

        #region [ Events ]

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ErrorReportManager.CheckForPreviousException(
                AppResources.ErrorReportMessage,
                AppResources.ErrorReportCaption,
                AppResources.AuthorEmail);

            ShowFeebackPopupIfNeeded();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!this.ViewModel.IsDataLoaded)
                this.ViewModel.LoadData();
        }

        private void AddAppBarButton_Click(object sender, EventArgs e)
        {
            var pivotType = (PivotTypes)this.Pivot.SelectedIndex;
            if (pivotType == PivotTypes.Categories)
            {
                base.NavigationService.Navigate(new Uri(CategoryPage.Uri, UriKind.Relative));
            }
            else
            {
                PhoneApplicationService.Current.State["OperationType"] = (pivotType == PivotTypes.Credits)
                    ? OperationEnum.Credit  // 2
                    : OperationEnum.Debit;  // 1

                base.NavigationService.Navigate(new Uri(OperationPage.Uri, UriKind.Relative));
            }
        }

        private void OptionsAppBarButton_Click(object sender, EventArgs e)
        {
            base.NavigationService.Navigate(new Uri(OptionsPage.Uri, UriKind.Relative));
        }

        //private void CategoryMenuItem_Click(object sender, EventArgs e)
        //{
        //    base.NavigationService.Navigate(new Uri(CategoryPage.Uri, UriKind.Relative));
        //}

        private void NotesMenuItem_Click(object sender, EventArgs e)
        {
            base.NavigationService.Navigate(new Uri(NotesPage.Uri, UriKind.Relative));
        }

        private void ExportMenuItem_Click(object sender, EventArgs e)
        {
            ExportDataManager.ShowExportDataEmailComposeTask();
        }

        private void PurchaseMenuItem_Click(object sender, EventArgs e)
        {
            TaskHelper.OpenMarketplace();
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            base.NavigationService.Navigate(new Uri(AboutPage.Uri, UriKind.Relative));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var op = button.DataContext as IOperation;
                if (op != null)
                {
                    PhoneApplicationService.Current.State["OperationItem"] = op;
                    base.NavigationService.Navigate(new Uri(OperationPage.Uri, UriKind.Relative));
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var op = button.DataContext as IOperation;
                if (op != null)
                {
                    VibrationDeviceHelper.Vibrate(TimeSpan.FromMilliseconds(150));

                    string caption = AppResources.ContinueQuestion;
                    string message = string.Format(AppResources.DeleteOperationMessage, op.Caption, op.Value);

                    if (MessageBox.Show(message, caption, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        string tag = button.Tag as string;

                        if (tag == "Credit")
                        {
                            this.ViewModel.CreditsViewModel.Delete((Credit)op);
                        }

                        if (tag == "Debit")
                        {
                            this.ViewModel.DebitsViewModel.Delete((Debit)op);
                        }

                        this.ViewModel.LoadData();
                    }
                }
            }

            this.Focus();
        }

        private bool _showGraph;

        private void SwithStatsButton_Click(object sender, RoutedEventArgs e)
        {
            _showGraph = !_showGraph;

            this.PieChart.Visibility = (_showGraph) ? Visibility.Visible : Visibility.Collapsed;
            this.Amounts.Visibility = (_showGraph) ? Visibility.Collapsed : Visibility.Visible;

            this.SwithStatsButton.Content = (_showGraph) ? AppResources.ShowAmounts : AppResources.ShowPieGraph;
        }

        #endregion
    }
}