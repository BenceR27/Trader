using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Trader
{
    internal class DatabaseStatements
    {
        // Assuming you already have a Connect class that provides conn._connection
        private readonly Connect conn = new Connect();

        public object AddNewUser(object user)
        {
            try
            {
                conn._connection.Open();

                var newUser = user.GetType().GetProperties();

                string salt = GenerateSalt();
                string passwordHash = ComputeHmacSha256(newUser[2].GetValue(user).ToString(), salt);

                string sql = "INSERT INTO `users`(`UserName`, `FullName`, `Password`, `Salt`, `Email`) VALUES (@username, @fullname, @password, @salt, @email)";

                MySqlCommand cmd = new MySqlCommand(sql, conn._connection);

                cmd.Parameters.AddWithValue("@username", newUser[0].GetValue(user));
                cmd.Parameters.AddWithValue("@fullname", newUser[1].GetValue(user));
                cmd.Parameters.AddWithValue("@password", passwordHash);
                cmd.Parameters.AddWithValue("@salt", salt);
                cmd.Parameters.AddWithValue("@email", newUser[4].GetValue(user));

                cmd.ExecuteNonQuery();

                conn._connection.Close();

                return new { message = "Sikeres hozzáadás." };
            }
            catch (Exception ex)
            {
                if (conn._connection.State == ConnectionState.Open)
                    conn._connection.Close();

                return new { message = ex.Message };
            }
        }

        public object LoginUser(object user)
        {
            try
            {
                conn._connection.Open();
                string sql = "SELECT * FROM users WHERE UserName = @username AND Password = @password";
                MySqlCommand cmd = new MySqlCommand(sql, conn._connection);
                var logUser = user.GetType().GetProperties();

                cmd.Parameters.AddWithValue("@username", logUser[0].GetValue(user));
                cmd.Parameters.AddWithValue("@password", logUser[1].GetValue(user));

                MySqlDataReader reader = cmd.ExecuteReader();
                object isRegistered = reader.Read()
                    ? new { message = "Regisztrált" }
                    : new { message = "Nem regisztrált" };

                conn._connection.Close();
                return isRegistered;
            }
            catch (Exception ex)
            {
                if (conn._connection.State == ConnectionState.Open)
                    conn._connection.Close();

                return new { message = ex.Message };
            }
        }

        public DataView GetUserList()
        {
            try
            {
                conn._connection.Open();
                string sql = "SELECT * FROM users";

                MySqlCommand cmd = new MySqlCommand(sql, conn._connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
                return null;
            }
            finally
            {
                if (conn._connection.State == ConnectionState.Open)
                    conn._connection.Close();
            }
        }

        public void DeleteUser(int userId)
        {
            string query = "DELETE FROM users WHERE Id = @Id";

            try
            {
                conn._connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn._connection))
                {
                    cmd.Parameters.AddWithValue("@Id", userId);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        MessageBox.Show("User deleted successfully!");
                    else
                        MessageBox.Show("No user found with that ID.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting user: " + ex.Message);
            }
            finally
            {
                if (conn._connection.State == ConnectionState.Open)
                    conn._connection.Close();
            }
        }

        public string GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rnd = RandomNumberGenerator.Create())
            {
                rnd.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public string ComputeHmacSha256(string password, string salt)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
