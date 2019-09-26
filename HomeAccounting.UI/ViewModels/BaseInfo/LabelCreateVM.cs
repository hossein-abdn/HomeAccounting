using FluentValidation.Results;
using HomeAccounting.Business;
using HomeAccounting.DataAccess.Models;
using HomeAccounting.UI.Views;
using Infra.Wpf.Business;
using Infra.Wpf.Mvvm;

namespace HomeAccounting.UI.ViewModels
{
    [ViewType(typeof(LabelCreateView))]
    public class LabelCreateVM : ViewModelBase<Label>
    {
        private bool isEdit = false;

        public RelayCommand SubmitCommand { get; set; }

        private AccountingUow accountingUow { get; set; }

        private LabelBusinessSet businessSet { get; set; }

        public LabelCreateVM(AccountingUow uow, Label model = null)
        {
            SubmitCommand = new RelayCommand(SubmitExecute);

            if(model == null)
            {
                ViewTitle = "افزودن برچسب";
                Model = new Label();
            }
            else
            {
                ViewTitle = "ویرایش برچسب";
                isEdit = true;
                Model = model;
                Messenger.Default.Send(Model.LabelId, "LabelListView_SaveItemId");
            }

            accountingUow = uow;
            businessSet = new LabelBusinessSet(accountingUow.LabelRepository, accountingUow.Logger);

            Model.Exclude(new string[] { "UserId", "RecordStatusId", "CreateDate" });
        }

        private void SubmitExecute()
        {
            ValidationResult validationResult = Model.Validate();
            if(!validationResult.IsValid)
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
            
            if(result.HasException == false && result.IsOnBeforExecute)
            {
                BusinessResult<int> saveResult = accountingUow.SaveChange();
                if (saveResult.HasException)
                    result.Message = saveResult.Message;
                else
                {
                    NavigationService.GoBack();
                    Messenger.Default.Send(Model.LabelId, "LabelListView_SaveItemId");
                }
            }

            Billboard.ShowMessage(result.Message.MessageType, result.Message.Message);
        }
    }
}
