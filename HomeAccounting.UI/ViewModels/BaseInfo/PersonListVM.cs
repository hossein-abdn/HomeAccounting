using HomeAccounting.Business;
using HomeAccounting.DataAccess.Models;
using HomeAccounting.UI.Views.BaseInfo;
using Infra.Wpf.Mvvm;
using System.Collections.ObjectModel;
using System;
using Infra.Wpf.Business;
using HomeAccounting.Business.BaseInfo;

namespace HomeAccounting.UI.ViewModels.BaseInfo
{
    [ViewType(typeof(PersonListView))]
    public class PersonListVM : ViewModelBase<Person>
    {
        public RelayCommand<string> GetAllCommand { get; set; }

        public RelayCommand LoadedEventCommand { get; set; }

        public RelayCommand<Person> CreateEditCommand { get; set; }

        public RelayCommand<Person> ChangeStatusCommand { get; set; }

        public string SearchPhrase
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        private AccountingUow accountingUow { get; set; }


        public PersonListVM(AccountingUow uow)
        {
            ViewTitle = "لیست اشخاص";

            GetAllCommand = new RelayCommand<string>(GetAllExecute);
            LoadedEventCommand = new RelayCommand(LoadedEventExecute);
            CreateEditCommand = new RelayCommand<Person>(CreateEditExecute);
            ChangeStatusCommand = new RelayCommand<Person>(ChangeStatusExecute);

            accountingUow = uow;
        }

        private void ChangeStatusExecute(Person model)
        {
            if (ShowMessageBox("آیا مطمئن هستید؟", "حذف", Infra.Wpf.Controls.MsgButton.YesNo, Infra.Wpf.Controls.MsgIcon.Question, Infra.Wpf.Controls.MsgResult.No) == Infra.Wpf.Controls.MsgResult.Yes)
            {
                Messenger.Default.Send(model, "PersonListView_SaveItemIndex");
                var result = ((PersonRepository) accountingUow.PersonRepository).ChangeStatus(model);

                BusinessResult<int> saveResult = null;
                if (result.Exception == null)
                {
                    saveResult = accountingUow.SaveChange();
                    if (saveResult.Exception != null)
                        result.Message = saveResult.Message;

                    GetAllExecute(SearchPhrase);
                    Messenger.Default.Send("index", "PersonListView_SetScrollView");
                }

                Billboard.ShowMessage(result.Message.MessageType, result.Message.Message);
            }
        }

        private void CreateEditExecute(Person model)
        {
            NavigationService.NavigateTo(new PersonCreateVM(accountingUow, model));
        }

        private void LoadedEventExecute()
        {
            GetAllExecute(SearchPhrase);
            Messenger.Default.Send("id", "PersonListView_SetScrollView");
        }

        private void GetAllExecute(string predicate)
        {
            var result = accountingUow.PersonRepository.GetAll(predicate: predicate);

            if (result.Exception == null)
                ItemsSource = new ObservableCollection<Person>(result.Data);
            else
                Billboard.ShowMessage(result.Message.MessageType, result.Message.Message);
        }
    }
}
