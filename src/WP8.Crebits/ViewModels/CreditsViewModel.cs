
namespace WP8.Crebits.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using WP8.Crebits.DataServices;
    using WP8.Crebits.Entities;
    using WP8.Toolkit;

    public class CreditsViewModel : WP8.Toolkit.ViewModels.ViewModel
    {
        #region [ Constructor ]

        public CreditsViewModel()
        {
            #region [ Register Commands ]

            this.DeleteCommand = new DelegateCommand(this.Delete, this.CanExecuteDelete);
            base.RegisterCommand(DeleteCommand);

            #endregion
        }

        #endregion

        #region [ Properties ]

        #region [ Credits ]

        private ObservableCollection<Credit> _credits;
        public ObservableCollection<Credit> Credits
        {
            get
            {
                if (_credits == null) this.Credits = new ObservableCollection<Credit>();
                return _credits;
            }
            set { base.SetProperty(ref _credits, value); }
        }

        #endregion

        #region [ HasNoData ]

        private bool _hasNoData;

        public bool HasNoData
        {
            get { return _hasNoData; }
            set { base.SetProperty(ref _hasNoData, value); }
        }

        #endregion

        #endregion

        #region [ Methods ]

        public void Update(IEnumerable<Credit> credits)
        {
            this.Credits = new ObservableCollection<Credit>(credits.OrderByDescending(i => i.CurrentValue));
            this.HasNoData = this.Credits.Count == 0;
        }

        #endregion

        #region [ Commands ]

        #region [ DeleteCommand ]

        public DelegateCommand DeleteCommand { get; set; }

        private bool CanExecuteDelete(object parameter)
        {
            if (parameter == null)
                return false;

            return true;
        }

        private void Delete(object parameter)
        {
            var item = parameter as Credit;
            if (item != null)
            {
                using (var dataService = new DataService())
                {
                    dataService.DeleteCredit(item);
                }
            }
        }

        public bool Delete(Credit item)
        {
            if (this.DeleteCommand.CanExecute(item))
            {
                this.DeleteCommand.Execute(item);
                return true;
            }

            return false;
        }

        #endregion

        #endregion
    }
}