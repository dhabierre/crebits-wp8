
namespace WP8.Crebits.Entities
{
    using System;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;

    [Table]
    public class Debit : WP8.Toolkit.Entities.Entity, IOperation
    {
        #region [ Constructor ]

        public Debit()
        {
            this.Date = DateTime.Today;
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

        #region [ IdCategory ]

        private int? _idCategory;

        [Column(DbType = "INT NULL", CanBeNull = true)]
        public int? IdCatgory
        {
            get { return _idCategory; }
            set { base.SetProperty(ref _idCategory, value); }
        }

        #endregion

        #region [ Date ]

        private DateTime _date;

        [Column(CanBeNull = false)]
        public DateTime Date
        {
            get { return _date; }
            set { base.SetProperty(ref _date, value); }
        }

        #endregion

        #region [ Caption ]

        private string _caption;

        [Column(DbType = "NVARCHAR(48) NOT NULL", CanBeNull = false)]
        public string Caption
        {
            get { return _caption; }
            set { base.SetProperty(ref _caption, value); }
        }

        #endregion

        #region [ OverrideValue ]

        private double? _overrideValue;

        [Column]
        public double? OverrideValue
        {
            get { return _overrideValue; }
            set { base.SetProperty(ref _overrideValue, value); }
        }

        #endregion

        #region [ Value ]

        private double _value;

        [Column]
        public double Value
        {
            get { return _value; }
            set { base.SetProperty(ref _value, value); }
        }

        #endregion

        #region [ IsMonthly ]

        private bool _isMonthly;

        [Column]
        public bool IsMonthly
        {
            get { return _isMonthly; }
            set { base.SetProperty(ref _isMonthly, value); }
        }

        #endregion

        #region [ IsDisabled ]

        private bool _isDisabled;

        [Column]
        public bool IsDisabled
        {
            get { return _isDisabled; }
            set { base.SetProperty(ref _isDisabled, value); }
        }

        #endregion

        #region [ Version ]
#pragma warning disable 0169

        [Column(IsVersion = true)]
        private Binary Version;

#pragma warning restore 0169
        #endregion

        #endregion

        #region [ Methods ]

        public void MapFrom(Debit item)
        {
            this.Id = item.Id;
            this.IdCatgory = item.IdCatgory;

            this.Date = item.Date;
            this.Caption = item.Caption;
            this.Value = item.Value;
            this.OverrideValue = item.OverrideValue;
            this.IsMonthly = item.IsMonthly;
            this.IsDisabled = item.IsDisabled;
        }

        public void Prepare()
        {
            if (!this.IsMonthly)
            {
                this.OverrideValue = null;
            }
        }

        #endregion

        public double CurrentValue
        {
            get
            {
                return (this.OverrideValue != null && this.OverrideValue > 0)
                    ? this.OverrideValue.Value
                    : this.Value;
            }
        }
    }
}