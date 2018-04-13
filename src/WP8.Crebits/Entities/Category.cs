
namespace WP8.Crebits.Entities
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Linq;

    [Table]
    public class Category : WP8.Toolkit.Entities.Entity
    {
        #region [ Constructor ]

        public Category()
        {
        }

        #endregion

        #region [ Properties ]

        #region [ Id ]

        private int _id;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int Id
        {
            get { return _id; }
            set { base.SetProperty(ref _id, value); }
        }

        #endregion

        #region [ Caption ]

        private string _caption;

        [Column(DbType = "NVARCHAR(20) NOT NULL", CanBeNull = false)]
        public string Caption
        {
            get { return _caption; }
            set { base.SetProperty(ref _caption, value); }
        }

        #endregion

        #region [ TotalValue ]

        private double _totalValue;

        public double TotalValue
        {
            get { return _totalValue; }
            set { base.SetProperty(ref _totalValue, value); }
        }

        #endregion

        #region [ PercentValue ]

        private double _percentValue;

        public double PercentValue
        {
            get { return _percentValue; }
            set { base.SetProperty(ref _percentValue, value); }
        }

        #endregion

        #region [ Operations ]

        private ObservableCollection<IOperation> _operations;

        public ObservableCollection<IOperation> Operations
        {
            get { return _operations; }
            set { base.SetProperty(ref _operations, value); }
        }

        #endregion

        #region [ Version ]
#pragma warning disable 0169

        [Column(IsVersion = true)]
        private Binary Version;

#pragma warning restore 0169
        #endregion

        #region [ PercentCaption ]

        private string _percentCaption;

        public string PercentCaption
        {
            get { return _percentCaption; }
            set { base.SetProperty(ref _percentCaption, value); }
        }

        #endregion

        #endregion

        #region [ Events ]

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Operations")
            {
                this.TotalValue = (this.Operations != null) ? this.Operations.Sum(i => i.CurrentValue) : 0;
            }
        }

        #endregion
    }

    public static class CategoryExtensions
    {
        public static double GetPercentageDistribution(this Category category, double debitsSum, int round = 2)
        {
            if (debitsSum == 0)
            {
                return 0;
            }

            return Math.Round(category.TotalValue * 100 / debitsSum, round);
        }
    }
}