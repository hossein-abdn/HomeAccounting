using HomeAccounting.DataAccess.Models;
using HomeAccounting.UI.Views.BaseInfo;
using Infra.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infra.Wpf.Common.Helpers;
using HomeAccounting.Business;
using Infra.Wpf.Business;
using HomeAccounting.UI.Validators;
using FluentValidation.Results;
using HomeAccounting.Business.BaseInfo;

namespace HomeAccounting.UI.ViewModels.BaseInfo
{
    [ViewType(typeof(PersonCreateView))]
    public class PersonCreateVM : ViewModelBase<Person>
    {
        private bool isEdit = false;

        public RelayCommand SubmitCommand { get; set; }

        private AccountingUow accountingUow { get; set; }

        public PersonCreateVM(AccountingUow uow, Person model = null)
        {
            SubmitCommand = new RelayCommand(SubmitExecute);

            if (model == null)
            {
                ViewTitle = "افزودن شخص";
                Model = new Person();
            }
            else
            {
                isEdit = true;
                ViewTitle = "ویرایش شخص";
                Model = model;
                Messenger.Default.Send(Model.PersonId, "PersonListView_SaveItemId");
            }

            accountingUow = uow;
        }

        private void SubmitExecute()
        {
            PersonValidator validator = new PersonValidator();
            ValidationResult validationResult = validator.Validate(Model);
            if (!validationResult.IsValid)
            {
                foreach (var item in validationResult.Errors)
                    Billboard.ShowMessage(Infra.Wpf.Controls.MessageType.Error, item.ErrorMessage);
                return;
            }

            BusinessResult<bool> result = ((PersonRepository) accountingUow.PersonRepository).AddOrUpdate(Model, isEdit);
            if (result.HasException == false && result.IsOnBeforExecute)
            {
                BusinessResult<int> saveResult = accountingUow.SaveChange();
                if (saveResult.HasException)
                    result.Message = saveResult.Message;
                else
                {
                    NavigationService.GoBack();
                    Messenger.Default.Send(Model.PersonId, "PersonListView_SaveItemId");
                }
            }

            Billboard.ShowMessage(result.Message.MessageType, result.Message.Message);
        }
    }
}