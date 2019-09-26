using HomeAccounting.Business.Repository;
using HomeAccounting.DataAccess.Enums;
using HomeAccounting.DataAccess.Models;
using System;
using System.Threading;
using System.Collections.Generic;
using Infra.Wpf.Common;
using Infra.Wpf.Business;
using Infra.Wpf.Controls;
using Infra.Wpf.Security;

namespace HomeAccounting.Business
{
    public class PersonBusinessSet
    {
        private PersonRepository repository;

        private ILogger logger;

        public PersonBusinessSet(PersonRepository repository, ILogger logger = null)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public BusinessResult<List<Person>> GetAll(string predicate)
        {
            BusinessBase<List<Person>> getAll = new BusinessBase<List<Person>>(logger);

            getAll.Execute(() =>
            {
                string orderBy = "OrderItem";
                if (string.IsNullOrEmpty(predicate))
                    predicate = "RecordStatusId == @0";
                else
                    predicate = predicate + " AND RecordStatusId == @0";

                getAll.Result.Data = repository.GetAll(predicate, new object[] { RecordStatus.Exist }, orderBy);
                return true;
            });

            return getAll.Result;
        }

        public BusinessResult<bool> Delete(Person model)
        {
            BusinessBase<bool> delete = new BusinessBase<bool>(logger);

            delete.Execute(() =>
            {
                model.RecordStatusId = RecordStatus.Deleted;

                var listResult = repository.GetAll(x => x.OrderItem > model.OrderItem && x.RecordStatusId == RecordStatus.Exist);
                foreach (var item in listResult)
                    item.OrderItem--;

                return repository.Update(model);
            });

            if (delete.Result.IsOnExecute)
            {
                delete.Result.Data = true;
                delete.Result.Message = new BusinessMessage("حذف اطلاعات", "عملیات حذف با موفقیت انجام شد.", MessageType.Information);
            }

            return delete.Result;
        }

        public BusinessResult<bool> Add(Person model)
        {
            BusinessBase<bool> add = new BusinessBase<bool>(logger);

            Func<bool> onBeforeExecute = () => 
            {
                var result = repository.Any(x => x.Name.Equals(model.Name) && x.RecordStatusId == RecordStatus.Exist);
                if (result)
                {
                    add.Result.Message = new BusinessMessage("اطلاعات تکراری", "شخص وارد شده تکراری می باشد.", MessageType.Error);
                    return false;
                }

                return true;
            };

            Func<bool> onExecute = () => 
            {
                SetOrderItems(false, model);

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

        public BusinessResult<bool> Update(Person model)
        {
            BusinessBase<bool> update = new BusinessBase<bool>(logger);

            Func<bool> onBeforeExecute = () =>
            {
                var result = repository.Any(x => x.Name.Equals(model.Name) && x.PersonId != model.PersonId && x.RecordStatusId == RecordStatus.Exist);
                if (result)
                {
                    update.Result.Message = new BusinessMessage("اطلاعات تکراری", "شخص وارد شده تکراری می باشد.", MessageType.Error);
                    return false;
                }

                return true;
            };

            Func<bool> onExecute = () =>
            {
                SetOrderItems(true, model);
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

        public void SetOrderItems(bool isEdit, Person model)
        {
            var countResult = repository.GetCount(x => x.RecordStatusId == RecordStatus.Exist);

            if (isEdit)
            {
                var orginalModel = repository.GetFirst(x => x.PersonId == model.PersonId);
                orginalModel.OrderItem = model.OrderItem;

                var orginalValue = repository.GetOrginalOrderItem(orginalModel);

                if (orginalModel.OrderItem == null || orginalModel.OrderItem > countResult)
                    orginalModel.OrderItem = countResult;

                if (orginalModel.OrderItem > orginalValue)
                {
                    var listResult = repository.GetAll(x => x.OrderItem > orginalValue && x.OrderItem <= orginalModel.OrderItem && x.RecordStatusId == RecordStatus.Exist);

                    foreach (var item in listResult)
                        item.OrderItem--;
                }
                else if (orginalModel.OrderItem < orginalValue)
                {
                    var listResult = repository.GetAll(x => x.OrderItem >= orginalModel.OrderItem && x.OrderItem < orginalValue && x.RecordStatusId == RecordStatus.Exist);

                    foreach (var item in listResult)
                        item.OrderItem++;
                }
            }
            else
            {
                if (model.OrderItem == null || model.OrderItem > countResult + 1)
                    model.OrderItem = countResult + 1;
                else
                {
                    var listResult = repository.GetAll(x => x.OrderItem >= model.OrderItem && x.RecordStatusId == RecordStatus.Exist);

                    foreach (var item in listResult)
                        item.OrderItem++;
                }
            }
        }
    }
}
