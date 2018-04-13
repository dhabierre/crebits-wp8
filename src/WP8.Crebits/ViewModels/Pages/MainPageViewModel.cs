
namespace WP8.Crebits.ViewModels
{
    using System.Collections.Generic;

    using WP8.Crebits.DataServices;
    using WP8.Crebits.Entities;

    public class MainPageViewModel : WP8.Toolkit.ViewModels.PageViewModel
    {
        #region [ Members ]

        private IEnumerable<Category> _categories = null;
        private IEnumerable<Credit> _credits = null;
        private IEnumerable<Debit> _debits = null;

        #endregion

        #region [ Constructors ]

        public MainPageViewModel()
        {
        }

        #endregion

        #region [ ViewModels ]

        #region [ SummaryViewModel ]

        private SummaryViewModel _summaryViewModel;
        public SummaryViewModel SummaryViewModel
        {
            get
            {
                if (_summaryViewModel == null) this.SummaryViewModel = new SummaryViewModel();
                return _summaryViewModel;
            }
            set { base.SetProperty(ref _summaryViewModel, value); }
        }

        #endregion

        #region [ StatsViewModel ]

        private StatsViewModel _statsViewModel;
        public StatsViewModel StatsViewModel
        {
            get
            {
                if (_statsViewModel == null) this.StatsViewModel = new StatsViewModel();
                return _statsViewModel;
            }
            set { base.SetProperty(ref _statsViewModel, value); }
        }

        #endregion

        #region [ CreditsViewModel ]

        private CreditsViewModel _creditsViewModel;
        public CreditsViewModel CreditsViewModel
        {
            get
            {
                if (_creditsViewModel == null) this.CreditsViewModel = new CreditsViewModel();
                return _creditsViewModel;
            }
            set { base.SetProperty(ref _creditsViewModel, value); }
        }

        #endregion

        #region [ DebitsViewModel ]

        private DebitsViewModel _debitsViewModel;
        public DebitsViewModel DebitsViewModel
        {
            get
            {
                if (_debitsViewModel == null) this.DebitsViewModel = new DebitsViewModel();
                return _debitsViewModel;
            }
            set { base.SetProperty(ref _debitsViewModel, value); }
        }

        #endregion

        #endregion

        #region [ Properties ]

        #region [ IsDataLoaded ]

        public bool IsDataLoaded { get; private set; }

        #endregion

        #endregion

        #region [ Methods ]

        public void LoadData()
        {
            using (var dataService = new DataService())
            {
                _categories = dataService.GetAllCategories();

                _credits = dataService.GetAllCredits(withEnabledOnly: false);
                _debits = dataService.GetAllDebits(withEnabledOnly: false);

                this.UpdateCredits();
                this.UpdateDebits();

                this.UpdateSummary();
                this.UpdateStats();

                this.IsDataLoaded = true;
            }
        }

        private void UpdateStats()
        {
            this.StatsViewModel.Update(_categories, _debits);
        }

        private void UpdateSummary()
        {
            this.SummaryViewModel.Update(_credits, _debits);

            App.UpdateTile(this.SummaryViewModel.Cash, this.SummaryViewModel.HasData);
        }

        private void UpdateCredits()
        {
            this.CreditsViewModel.Update(_credits);
        }

        private void UpdateDebits()
        {
            this.DebitsViewModel.Update(_debits);
        }

        #endregion
    }
}