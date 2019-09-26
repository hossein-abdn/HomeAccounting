using Infra.Wpf.Repository;
using Infra.Wpf.Common.Helpers;
using HomeAccounting.Business.Repository;
using HomeAccounting.DataAccess.Models;

namespace HomeAccounting.Business
{
    public class AccountingUow : UnitOfWork
    {
        private IRepository<Person> personRepository;

        private IRepository<Label> labelRepository;

        private IRepository<TransactionGroup> transactionGroupRepository;

        public static string ConnectionString;

        private static string key = "87b81f22-13cf-4c2b-a17f-64f6464e62b6";

        static AccountingUow()
        {
            ConnectionString = ConnectionStringHelper.GetConnectionString("AccountingContext", key);
        }

        public IRepository<Person> PersonRepository
        {
            get { return personRepository ?? (personRepository = new PersonRepository(Context, Logger)); }
        }

        public IRepository<Label> LabelRepository
        {
            get { return labelRepository ?? (labelRepository = new Repository<Label>(Context, Logger)); }
        }

        public IRepository<TransactionGroup> TransactionGroupRepository
        {
            get { return transactionGroupRepository ?? (transactionGroupRepository = new Repository<TransactionGroup>(Context, Logger)); }
        }

        public AccountingUow() : this(new AccountingContext(ConnectionString))
        {
        }

        public AccountingUow(System.Data.Entity.DbContext context) : base(context, new Infra.Wpf.Common.Logger(ConnectionString))
        {
        }

        protected override void Dispose(bool disposing)
        {
            personRepository = null;

            base.Dispose(disposing);
        }
    }
}
