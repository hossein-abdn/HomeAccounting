using HomeAccounting.UI.Views;
using HomeAccounting.Business;
using HomeAccounting.DataAccess.Models;
using HomeAccounting.Business.Repository;
using Infra.Wpf.Mvvm;
using Infra.Wpf.Business;
using FluentValidation.Results;

namespace HomeAccounting.UI.ViewModels
{
    [ViewType(typeof(PersonCreateView))]
    public class PersonCreateVM : ViewModelBase<Person>
    {
        private bool isEdit = false;

        public RelayCommand SubmitCommand { get; set; }

        private AccountingUow accountingUow { get; set; }

        private PersonBusinessSet businessSet { get; set; }

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
            businessSet = new PersonBusinessSet((PersonRepository)accountingUow.PersonRepository, accountingUow.Logger);

            Model.Exclude(new string[] { "UserId", "CreateDate", "RecordStatusId" });
        }

        private void SubmitExecute()
        {
            ValidationResult validationResult = Model.Validate();
            if (!validationResult.IsValid)
            {
                Billboard.ShowMessage(Infra.Wpf.Controls.MessageType.Error, validationResult.Errors[0].ErrorMessage);
                FocusByPropertyName(validationResult.Errors[0].PropertyName);
                return;
            }

            BusinessResult<bool> result;
            if (isEdit)
                result = businessSet.Update(Model);
            else
                result = businessSet.Add(Model);

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