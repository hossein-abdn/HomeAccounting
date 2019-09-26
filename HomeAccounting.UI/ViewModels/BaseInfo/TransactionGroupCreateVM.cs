using HomeAccounting.Business;
using HomeAccounting.DataAccess.Models;
using HomeAccounting.UI.Views;
using Infra.Wpf.Mvvm;
using System.Collections.Generic;

namespace HomeAccounting.UI.ViewModels
{
    [ViewType(typeof(TransactionGroupCreateView))]
    public class TransactionGroupCreateVM : ViewModelBase<TransactionGroup>
    {
        private bool isEdit = false;

        public RelayCommand SubmitCommand { get; set; }

        public RelayCommand<List<KeyValuePair<string, string>>> GetAllCommand { get; set; }

        private AccountingUow accountingUow { get; set; }

        //LookupTransactionGroupList
        //GetAllCommand

        public TransactionGroupCreateVM(AccountingUow uow, TransactionGroup model = null,bool isEdit = false)
        {
            SubmitCommand = new RelayCommand(SubmitExecute);
            GetAllCommand = new RelayCommand<List<KeyValuePair<string, string>>>(GetAllExecute);

            this.isEdit = isEdit;
            Model = model;

            if(isEdit ==  false)
            {
                ViewTitle = "افزودن گروه هزینه و درآمد";
                if (Model == null)
                    Model = new TransactionGroup();
            }
            else
                ViewTitle = "ویرایش گروه هزینه و درآمد";

            accountingUow = uow;

            Model.Exclude(new string[] { "TransactionGroupId", "CreateDate", "RecordStatusId" });
        }

        private void GetAllExecute(List<KeyValuePair<string, string>> obj)
        {
            ItemsSource = null;
        }

        private void SubmitExecute()
        {
            
        }
    }
}
