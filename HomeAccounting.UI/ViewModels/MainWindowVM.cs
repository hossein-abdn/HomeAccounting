using HomeAccounting.UI.ViewModels.BaseInfo;
using Infra.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeAccounting.UI.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        #region Properties

        public RelayCommand PersonListViewCommand { get; set; }

        public RelayCommand LabelListViewCommand { get; set; }

        public RelayCommand TransactionGroupListViewCommand { get; set; }

        public RelayCommand LoadedEventCommand { get; set; }

        #endregion

        #region Methods

        public MainWindowVM()
        {
            PersonListViewCommand = new RelayCommand(PersonListViewExecute);
            LabelListViewCommand = new RelayCommand(LabelListViewExecute);
            TransactionGroupListViewCommand = new RelayCommand(TransactionGroupListViewExecute);
            LoadedEventCommand = new RelayCommand(LoadedEventExecute);
        }

        private void TransactionGroupListViewExecute()
        {
            NavigationService.NavigateTo(new TransactionGroupListVM());
        }

        private void LoadedEventExecute()
        {
            NavigationService = new NavigationService();
        }

        private void PersonListViewExecute()
        {
            NavigationService.NavigateTo(new PersonListVM());
        }

        private void LabelListViewExecute()
        {
            NavigationService.NavigateTo(new LabelListVM());
        }

        #endregion
    }
}
