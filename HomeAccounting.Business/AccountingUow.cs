using HomeAccounting.Business.BaseInfo;
using HomeAccounting.DataAccess.Models;
using Infra.Wpf.Common.Helpers;
using Infra.Wpf.Repository;
using Infra.Wpf.Security;

namespace HomeAccounting.Business
{
    public class AccountingUow : UnitOfWork
    {
        private IRepository<Person> personRepository;

        public static string ConnectionString;

        private static string key = "87b81f22-13cf-4c2b-a17f-64f6464e62b6";

        static AccountingUow()
        {
            ConnectionString = ConnectionStringHelper.GetConnectionString("AccountingContext",key);
        }

        public IRepository<Person> PersonRepository
        {
            get { return personRepository ?? (personRepository = new PersonRepository(Context, Logger, false)); }
        }

        public AccountingUow() : this(new AccountingContext(ConnectionString))
        {
        }

        public AccountingUow(System.Data.Entity.DbContext context) : base(context, new Infra.Wpf.Business.Logger(ConnectionString))
        {
        }

        protected override void Dispose(bool disposing)
        {
            personRepository = null;

            base.Dispose(disposing);
        }
    }
}
