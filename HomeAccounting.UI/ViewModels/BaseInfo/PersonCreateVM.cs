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

            Model.Exclude(new string[] { "UserId", "CreateDate", "RecordStatusId" });
        }

        private void SubmitExecute()
        {
            ValidationResult validationResult = Model.Validate();
            if (!validationResult.IsValid)
            {
                Billboard.ShowMessage(Infra.Wpf.Controls.MessageType.Error, validationResult.Errors[0].ErrorMessage);
                return;
            }

            BusinessResult<bool> result = ((PersonRepository)accountingUow.PersonRepository).AddOrUpdate(Model, isEdit);
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