using HomeAccounting.DataAccess.Enums;
using HomeAccounting.DataAccess.Models;
using Infra.Wpf.Business;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.Business.BaseInfo
{
    public class SetOrderItemsBusiness : BusinessBase
    {
        private PersonRepository _repository { get; set; }

        private bool _isEdit { get; set; }

        public Person _model { get; set; }

        public DbContext _context { get; set; }

        public SetOrderItemsBusiness(PersonRepository repository, DbContext context, bool isEdit, Person model, Logger logger) : base(logger)
        {
            _repository = repository;
            _isEdit = isEdit;
            _model = model;
            _context = context;
            OnExecute = SetOrderItems;
        }

        public bool SetOrderItems()
        {
            var countResult = _repository.GetCount(x => x.RecordStatusId == RecordStatus.Exist);
            if (countResult.HasException)
                throw countResult.Exception;

            if (_isEdit)
            {
                var result = _repository.GetFirst(x => x.PersonId == _model.PersonId);
                if (result.HasException)
                    throw result.Exception;

                var orginalModel = result.Data;
                orginalModel.OrderItem = _model.OrderItem;
                var entry = _context.Entry(orginalModel);
                var orginalValue = (entry.OriginalValues["OrderItem"] as int?);

                if (orginalModel.OrderItem == null || orginalModel.OrderItem > countResult.Data)
                    orginalModel.OrderItem = countResult.Data;

                if (orginalModel.OrderItem > orginalValue)
                {
                    var listResult = _repository.GetAll(x => x.OrderItem > orginalValue && x.OrderItem <= orginalModel.OrderItem && x.RecordStatusId == RecordStatus.Exist);
                    if (listResult.HasException)
                        throw listResult.Exception;

                    foreach (var item in listResult.Data)
                        item.OrderItem--;
                }
                else if (orginalModel.OrderItem < orginalValue)
                {
                    var listResult = _repository.GetAll(x => x.OrderItem >= orginalModel.OrderItem && x.OrderItem < orginalValue && x.RecordStatusId == RecordStatus.Exist);
                    if (listResult.HasException)
                        throw listResult.Exception;

                    foreach (var item in listResult.Data)
                        item.OrderItem++;
                }
            }
            else
            {
                if (_model.OrderItem == null || _model.OrderItem > countResult.Data + 1)
                    _model.OrderItem = countResult.Data + 1;
                else
                {
                    var listResult = _repository.GetAll(x => x.OrderItem >= _model.OrderItem && x.RecordStatusId == RecordStatus.Exist);
                    if (listResult.HasException)
                        throw listResult.Exception;

                    foreach (var item in listResult.Data)
                        item.OrderItem++;
                }
            }

            Result.Message = new BusinessMessage(" اطلاعات", "عملیات با موفقیت انجام شد.", Infra.Wpf.Controls.MessageType.Information);

            return true;
        }
    }
}
