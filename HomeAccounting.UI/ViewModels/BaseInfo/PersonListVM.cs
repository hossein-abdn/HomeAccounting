using HomeAccounting.Business;
using HomeAccounting.DataAccess.Models;
using HomeAccounting.UI.Views.BaseInfo;
using Infra.Wpf.Mvvm;
using System.Collections.ObjectModel;
using System;

namespace HomeAccounting.UI.ViewModels.BaseInfo
{
    [ViewType(typeof(PersonListView))]
    public class PersonListVM : ViewModelBase<Person>
    {
        public RelayCommand<string> GetAllCommand { get; set; }

        public RelayCommand LoadedEventCommand { get; set; }

        public RelayCommand CreateCommand { get; set; }

        public PersonListVM()
        {
            ViewTitle = "لیست اشخاص";
            GetAllCommand = new RelayCommand<string>(GetAllExecute);
            LoadedEventCommand = new RelayCommand(LoadedEventExecute);
            CreateCommand = new RelayCommand(CreateExecute);
        }

        private void CreateExecute()
        {
            NavigationService.NavigateTo(new PersonCreateVM(null));
        }

        private void LoadedEventExecute()
        {
            GetAllExecute(string.Empty);
        }

        private void GetAllExecute(string predicate)
        {
            var uow = new AccountingUow();
            var result = uow.PersonRepository.GetAll(predicate: predicate);

            if (result.Exception == null)
                ItemsSource = new ObservableCollection<Person>(result.Data);
            else
                Billboard.ShowMessage(result.Message.MessageType, result.Message.Message);
        }
    }
}
