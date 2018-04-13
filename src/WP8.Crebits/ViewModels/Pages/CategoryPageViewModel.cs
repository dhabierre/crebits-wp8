
namespace WP8.Crebits.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using WP8.Crebits.DataServices;
    using WP8.Crebits.Entities;
    using WP8.Crebits.Managers;
    using WP8.Toolkit;

    public class CategoryPageViewModel : WP8.Toolkit.ViewModels.PageViewModel
    {
        #region [ Constructor ]

        public CategoryPageViewModel()
        {
            #region [ Register Commands ]

            this.SaveCommand = new DelegateCommand(this.Save, this.CanExecuteSave);
            base.RegisterCommand(this.SaveCommand);

            this.DeleteCommand = new DelegateCommand(this.Delete, this.CanExecuteDelete);
            base.RegisterCommand(this.DeleteCommand);

            #endregion

            this.LoadCategories();
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

        #region [ Caption ]

        private string _caption;

        public string Caption
        {
            get { return _caption; }
            set { base.SetProperty(ref _caption, value); }
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

        #region [ Methods ]

        private void LoadCategories()
        {
            using (var dataService = new DataService())
            {
                this.Categories = new ObservableCollection<Category>(dataService.GetAllCategories());
            }
        }

        #endregion

        #region [ Commands ]

        #region [ SaveCommand ]

        public DelegateCommand SaveCommand { get; set; }

        private bool CanExecuteSave(object parameter)
        {
            return !string.IsNullOrWhiteSpace(this.Caption) && !this.Categories.Any(i =>
                string.Compare(i.Caption, this.Caption, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public bool Save()
        {
            if (this.SaveCommand.CanExecute(null))
            {
                this.SaveCommand.Execute(null);
                return true;
            }

            return false;
        }

        private void Save(object parameter)
        {
            var category = new Category { Caption = this.Caption };

            using (var dataService = new DataService())
            {
                dataService.InsertCategory(category);
            }

            var updatedCategories = new List<Category>(this.Categories);
            updatedCategories.Insert(0, category);

            this.Categories = new ObservableCollection<Category>(updatedCategories);
            this.Caption = null;

            RaiseEventsManager.RaiseRefreshMainScreen();
        }

        #endregion

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
            Category item = parameter as Category;
            if (item != null)
            {
                using (var dataService = new DataService())
                {
                    dataService.DeleteCategory(item);
                }

                this.LoadCategories();
            }
        }

        public bool Delete(Category item)
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

        #region [ PageViewModel Overrides ]

        public override string PageTitle
        {
            get
            {
                return WP8.Crebits.Resources.AppResources.Category;
            }
        }

        #endregion
    }
}