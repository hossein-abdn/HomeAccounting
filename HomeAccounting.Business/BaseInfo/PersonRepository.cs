using HomeAccounting.DataAccess.Models;
using Infra.Wpf.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using Infra.Wpf.Business;
using HomeAccounting.DataAccess.Enums;
using System.Threading;
using Infra.Wpf.Security;

namespace HomeAccounting.Business.BaseInfo
{
    public class PersonRepository : Repository<Person>
    {
        internal PersonRepository(DbContext context, Logger logger, bool logOnException) : base(context, logger, logOnException)
        {
        }

        public override BusinessResult<List<Person>> GetAll(string predicate, object[] values, string orderBy = null, int? take = default(int?), int? skip = default(int?), string include = null)
        {
            if (string.IsNullOrEmpty(predicate))
                predicate = "RecordStatusId == @0";
            else
                predicate = predicate + " AND RecordStatusId == @0";

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
            if (listResult.HasException)
                return new BusinessResult<bool> { Exception = listResult.Exception, Message = listResult.Message };

            foreach (var item in listResult.Data)
                item.OrderItem--;

            return Update(model);
        }

        public BusinessResult<bool> AddOrUpdate(Person model, bool isEdit)
        {
            model.UserId = (Thread.CurrentPrincipal.Identity as Identity).Id;
            model.RecordStatusId = RecordStatus.Exist;

            var resultOrder = ChangeOrderItem(isEdit, model);
            if (resultOrder.HasException)
                return new BusinessResult<bool> { Exception = resultOrder.Exception, Message = resultOrder.Message };

            if (isEdit)
            {
                UpdateBusiness.OnBeforeExecute = () =>
                {
                    var result = Any(x => x.Name.Equals(model.Name) && x.PersonId != model.PersonId && x.RecordStatusId == RecordStatus.Exist);
                    if (result.HasException)
                    {
                        UpdateBusiness.Result = new BusinessResult<bool> { Exception = result.Exception, Message = result.Message };
                        return false;
                    }
                    else if (result.Data)
                    {
                        UpdateBusiness.Result.Message = new BusinessMessage("اطلاعات تکراری", "شخص وارد شده تکراری می باشد.", Infra.Wpf.Controls.MessageType.Error);
                        return false;
                    }

                    return true;
                };

                return Update(model);
            }
            else
            {
                var resultDate = GetDateTime();
                if(resultDate.HasException)
                    return new BusinessResult<bool> { Exception = resultDate.Exception, Message = resultDate.Message };

                model.CreateDate = resultDate.Data;

                AddBusiness.OnBeforeExecute = () =>
                {
                    var result = Any(x => x.Name.Equals(model.Name) && x.RecordStatusId == RecordStatus.Exist);
                    if (result.HasException)
                    {
                        AddBusiness.Result = new BusinessResult<bool> { Exception = result.Exception, Message = result.Message };
                        return false;
                    }
                    else if (result.Data)
                    {
                        AddBusiness.Result.Message = new BusinessMessage("اطلاعات تکراری", "شخص وارد شده تکراری می باشد.", Infra.Wpf.Controls.MessageType.Error);
                        return false;
                    }

                    return true;
                };

                return Add(model);
            }
        }

        public BusinessResult ChangeOrderItem(bool isEdit, Person model)
        {
            SetOrderItemsBusiness setOrderItems = new SetOrderItemsBusiness(this, this.Context, isEdit, model, Logger);
            setOrderItems.Execute();

            return setOrderItems.Result;
        }
    }
}
