
namespace WP8.Crebits.DataServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using WP8.Crebits.DataContexts;
    using WP8.Crebits.Entities;

    public class DataService : IDataService
    {
        #region [ Members ]

        private readonly AppDataContext _context = null;

        #endregion

        #region [ Constructor ]

        public DataService()
        {
            _context = new AppDataContext();
        }

        ~DataService()
        {
            this.Dispose(false);
        }

        #endregion

        #region [ IDataService Implementation ]

        public IEnumerable<Category> GetAllCategories()
        {
            var query = from item in _context.Categories
                        orderby item.Caption
                        select item;

            return query.AsEnumerable();
        }

        public IEnumerable<Credit> GetAllCredits(bool withEnabledOnly = true)
        {
            return _context.Credits.Where(i => withEnabledOnly && !i.IsDisabled || !withEnabledOnly)
                                   .OrderByDescending(j => j.Value);
        }

        public IEnumerable<Debit> GetAllDebits(bool withEnabledOnly = true)
        {
            return _context.Debits.Where(i => withEnabledOnly && !i.IsDisabled || !withEnabledOnly)
                                  .OrderByDescending(j => j.Value)
                                  .ThenByDescending(k => k.Date)
                                  .ThenBy(l => l.Caption);

        }

        public void InsertCategory(Category item)
        {
            if (item != null)
            {
                _context.Categories.InsertOnSubmit(item);
                _context.SubmitChanges();
            }
        }

        public void PersistCredit(Credit item)
        {
            if (item != null)
            {
                item.Prepare();

                Credit credit = null;

                if (item.Id > 0)
                {
                    credit = _context.Credits.FirstOrDefault(i => i.Id == item.Id);
                    if (credit != null)
                    {
                        credit.MapFrom(item);
                    }
                }

                if (credit == null)
                {
                    _context.Credits.InsertOnSubmit(item);
                }

                _context.SubmitChanges();
            }
        }

        public void PersistDebit(Debit item)
        {
            if (item != null)
            {
                item.Prepare();

                Debit debit = null;

                if (item.Id > 0)
                {
                    debit = _context.Debits.FirstOrDefault(i => i.Id == item.Id);
                    if (debit != null)
                    {
                        debit.MapFrom(item);
                    }
                }

                if (debit == null)
                {
                    _context.Debits.InsertOnSubmit(item);
                }

                _context.SubmitChanges();
            }
        }

        public void DeleteCategory(Category item)
        {
            if (item != null)
            {
                var record = _context.Categories.FirstOrDefault(i => i.Id == item.Id);
                if (record != null)
                {
                    foreach (var debit in _context.Debits.Where(i => i.IdCatgory == item.Id))
                    {
                        debit.IdCatgory = null; // reinit
                    }

                    _context.Categories.DeleteOnSubmit(record);
                    _context.SubmitChanges();
                }
            }
        }

        public void DeleteCredit(Credit item)
        {
            if (item != null)
            {
                var record = _context.Credits.FirstOrDefault(i => i.Id == item.Id);
                if (record != null)
                {
                    _context.Credits.DeleteOnSubmit(record);
                    _context.SubmitChanges();
                }
            }
        }

        public void DeleteDebit(Debit item)
        {
            if (item != null)
            {
                var record = _context.Debits.FirstOrDefault(i => i.Id == item.Id);
                if (record != null)
                {
                    _context.Debits.DeleteOnSubmit(record);
                    _context.SubmitChanges();
                }
            }
        }

        public void PurgeCrebits()
        {
            var query = from item in _context.Credits
                        where item.IsMonthly
                        select item;

            foreach (var item in query)
            {
                item.OverrideValue = null;
            }

            query = from item in _context.Credits
                    where !item.IsMonthly
                    select item;

            _context.Credits.DeleteAllOnSubmit(query);
            _context.SubmitChanges();
        }

        public void PurgeDebits()
        {
            var query = from item in _context.Debits
                        where item.IsMonthly
                        select item;

            foreach (var item in query)
            {
                item.OverrideValue = null;
            }

            query = from item in _context.Debits
                    where !item.IsMonthly
                    select item;

            _context.Debits.DeleteAllOnSubmit(query);
            _context.SubmitChanges();
        }

        #endregion

        #region [ IDisposable ]

        private bool _isDisposed = false;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_isDisposed)
                {
                    _context.Dispose();
                }

                _isDisposed = true;
            }
        }

        #endregion
    }
}
