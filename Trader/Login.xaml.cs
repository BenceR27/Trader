using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Trader
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private readonly DatabaseStatements _databaseStatements = new DatabaseStatements();
        private readonly MainWindow _mainWindow;
        public Login(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void logButton_Click(object sender, RoutedEventArgs e)
        {
            var user = new
            {
                Name = usernameTextbox.Text,
                Pass = passwordPassbox.Password
            };
            MessageBox.Show(_databaseStatements.LoginUser(user).ToString());
        }

        private void regLink_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.StartWindow.Navigate(new Page1(_mainWindow));
        }
    }
}
