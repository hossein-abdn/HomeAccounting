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
                orderBy = "OrderItem";
            else
                orderBy = "OrderItem," + orderBy;

            return base.GetAll(predicate, new object[] { RecordStatus.Exist }, orderBy, take, skip, include);
        }

        public BusinessResult<bool> ChangeStatus(Person model)
        {
            model.RecordStatusId = RecordStatus.Deleted;
            var listResult = GetAll(x => x.OrderItem > model.OrderItem && x.RecordStatusId == RecordStatus.Exist);
            if(listResult.Exception !=null)
                return new BusinessResult<bool> { Exception = listResult.Exception, Message = listResult.Message };

            foreach (var item in listResult.Data)
                item.OrderItem--;

            return Update(model);
        }

        public BusinessResult<bool> AddOrUpdate(Person model, bool isEdit)
        {
            AddBusiness.OnBeforeExecute = () =>
            {
                var result = Any(x => x.Name.Equals(model.Name) && x.RecordStatusId == RecordStatus.Exist);
                if (result.Data)
                    AddBusiness.Result.Message = new BusinessMessage("اطلاعات تکراری", "شخص وارد شده تکراری می باشد.", Infra.Wpf.Controls.MessageType.Error);
                return !result.Data;
            };

            UpdateBusiness.OnBeforeExecute = () =>
            {
                var result = Any(x => x.Name.Equals(model.Name) && x.PersonId != model.PersonId && x.RecordStatusId == RecordStatus.Exist);
                if (result.Data)
                    UpdateBusiness.Result.Message = new BusinessMessage("اطلاعات تکراری", "شخص وارد شده تکراری می باشد.", Infra.Wpf.Controls.MessageType.Error);
                return !result.Data;
            };

            model.UserId = 1;
            model.RecordStatusId = RecordStatus.Exist;
            if (!isEdit)
                model.CreateDate = DateTime.Now;

            var orderItemResult = SetOrderItems(isEdit, model);
            if (orderItemResult != null && orderItemResult.Exception != null)
                return new BusinessResult<bool> { Exception = orderItemResult.Exception, Message = orderItemResult.Message };

            if (isEdit)
                return Update(model);
            else
                return Add(model);
        }

        private BusinessResult SetOrderItems(bool isEdit, Person model)
        {
            try
            {
                var countResult = GetCount(x => x.RecordStatusId == RecordStatus.Exist);
                if (countResult.Exception != null)
                    throw countResult.Exception;

                if (isEdit)
                {
                    var entry = Context.Entry(model);
                    if (entry.State == EntityState.Detached)
                    {
                        Set.Attach(model);
                        entry = Context.Entry(model);
                        entry.State = EntityState.Modified;
                    }
                    var orginalValue = (entry.OriginalValues["OrderItem"] as int?);

                    if (model.OrderItem == null || model.OrderItem > countResult.Data)
                        model.OrderItem = countResult.Data;

                    if (model.OrderItem > orginalValue)
                    {
                        var listResult = GetAll(x => x.OrderItem > orginalValue && x.OrderItem <= model.OrderItem && x.RecordStatusId == RecordStatus.Exist);
                        if (listResult.Exception != null)
                            throw listResult.Exception;

                        foreach (var item in listResult.Data)
                            item.OrderItem--;
                    }
                    else if (model.OrderItem < orginalValue)
                    {
                        var listResult = GetAll(x => x.OrderItem >= model.OrderItem && x.OrderItem < orginalValue && x.RecordStatusId == RecordStatus.Exist);
                        if (listResult.Exception != null)
                            throw listResult.Exception;

                        foreach (var item in listResult.Data)
                            item.OrderItem++;
                    }
                }
                else
                {
                    if (model.OrderItem == null || model.OrderItem > countResult.Data + 1)
                        model.OrderItem = countResult.Data + 1;
                    else
                    {
                        var listResult = GetAll(x => x.OrderItem >= model.OrderItem && x.RecordStatusId == RecordStatus.Exist);
                        if (listResult.Exception != null)
                            throw listResult.Exception;

                        foreach (var item in listResult.Data)
                            item.OrderItem++;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return new BusinessResult { Exception = ex, Message = new BusinessMessage("خطا", "در سامانه خطایی رخ داده است.") };
            }
        }
    }
}
