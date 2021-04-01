using System.Windows;
using HelloLibrary;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var name = textBox.Text;

            var printLine = WelcomeMessage.GetWelcomeMessage(name);

            MessageBox.Show(printLine);
        }
    }
}
