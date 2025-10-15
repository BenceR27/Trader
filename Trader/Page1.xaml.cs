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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        private readonly DatabaseStatements db = new DatabaseStatements();
        private readonly  MainWindow _mainWindow;

        public Page1(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            if (passwordPassbox.Password == passwordPassboxAg.Password)
            {
                var user = new
                {
                    UserName = usernameTextbox.Text,
                    UserPassword = passwordPassbox.Password,
                    Fullname = fullnameTextbox.Text,
                    Salt = "",
                    Email = emailTextbox.Text
                };

                MessageBox.Show(db.AddNewUser(user).ToString());
                _mainWindow.StartWindow.Navigate(new Login(_mainWindow));
            }
            else
            {
                MessageBox.Show("Eltérő jelszavak");
            }
        }
    }
}

