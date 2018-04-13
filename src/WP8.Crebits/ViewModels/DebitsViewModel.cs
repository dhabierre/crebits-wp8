
namespace WP8.Crebits.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using WP8.Crebits.DataServices;
    using WP8.Crebits.Entities;
    using WP8.Toolkit;

    public class DebitsViewModel : WP8.Toolkit.ViewModels.ViewModel
    {
        #region [ Constructor ]

        public DebitsViewModel()
        {
            #region [ Register Commands ]

            this.DeleteCommand = new DelegateCommand(this.Delete, this.CanExecuteDelete);
            base.RegisterCommand(DeleteCommand);

            #endregion
        }

        #endregion

        #region [ Properties ]

        #region [ Debits ]

        private ObservableCollection<Debit> _debits;
        public ObservableCollection<Debit> Debits
        {
            get
            {
                if (_debits == null) this.Debits = new ObservableCollection<Debit>();
                return _debits;
            }
            set { base.SetProperty(ref _debits, value); }
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

        public void Update(IEnumerable<Debit> debits)
        {
            this.Debits = new ObservableCollection<Debit>(debits.OrderByDescending(i => i.CurrentValue));
            this.HasNoData = this.Debits.Count == 0;
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
            var item = parameter as Debit;
            if (item != null)
            {
                using (var dataService = new DataService())
                {
                    dataService.DeleteDebit(item);
                }
            }
        }

        public bool Delete(Debit item)
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