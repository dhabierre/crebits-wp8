
namespace WP8.Crebits.ViewModels
{
    using Microsoft.Phone.Shell;

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;

    using WP8.Crebits.DataServices;
    using WP8.Crebits.Enums;
    using WP8.Crebits.Entities;
    using WP8.Crebits.Managers;
    using WP8.Crebits.Resources;
    using WP8.Toolkit;

    public class OperationPageViewModel : WP8.Toolkit.ViewModels.PageViewModel
    {
        #region [ Constructor ]

        public OperationPageViewModel()
        {
            #region [ Register Commands ]

            this.PersistCommand = new DelegateCommand(this.Persist, this.CanExecutePersist);
            base.RegisterCommand(this.PersistCommand);

            #endregion

            this.LoadCategories(); // before init blocks!

            this.IsDebit = true;
            this.CanChangeOperationType = true;

            this.IsMonthly = false;
            this.Date = DateTime.Today;

            #region [ Init by OperationType ]

            if (PhoneApplicationService.Current.State.ContainsKey("OperationType"))
            {
                var op = (OperationEnum)PhoneApplicationService.Current.State["OperationType"];
                if (op == OperationEnum.Credit)
                {
                    this.IsCredit = true;
                }

                PhoneApplicationService.Current.State.Remove("OperationType");
            }

            #endregion

            #region [ Init by OperationItem ]

            if (PhoneApplicationService.Current.State.ContainsKey("OperationItem"))
            {
                var item = PhoneApplicationService.Current.State["OperationItem"] as IOperation;
                if (item != null)
                {
                    this.CanChangeOperationType = false;

                    this.Id = item.Id;

                    if (item is Credit)
                    {
                        this.IsCredit = true;
                    }

                    if (item is Debit)
                    {
                        this.IsDebit = true;
                        this.SelectedCategory = this.Categories.FirstOrDefault(i => i.Id == ((Debit)item).IdCatgory);
                    }

                    this.Date = item.Date;
                    this.Caption = item.Caption;
                    this.Value = item.Value;
                    this.OverrideValue = item.OverrideValue;
                    this.IsMonthly = item.IsMonthly;
                    this.IsDisabled = item.IsDisabled;
                }

                PhoneApplicationService.Current.State.Remove("OperationItem");
            }

            #endregion
        }

        #endregion

        #region [ Properties ]

        #region [ Id ]

        private int _id;

        public int Id
        {
            get { return _id; }
            set { base.SetProperty(ref _id, value); }
        }

        #endregion

        #region [ Date ]

        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set { base.SetProperty(ref _date, value); }
        }

        #endregion

        #region [ Caption ]

        private string _caption;

        public string Caption
        {
            get { return _caption; }
            set { base.SetProperty(ref _caption, value); }
        }

        #endregion

        #region [ Value ]

        private double? _value;

        public double? Value
        {
            get { return _value; }
            set { base.SetProperty(ref _value, value); }
        }

        #endregion

        #region [ OverrideValue ]

        private double? _overrideValue;

        public double? OverrideValue
        {
            get { return _overrideValue; }
            set { base.SetProperty(ref _overrideValue, value); }
        }

        #endregion

        #region [ IsMonthly ]

        private bool _isMonthly;

        public bool IsMonthly
        {
            get { return _isMonthly; }
            set { base.SetProperty(ref _isMonthly, value); }
        }

        #endregion

        #region [ IsDisabled ]

        private bool _isDisabled;

        public bool IsDisabled
        {
            get { return _isDisabled; }
            set { base.SetProperty(ref _isDisabled, value); }
        }

        #endregion

        #region [ IsCredit ]

        private bool _isCredit;

        public bool IsCredit
        {
            get { return _isCredit; }
            set { base.SetProperty(ref _isCredit, value); }
        }

        #endregion

        #region [ IsDebit ]

        private bool _isDebit;

        public bool IsDebit
        {
            get { return _isDebit; }
            set { base.SetProperty(ref _isDebit, value); }
        }

        #endregion

        #region [ SelectedCategory ]

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { base.SetProperty(ref _selectedCategory, value); }
        }

        #endregion

        #region [ Categories ]

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get
            {
                if (_categories == null) this.Categories = new ObservableCollection<Category>();
                return _categories;
            }
            set { base.SetProperty(ref _categories, value); }
        }

        #endregion

        #endregion

        #region [ Technicals ]

        #region [ CanChangeOperationType ]

        private bool _canChangeOperationType;

        public bool CanChangeOperationType
        {
            get { return _canChangeOperationType; }
            set { base.SetProperty(ref _canChangeOperationType, value); }
        }

        #endregion

        #endregion

        #region [ Methods ]

        private void LoadCategories()
        {
            using (var dataService = new DataService())
            {
                var categories = new List<Category>(dataService.GetAllCategories());
                categories.Insert(0, new Category { Id = 0, Caption = AppResources.NoCategory });

                this.Categories = new ObservableCollection<Category>(categories);
                if (this.Categories.Any())
                {
                    this.SelectedCategory = this.Categories[0]; // to set the selected defaultvalue (no category)
                }
            }
        }

        #endregion

        #region [ Events ]

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsCredit")
            {
                if (this.IsCredit)
                {
                    this.IsDebit = false;
                }
            }

            if (e.PropertyName == "IsDebit")
            {
                if (this.IsDebit)
                {
                    this.IsCredit = false;
                }
            }
        }

        #endregion

        #region [ Commands ]

        #region [ PersistCommand ]

        public DelegateCommand PersistCommand { get; set; }

        private bool CanExecutePersist(object parameter)
        {
            if (this.IsDebit == this.IsCredit)
            {
                return false;
            }

            return !string.IsNullOrEmpty(this.Caption) && this.Value != null && this.Value > 0;
        }

        private void Persist(object parameter)
        {
            using (var dataService = new DataService())
            {
                if (this.IsCredit)
                {
                    var credit = new Credit
                    {
                        Id = this.Id,
                        Caption = this.Caption,
                        Date = this.Date,
                        Value = this.Value.Value,
                        OverrideValue = this.OverrideValue,
                        IsMonthly = this.IsMonthly,
                        IsDisabled = this.IsDisabled
                    };

                    dataService.PersistCredit(credit);
                }

                if (this.IsDebit)
                {
                    var debit = new Debit
                    {
                        Id = this.Id,
                        Caption = this.Caption,
                        Date = this.Date,
                        Value = this.Value.Value,
                        OverrideValue = this.OverrideValue,
                        IsMonthly = this.IsMonthly,
                        IsDisabled = this.IsDisabled
                    };

                    if (this.SelectedCategory != null && this.SelectedCategory.Id > 0)
                    {
                        debit.IdCatgory = this.SelectedCategory.Id;
                    }

                    dataService.PersistDebit(debit);
                }
            }

            RaiseEventsManager.RaiseRefreshMainScreen();
        }

        public bool Persist()
        {
            if (this.PersistCommand.CanExecute(null))
            {
                this.PersistCommand.Execute(null);
                return true;
            }

            return false;
        }

        #endregion

        #endregion

        #region [ PageViewModel Overrides ]

        public override string PageTitle
        {
            get
            {
                return WP8.Crebits.Resources.AppResources.Operation;
            }
        }

        #endregion
    }
}