using C1.WPF.Toolbar;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace HomeAccounting.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        C1ToolbarButton selectedButton;

        public MainWindow()
        {
            InitializeComponent();

            CultureInfo calture = new CultureInfo("fa-IR");
            calture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
            Thread.CurrentThread.CurrentCulture = calture;
            Thread.CurrentThread.CurrentUICulture = calture;

            C1.WPF.Theming.C1Theme.ApplyTheme(toolbar, new C1.WPF.Theming.Office2013.C1ThemeOffice2013LightGray());
        }

        private void C1ToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            C1ToolbarButton senderButton = (sender as C1ToolbarButton);

            if (senderButton == selectedButton)
                return;

            if (selectedButton != null)
                selectedButton.SetValue(C1ToolbarButton.LargeImageSourceProperty, Infra.Wpf.Common.Helpers.AttachImage.GetImage(selectedButton));

            senderButton.SetValue(C1ToolbarButton.LargeImageSourceProperty, Infra.Wpf.Common.Helpers.AttachImage.GetImageOver(senderButton));
            selectedButton = senderButton;
        }
    }
}
