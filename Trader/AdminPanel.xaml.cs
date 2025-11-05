using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Trader
{
    public partial class AdminPanel : Page
    {
        private readonly DatabaseStatements db = new DatabaseStatements();

        public AdminPanel()
        {
            InitializeComponent();
            LoadUserList();
        }

        private void LoadUserList()
        {
            userDataGrid.ItemsSource = db.GetUserList();
        }

        private void userDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (userDataGrid.SelectedItem is DataRowView row)
            {
                int userId = Convert.ToInt32(row["Id"]);
                string userName = row["UserName"].ToString();

                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to delete user '{userName}'?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    db.DeleteUser(userId);
                    LoadUserList(); 
                }
            }
        }
    }
}
