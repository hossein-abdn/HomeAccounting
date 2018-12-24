using HomeAccounting.DataAccess.Enums;
using HomeAccounting.DataAccess.Models;
using Infra.Wpf.Business;
using Infra.Wpf.Repository;
using Infra.Wpf.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HomeAccounting.Business.BaseInfo
{
    public class LabelRepository : Repository<Label>
    {
        internal LabelRepository(DbContext context, Logger logger = null, bool logOnException = true) : base(context, logger, logOnException)
        {
        }

        public override BusinessResult<List<Label>> GetAll(string predicate, object[] values = null, string orderBy = null, int? take = null, int? skip = null, string include = null)
        {
            if (string.IsNullOrEmpty(predicate))
                predicate = "RecordStatusId == @0";
            else
                predicate = predicate + " AND RecordStatusId == @0";

            if (string.IsNullOrEmpty(orderBy))
                orderBy = "CreateDate";

            return base.GetAll(predicate, new object[] { RecordStatus.Exist }, orderBy, take, skip, include);
        }

        public BusinessResult<bool> ChangeStatus(Label model)
        {
            model.RecordStatusId = RecordStatus.Deleted;
            return Update(model);
        }

        public BusinessResult<bool> AddOrUpdate(Label model, bool isEdit)
        {
            model.UserId = (Thread.CurrentPrincipal.Identity as Identity).Id;
            model.RecordStatusId = RecordStatus.Exist;

            if(isEdit)
            {
                UpdateBusiness.OnBeforeExecute = () =>
                {
                    var result = Any(x => x.Title.Equals(model.Title) && x.LabelId != model.LabelId && x.RecordStatusId == RecordStatus.Exist);
                    if (result.HasException)
                    {
                        UpdateBusiness.Result = new BusinessResult<bool> { Exception = result.Exception, Message = result.Message };
                        return false;
                    }
                    else if (result.Data)
                    {
                        UpdateBusiness.Result.Message = new BusinessMessage("اطلاعات تکراری", "برچسب وارد شده تکراری می باشد.", Infra.Wpf.Controls.MessageType.Error);
                        return false;
                    }

                    return true;
                };

                return Update(model);
            }
            else
            {
                var resultDate = GetDateTime();
                if (resultDate.HasException)
                    return new BusinessResult<bool> { Exception = resultDate.Exception, Message = resultDate.Message };

                model.CreateDate = resultDate.Data;

                AddBusiness.OnBeforeExecute = () =>
                {
                    var result = Any(x => x.Title == model.Title && x.RecordStatusId == RecordStatus.Exist);
                    if (result.HasException)
                    {
                        AddBusiness.Result = new BusinessResult<bool> { Exception = result.Exception, Message = result.Message };
                        return false;
                    }
                    else if (result.Data)
                    {
                        AddBusiness.Result.Message = new BusinessMessage("اطلاعات تکراری", "برچسب وارده شده تکراری می باشد.", Infra.Wpf.Controls.MessageType.Error);
                        return false;
                    }

                    return true;
                };

                return Add(model);
            }
        }
    }
}
