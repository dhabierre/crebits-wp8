
namespace WP8.Crebits.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using WP8.Crebits.Entities;

    public class SummaryViewModel : WP8.Toolkit.ViewModels.ViewModel
    {
        #region [ Constructor ]

        public SummaryViewModel()
        {
        }

        #endregion

        #region [ Properties ]

        #region [ TotalCredits ]

        private double _totalCredits;

        public double TotalCredits
        {
            get { return _totalCredits; }
            set { base.SetProperty(ref _totalCredits, value); }
        }

        #endregion

        #region [ TotalMonthlyCredits ]

        private double _totalMonthlyCredits;

        public double TotalMonthlyCredits
        {
            get { return _totalMonthlyCredits; }
            set { base.SetProperty(ref _totalMonthlyCredits, value); }
        }

        #endregion

        #region [ TotalNotMonthlyCredits ]

        private double _totalNotMonthlyCredits;

        public double TotalNotMonthlyCredits
        {
            get { return _totalNotMonthlyCredits; }
            set { base.SetProperty(ref _totalNotMonthlyCredits, value); }
        }

        #endregion

        #region [ TotalDebits ]

        private double _totalDebits;

        public double TotalDebits
        {
            get { return _totalDebits; }
            set { base.SetProperty(ref _totalDebits, value); }
        }

        #endregion

        #region [ TotalMonthlyDebits ]

        private double _totalMonthlyDebits;

        public double TotalMonthlyDebits
        {
            get { return _totalMonthlyDebits; }
            set { base.SetProperty(ref _totalMonthlyDebits, value); }
        }

        #endregion

        #region [ TotalNotMonthlyDebits ]

        private double _totalNotMonthlyDebits;

        public double TotalNotMonthlyDebits
        {
            get { return _totalNotMonthlyDebits; }
            set { base.SetProperty(ref _totalNotMonthlyDebits, value); }
        }

        #endregion

        #region [ HasData ]

        public bool HasData
        {
            get { return this.TotalCredits != 0 || this.TotalDebits != 0; }
        }

        #endregion

        #region [ Cash ]

        private double _cash;

        public double Cash
        {
            get { return Math.Round(_cash, 2); }
            set { base.SetProperty(ref _cash, value); }
        }

        #endregion

        #region [ CashPercentLeft ]

        private double _cashPercentLeft;

        public double CashPercentLeft
        {
            get { return _cashPercentLeft; }
            set { base.SetProperty(ref _cashPercentLeft, value); }
        }

        #endregion

        #endregion

        #region [ Methods ]

        public void Update(IEnumerable<Credit> credits, IEnumerable<Debit> debits)
        {
            this.TotalCredits = credits.Where(c => !c.IsDisabled).Sum(i => i.CurrentValue);
            this.TotalMonthlyCredits = credits.Where(c => !c.IsDisabled).Where(i => i.IsMonthly).Sum(i => i.CurrentValue);
            this.TotalNotMonthlyCredits = this.TotalCredits - this.TotalMonthlyCredits;

            this.TotalDebits = debits.Where(d => !d.IsDisabled).Sum(i => i.CurrentValue);
            this.TotalMonthlyDebits = debits.Where(d => !d.IsDisabled).Where(i => i.IsMonthly).Sum(i => i.CurrentValue);
            this.TotalNotMonthlyDebits = this.TotalDebits - this.TotalMonthlyDebits;

            this.Cash = this.TotalCredits - this.TotalDebits;
            this.CashPercentLeft = (this.TotalCredits > 0) ? Math.Round(this.Cash * 100 / this.TotalCredits, 1) : 0;
        }

        #endregion
    }
}