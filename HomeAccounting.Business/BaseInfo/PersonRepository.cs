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
using HomeAccounting.DataAccess.Enums;

namespace HomeAccounting.Business.BaseInfo
{
    public class PersonRepository : Repository<Person>
    {
        internal PersonRepository(DbContext context) : base(context)
        {
        }

        public override BusinessResult<List<Person>> GetAll(string predicate, object[] values, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null)
        {
            if (string.IsNullOrEmpty(predicate))
                predicate = "RecordStatusId == @0";
            else
                predicate = predicate + "AND RecordStatusId == @0";

            if (string.IsNullOrEmpty(orderBy))
                orderBy = "OrderItem desc";
            else
                orderBy = "OrderItem desc," + orderBy;

            return base.GetAll(predicate, new object[] { RecordStatus.Exist }, orderBy, take, skip, include);
        }

        public BusinessResult<bool> ChangeStatus(Person model)
        {
            model.RecordStatusId = RecordStatus.Deleted;
            return Update(model);
        }

        public BusinessResult<bool> AddOrUpdate(Person model, bool isEdit)
        {
            UpdateBusiness.OnBeforeExecute = () =>
            {
                var result = Any(x => x.Name.Equals(model.Name) && x.PersonId != model.PersonId && x.RecordStatusId == RecordStatus.Exist);
                if (result.Data)
                    UpdateBusiness.Result.Message = new BusinessMessage("اطلاعات تکراری", "شخص وارد شده تکراری می باشد.", Infra.Wpf.Controls.MessageType.Error);
                return !result.Data;
            };
            AddBusiness.OnBeforeExecute = () =>
            {
                var result = Any(x => x.Name.Equals(model.Name) && x.RecordStatusId == RecordStatus.Exist);
                if (result.Data)
                    AddBusiness.Result.Message = new BusinessMessage("اطلاعات تکراری", "شخص وارد شده تکراری می باشد.", Infra.Wpf.Controls.MessageType.Error);
                return !result.Data;
            };

            model.UserId = 1;
            model.RecordStatusId = RecordStatus.Exist;
            if (!isEdit)
                model.CreateDate = DateTime.Now;

            if (isEdit)
                return Update(model);
            else
                return Add(model);
        }
    }
}
