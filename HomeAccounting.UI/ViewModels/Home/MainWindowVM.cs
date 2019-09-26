using HomeAccounting.Business;
using Infra.Wpf.Common;
using Infra.Wpf.Mvvm;
using Infra.Wpf.Security;
using System.Threading;

namespace HomeAccounting.UI.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        #region Properties

        public RelayCommand PersonListViewCommand { get; set; }

        public RelayCommand LabelListViewCommand { get; set; }

        public RelayCommand TransactionGroupListViewCommand { get; set; }

        public RelayCommand LoadedEventCommand { get; set; }

        public RelayCommand ClosedEventCommand { get; set; }

        #endregion

        #region Methods

        public MainWindowVM()
        {
            PersonListViewCommand = new RelayCommand(PersonListViewExecute);
            LabelListViewCommand = new RelayCommand(LabelListViewExecute);
            TransactionGroupListViewCommand = new RelayCommand(TransactionGroupListViewExecute);
            LoadedEventCommand = new RelayCommand(LoadedEventExecute);
            ClosedEventCommand = new RelayCommand(ClosedEventExecute);
        }

        private void ClosedEventExecute()
        {
            ILogger logger = new Logger(AccountingUow.ConnectionString);
            logger.Log(new LogInfo
            {
                CallSite = this.GetType().FullName,
                LogType = LogType.Information,
                UserId = ((Identity) Thread.CurrentPrincipal.Identity).Id,
                Message = "Logout: UserName = " + (Thread.CurrentPrincipal.Identity as Identity).Name
            });
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
