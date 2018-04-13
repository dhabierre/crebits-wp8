
namespace WP8.Crebits.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using WP8.Crebits.Entities;

    public class StatsViewModel : WP8.Toolkit.ViewModels.ViewModel
    {
        #region [ Properties ]

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

        #region [ HasNoCategory ]

        private bool _hasNoCategory;

        public bool HasNoCategory
        {
            get { return _hasNoCategory; }
            set { base.SetProperty(ref _hasNoCategory, value); }
        }

        #endregion

        #region [ PieChartCategories ]

        private ObservableCollection<Category> _pieChartCategories;
        public ObservableCollection<Category> PieChartCategories
        {
            get
            {
                if (_pieChartCategories == null) this.PieChartCategories = new ObservableCollection<Category>();
                return _pieChartCategories;
            }
            set { base.SetProperty(ref _pieChartCategories, value); }
        }

        #endregion

        #region [ DisplayPieChart ]

        private bool _displayPieChart;

        public bool DisplayPieChart
        {
            get { return _displayPieChart; }
            set { base.SetProperty(ref _displayPieChart, value); }
        }

        #endregion

        #endregion

        #region [ Methods ]

        public void Update(IEnumerable<Category> categories, IEnumerable<Debit> debits)
        {
            //this.HasNoCategory = true;

            //if (debits == null || !debits.Any())
            //    return;

            debits = debits.Where(i => !i.IsDisabled);

            var debitsSum = debits.Sum(i => i.CurrentValue);

            #region [ Categories ]

            var list = new List<Category>(categories);

            var unknownCategory = new Category { Id = -1, Caption = "?" };

            unknownCategory.TotalValue = debits.Where(i => i.IdCatgory == null).Sum(i => i.CurrentValue);
            unknownCategory.PercentValue = unknownCategory.GetPercentageDistribution(debitsSum, 0);
            unknownCategory.PercentCaption = string.Format("{0} {1}%", unknownCategory.Caption, unknownCategory.PercentValue);

            list.Add(unknownCategory);

            var ids = debits.Where(i => i.IdCatgory != null) // used categories
                            .Select(i => i.IdCatgory.Value)
                            .Distinct();

            foreach (int id in ids)
            {
                var category = list.First(i => i.Id == id);

                category.Operations = new ObservableCollection<IOperation>(debits.Where(i => i.IdCatgory == category.Id));

                string caption = category.Caption;
                if (caption.Length > 12)
                {
                    caption = string.Concat(caption.Substring(0, 9), "...");
                }

                category.PercentValue = category.GetPercentageDistribution(debitsSum, round: 0);
                category.PercentCaption = string.Format("{0} {1}%", caption, category.PercentValue);
            }

            this.Categories = new ObservableCollection<Category>(
                list/*.Where(i => i.TotalValue > 0)*/
                    .OrderByDescending(j => j.TotalValue)
                    .ThenBy(k => k.Caption));

            //this.HasNoCategory = !this.Categories.Any();

            #endregion

            #region [ PieChartCategories ]

            this.PieChartCategories = new ObservableCollection<Category>(this.Categories.Where(i => /*i.Id != -1 &&*/ i.PercentValue >= 1));

            this.DisplayPieChart = this.Categories.Sum(c => c.TotalValue) > 0;

            #endregion
        }

        #endregion
    }
}