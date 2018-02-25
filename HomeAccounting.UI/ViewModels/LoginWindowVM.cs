using HomeAccounting.Business;
using HomeAccounting.DataAccess.Models;
using Infra.Wpf.Business;
using Infra.Wpf.Mvvm;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HomeAccounting.UI.ViewModels
{
    public class LoginWindowVM : ViewModelBase<User>
    {
        #region Properties

        public string UserName
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Password
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public RelayCommand<ArgsParameter<RoutedEventArgs>> SetPasswordCommand { get; set; }

        public RelayCommand LoginCommand { get; set; }

        public RelayCommand CancelCommand { get; set; }

        public RelayCommand<ArgsParameter<KeyEventArgs>> KeyDownCommand { get; set; }

        #endregion

        #region Methods

        public LoginWindowVM()
        {
            SetPasswordCommand = new RelayCommand<ArgsParameter<RoutedEventArgs>>(SetPasswordExecute);
            LoginCommand = new RelayCommand(LoginExecute);
            CancelCommand = new RelayCommand(CancelExecute);
            KeyDownCommand = new RelayCommand<ArgsParameter<KeyEventArgs>>(KeyDownExecute);
        }

        private void KeyDownExecute(ArgsParameter<KeyEventArgs> obj)
        {
            if (obj.e.Key == Key.Enter)
                LoginCommand.Execute(null);
        }

        private void CancelExecute()
        {
            Application.Current.MainWindow.Close();
        }

        private void LoginExecute()
        {
            LoginBusiness business = new LoginBusiness(UserName, Password, new Logger("AccountingContext"));
            business.Execute();

            if (business.Result.IsOnExecute)
                Messenger.Default.Send("Close", "LoginWindow_CloseWindow");
            else
                ShowMessageBox(business.Result.Message.Message, business.Result.Message.Title, Infra.Wpf.Controls.MsgButton.OK, Infra.Wpf.Controls.MsgIcon.Error);
        }

        private void SetPasswordExecute(ArgsParameter<RoutedEventArgs> obj)
        {
            Password = ((PasswordBox) obj.sender).Password;
        }

        #endregion
    }
}
