using HomeAccounting.Business.BaseInfo;
using HomeAccounting.DataAccess.Models;
using Infra.Wpf.Repository;

namespace HomeAccounting.Business
{
    public class AccountingUow : UnitOfWork
    {
        private IRepository<Person> personRepository;

        public IRepository<Person> PersonRepository
        {
            get { return personRepository ?? (personRepository = new PersonRepository(Context, Logger, false)); }
        }

        public AccountingUow() : this(new AccountingContext())
        {
        }

        public AccountingUow(System.Data.Entity.DbContext context) : base(context, new Infra.Wpf.Business.Logger("AccountingContext"))
        {
        }

        protected override void Dispose(bool disposing)
        {
            personRepository = null;

            base.Dispose(disposing);
        }
    }
}
