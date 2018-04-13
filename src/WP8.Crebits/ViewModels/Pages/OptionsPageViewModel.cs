
namespace WP8.Crebits.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using WP8.Crebits.DataServices;
    using WP8.Crebits.Helpers;
    using WP8.Toolkit;

    public class OptionsPageViewModel : WP8.Toolkit.ViewModels.PageViewModel
    {
        #region [ Constructor ]

        public OptionsPageViewModel()
        {
            #region [ Register Commands ]

            this.PurgeCommand = new DelegateCommand(this.Purge, this.CanExecutePurge);
            base.RegisterCommand(this.PurgeCommand);

            #endregion

            this.LoadData();

            this.SetMinCashLimitColors();
            this.SetSelectedMinCashLimitColor();
        }

        #endregion

        #region [ Properties ]

        #region [ IsLightTheme ]

        private bool _isLightTheme;

        public bool IsLightTheme
        {
            get { return _isLightTheme; }
            set { base.SetProperty(ref _isLightTheme, value); }
        }

        #endregion

        #region [ WithLiveTileDecimalDigits ]

        private bool _withLiveTileDecimalDigits;

        public bool WithLiveTileDecimalDigits
        {
            get { return _withLiveTileDecimalDigits; }
            set { base.SetProperty(ref _withLiveTileDecimalDigits, value); }
        }

        #endregion

        #region [ ExportDataEmail ]

        private string _exportDataEmail;

        public string ExportDataEmail
        {
            get { return _exportDataEmail; }
            set { base.SetProperty(ref _exportDataEmail, value); }
        }

        #endregion

        #region [ MinCashLimitValue ]

        private int? _minCashLimitValue;

        public int? MinCashLimitValue
        {
            get { return _minCashLimitValue; }
            set { base.SetProperty(ref _minCashLimitValue, value); }
        }

        #endregion

        #region [ MinCashLimitColors ]

        private IList<string> _minCashLimitColors;

        public IList<string> MinCashLimitColors
        {
            get { return _minCashLimitColors; }
            set { base.SetProperty(ref _minCashLimitColors, value); }
        }

        #endregion

        #region [ MinCashLimitColor ]

        private string _selectedMinCashLimitColor;

        public string SelectedMinCashLimitColor
        {
            get { return _selectedMinCashLimitColor; }
            set { base.SetProperty(ref _selectedMinCashLimitColor, value); }
        }

        #endregion

        #endregion

        #region [ Methods ]

        private void LoadData()
        {
            this.IsLightTheme = SettingsHelper.GetIsLightTheme();
            this.WithLiveTileDecimalDigits = SettingsHelper.GetWithLiveTileDecimalDigits();
            this.ExportDataEmail = SettingsHelper.GetExportDataEmail();
            this.MinCashLimitValue = SettingsHelper.GetMinCashLimitValue();
            this.SelectedMinCashLimitColor = SettingsHelper.GetMinCashLimitColor();
        }

        private void SetMinCashLimitColors()
        {
            this.MinCashLimitColors = new List<string> { "Orange" /* <- default value*/, "Red", "Yellow" };
        }

        private void SetSelectedMinCashLimitColor()
        {
            this.SelectedMinCashLimitColor = this.MinCashLimitColors.FirstOrDefault(c =>
                c == SettingsHelper.GetMinCashLimitColor());
        }

        #endregion

        #region [ Events ]

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            bool bUpdateTile = false;

            if (e.PropertyName == "IsLightTheme")
            {
                SettingsHelper.SetIsLightTheme(this.IsLightTheme);
            }
            else if (e.PropertyName == "WithLiveTileDecimalDigits")
            {
                bUpdateTile = true;
                SettingsHelper.SetWithLiveTileDecimalDigits(this.WithLiveTileDecimalDigits);
            }
            else if (e.PropertyName == "ExportDataEmail")
            {
                SettingsHelper.SetExportDataEmail(this.ExportDataEmail);
            }
            else if (e.PropertyName == "MinCashLimitValue")
            {
                bUpdateTile = true;
                SettingsHelper.SetMinCashLimitValue(this.MinCashLimitValue);
            }
            else if (e.PropertyName == "SelectedMinCashLimitColor")
            {
                bUpdateTile = true;
                SettingsHelper.SetMinCashLimitColor(this.SelectedMinCashLimitColor);
            }

            if (bUpdateTile)
            {
                App.UpdateTile();
            }
        }

        #endregion

        #region [ Commands ]

        #region [ PurgeCommand ]

        public DelegateCommand PurgeCommand { get; set; }

        private bool CanExecutePurge(object parameter)
        {
            return true;
        }

        private void Purge(object parameter)
        {
            using (var dataService = new DataService())
            {
                dataService.PurgeCrebits();
                dataService.PurgeDebits();
            }
        }

        public bool Purge()
        {
            if (this.PurgeCommand.CanExecute(null))
            {
                this.PurgeCommand.Execute(null);
                return true;
            }

            return false;
        }

        #endregion

        #endregion
    }
}