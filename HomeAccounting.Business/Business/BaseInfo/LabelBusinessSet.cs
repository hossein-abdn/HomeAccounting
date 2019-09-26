using System;
using System.Collections.Generic;
using System.Threading;
using Infra.Wpf.Common;
using Infra.Wpf.Business;
using Infra.Wpf.Security;
using Infra.Wpf.Controls;
using Infra.Wpf.Repository;
using HomeAccounting.DataAccess.Enums;
using HomeAccounting.DataAccess.Models;

namespace HomeAccounting.Business
{
    public class LabelBusinessSet
    {
        private IRepository<Label> repository;

        private ILogger logger;

        public LabelBusinessSet(IRepository<Label> repository, ILogger logger = null)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public BusinessResult<List<Label>> GetAll(string predicate)
        {
            BusinessBase<List<Label>> getAll = new BusinessBase<List<Label>>(logger);

            getAll.Execute(() =>
            {
                string orderBy = "CreateDate";
                if (string.IsNullOrEmpty(predicate))
                    predicate = "RecordStatusId == @0";
                else
                    predicate = predicate + " AND RecordStatusId == @0";

                getAll.Result.Data = repository.GetAll(predicate, new object[] { RecordStatus.Exist }, orderBy);
                return true;
            });

            return getAll.Result;
        }

        public BusinessResult<bool> Delete(Label model)
        {
            BusinessBase<bool> delete = new BusinessBase<bool>(logger);

            model.RecordStatusId = RecordStatus.Deleted;
            delete.Execute(() => repository.Update(model));
            if (delete.Result.IsOnExecute)
            {
                delete.Result.Data = true;
                delete.Result.Message = new BusinessMessage("حذف اطلاعات", "عملیات حذف با موفقیت انجام شد.", MessageType.Information);
            }

            return delete.Result;
        }

        public BusinessResult<bool> Add(Label model)
        {
            BusinessBase<bool> add = new BusinessBase<bool>(logger);

            Func<bool> onBeforeExecute = () =>
            {
                bool result = repository.Any(x => x.Title == model.Title && x.RecordStatusId == RecordStatus.Exist);
                if (result)
                {
                    add.Result.Data = false;
                    add.Result.Message = new BusinessMessage("اطلاعات تکراری", "برچسب وارده شده تکراری می باشد.", MessageType.Error);
                    return false;
                }

                return true;
            };

            Func<bool> onExecute = () =>
            {
                model.UserId = (Thread.CurrentPrincipal.Identity as Identity).Id;
                model.RecordStatusId = RecordStatus.Exist;
                model.CreateDate = repository.GetDateTime();
                return repository.Add(model);
            };

            add.Execute(onBeforeExecute, onExecute, null);
            if (add.Result.IsOnExecute)
            {
                add.Result.Data = true;
                add.Result.Message = new BusinessMessage("افزودن اطلاعات", "عملیات افزودن با موفقیت انجام شد.", MessageType.Information);
            }

            return add.Result;
        }

        public BusinessResult<bool> Update(Label model)
        {
            BusinessBase<bool> update = new BusinessBase<bool>(logger);

            Func<bool> onBeforeExecute = () =>
            {
                var result = repository.Any(x => x.Title.Equals(model.Title) && x.LabelId != model.LabelId && x.RecordStatusId == RecordStatus.Exist);
                if (result)
                {
                    update.Result.Message = new BusinessMessage("اطلاعات تکراری", "برچسب وارد شده تکراری می باشد.", MessageType.Error);
                    return false;
                }

                return true;
            };

            Func<bool> onExecute = () =>
            {
                model.UserId = (Thread.CurrentPrincipal.Identity as Identity).Id;
                return repository.Update(model);
            };

            update.Execute(onBeforeExecute, onExecute, null);
            if (update.Result.IsOnExecute)
            {
                update.Result.Data = true;
                update.Result.Message = new BusinessMessage("افزودن اطلاعات", "عملیات ویرایش با موفقیت انجام شد.", MessageType.Information);
            }

            return update.Result;
        }
    }
}
