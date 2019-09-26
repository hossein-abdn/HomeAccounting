using System.Collections.Generic;
using System.Collections.ObjectModel;
using Infra.Wpf.Mvvm;
using Infra.Wpf.Business;
using Infra.Wpf.Common.Helpers;
using HomeAccounting.Business;
using HomeAccounting.DataAccess.Models;
using HomeAccounting.Business.Repository;
using HomeAccounting.UI.Views;

namespace HomeAccounting.UI.ViewModels
{
    [ViewType(typeof(PersonListView))]
    public class PersonListVM : ViewModelBase<Person>
    {
        public RelayCommand<List<KeyValuePair<string, string>>> GetAllCommand { get; set; }

        public RelayCommand LoadedEventCommand { get; set; }

        public RelayCommand<Person> CreateEditCommand { get; set; }

        public RelayCommand<Person> ChangeStatusCommand { get; set; }

        public RelayCommand<Person> UpCommand { get; set; }

        public RelayCommand<Person> DownCommand { get; set; }

        public List<KeyValuePair<string, string>> SearchPhraseList
        {
            get { return Get<List<KeyValuePair<string, string>>>(); }
            set { Set(value); }
        }

        private AccountingUow accountingUow { get; set; }

        private PersonBusinessSet businessSet { get; set; }

        public PersonListVM()
        {
            ViewTitle = "لیست اشخاص";

            GetAllCommand = new RelayCommand<List<KeyValuePair<string, string>>>(GetAllExecute);
            LoadedEventCommand = new RelayCommand(LoadedEventExecute);
            CreateEditCommand = new RelayCommand<Person>(CreateEditExecute);
            ChangeStatusCommand = new RelayCommand<Person>(ChangeStatusExecute);
            UpCommand = new RelayCommand<Person>(UpExecute);
            DownCommand = new RelayCommand<Person>(DownExecute);

            accountingUow = new AccountingUow();
            businessSet = new PersonBusinessSet((PersonRepository)accountingUow.PersonRepository, accountingUow.Logger);
        }

        private void DownExecute(Person model)
        {
            var resultCount = accountingUow.PersonRepository.GetCount(x => x.RecordStatusId == DataAccess.Enums.RecordStatus.Exist);
            if (model.OrderItem < resultCount)
            {
                model.OrderItem++;
                businessSet.SetOrderItems(true, model);

                BusinessResult<int> saveResult = accountingUow.SaveChange();
                if (saveResult.HasException)
                    Billboard.ShowMessage(saveResult.Message.MessageType, saveResult.Message.Message);
                else
                {
                    Messenger.Default.Send(model.PersonId, "PersonListView_SaveItemId");
                    LoadedEventExecute();
                }
            }
        }

        private void UpExecute(Person model)
        {
            if (model.OrderItem != 1)
            {
                model.OrderItem--;
                businessSet.SetOrderItems(true, model);

                BusinessResult<int> saveResult = accountingUow.SaveChange();
                if (saveResult.HasException)
                    Billboard.ShowMessage(saveResult.Message.MessageType, saveResult.Message.Message);
                else
                {
                    Messenger.Default.Send(model.PersonId, "PersonListView_SaveItemId");
                    LoadedEventExecute();
                }
            }
        }

        private void ChangeStatusExecute(Person model)
        {
            if (ShowMessageBox("آیا از حذف شخص مطمئین هستید؟", "حذف", Infra.Wpf.Controls.MsgButton.YesNo, Infra.Wpf.Controls.MsgIcon.Question, Infra.Wpf.Controls.MsgResult.No) == Infra.Wpf.Controls.MsgResult.Yes)
            {
                Messenger.Default.Send(model, "PersonListView_SaveItemIndex");

                var result = businessSet.Delete(model);
                if (result.HasException == false)
                {
                    BusinessResult<int> saveResult = accountingUow.SaveChange();
                    if (saveResult.HasException)
                        result.Message = saveResult.Message;
                    GetAllExecute(SearchPhraseList);
                    Messenger.Default.Send("index", "PersonListView_SetScrollView");
                }

                Billboard.ShowMessage(result.Message.MessageType, result.Message.Message);
            }
        }

        private void CreateEditExecute(Person model)
        {
            NavigationService.NavigateTo(new PersonCreateVM(accountingUow, model?.Copy()));
        }

        private void LoadedEventExecute()
        {
            GetAllExecute(SearchPhraseList);
            Messenger.Default.Send("id", "PersonListView_SetScrollView");
        }

        private void GetAllExecute(List<KeyValuePair<string, string>> filterList)
        {
            var predicate = GeneratePredicate(filterList);
            var result = businessSet.GetAll(predicate);
            if (result.HasException == false)
                ItemsSource = new ObservableCollection<Person>(result.Data);
            else
                Billboard.ShowMessage(result.Message.MessageType, result.Message.Message);
        }

        private string GeneratePredicate(List<KeyValuePair<string, string>> filterList)
        {
            string result = "";
            if (filterList != null)
            {
                foreach (var item in filterList)
                {
                    if (!string.IsNullOrWhiteSpace(item.Value))
                    {
                        if (!string.IsNullOrEmpty(result))
                            result += " AND ";
                        result += item.Value;
                    }
                }
            }

            return result;
        }
    }
}
