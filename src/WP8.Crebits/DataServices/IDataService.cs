
namespace WP8.Crebits.DataServices
{
    using System;
    using System.Collections.Generic;

    using WP8.Crebits.Entities;

    public interface IDataService : IDisposable
    {
        IEnumerable<Category> GetAllCategories();

        IEnumerable<Credit> GetAllCredits(bool withEnabledOnly = true);

        IEnumerable<Debit> GetAllDebits(bool withEnabledOnly = true);

        void InsertCategory(Category item);

        void PersistCredit(Credit item);

        void PersistDebit(Debit item);

        void DeleteCategory(Category item);

        void DeleteCredit(Credit item);

        void DeleteDebit(Debit item);

        void PurgeCrebits();

        void PurgeDebits();
    }
}
