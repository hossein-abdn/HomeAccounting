using HomeAccounting.DataAccess.Models;
using Infra.Wpf.Repository;
using System.Collections.Generic;
using System.Data.Entity;
using Infra.Wpf.Business;
using HomeAccounting.DataAccess.Enums;
using System.Threading;
using Infra.Wpf.Security;
using Infra.Wpf.Common;

namespace HomeAccounting.Business.Repository
{
    public class PersonRepository : Repository<Person>
    {
        internal PersonRepository(DbContext context, ILogger logger) : base(context, logger)
        {
        }

        public int? GetOrginalOrderItem(Person model)
        {
            var entry = Context.Entry(model);
            return (entry.OriginalValues["OrderItem"] as int?);
        }
    }
}
