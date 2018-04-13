
namespace WP8.Crebits.DataContexts
{
    using System.Data.Linq;

    using WP8.Crebits.Entities;

    public class AppDataContext : DataContext
    {
        #region [ Constants ]

        private static readonly string ConnectionString = "Data Source=isostore:/Data.sdf";

        #endregion

        #region [ Constructors ]

        public AppDataContext()
            : base(ConnectionString)
        {
        }

        #endregion

        #region [ Properties ]

        public Table<Category> Categories = null;

        public Table<Credit> Credits = null;

        public Table<Debit> Debits = null;

        #endregion
    }
}