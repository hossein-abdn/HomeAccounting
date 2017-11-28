using HomeAccounting.DataAccess.Models;
using Infra.Wpf.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infra.Wpf.Business;
using System.Linq.Expressions;

namespace HomeAccounting.Business.BaseInfo
{
    public class PersonRepository : Repository<Person>
    {
        internal PersonRepository(DbContext context):base(context)
        {
        }

        public override BusinessResult<List<Person>> GetAll(string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null)
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "OrderItem desc";
            else
                orderBy = "OrderItem desc," + orderBy;

            return base.GetAll(orderBy, take, skip, include);
        }

        public override BusinessResult<List<Person>> GetAll(Expression<Func<Person, bool>> predicate, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null)
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "OrderItem desc";
            else
                orderBy = "OrderItem desc," + orderBy;

            return base.GetAll(predicate, orderBy, take, skip, include);
        }

        public override BusinessResult<List<Person>> GetAll(string predicate, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null)
        {
            if (string.IsNullOrEmpty(orderBy))
                orderBy = "OrderItem desc";
            else
                orderBy = "OrderItem desc," + orderBy;

            return base.GetAll(predicate, orderBy, take, skip, include);
        }
    }
}
