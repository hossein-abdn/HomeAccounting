using System.Windows;
using System.Windows.Input;
using Infra.Wpf.Mvvm;

namespace HomeAccounting.UI.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            Messenger.Default.Register<string>(CloseWindow, "LoginWindow_CloseWindow");
        }

        private void CloseWindow(string obj)
        {
            var mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
