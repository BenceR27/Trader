using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader
{
    internal class DatabaseStatements
    {
        Connect conn = new Connect();

        public object AddNewUser(object user)
        {
            try
            {
            conn._connection.Open();

            string sql= "INSERT INTO `users`( `UserName`, `FullName`, `Password`, `Salt`, `Email`) " +
                "VALUES(@username, @fullname, @password, @salt, @email";

            MySqlCommand cmd = new MySqlCommand(sql, conn._connection);

                var newuser = user.GetType().GetProperties();
    
            cmd.Parameters.AddWithValue("@username", newuser[0].GetValue(user));
            cmd.Parameters.AddWithValue("@fullname", newuser[1].GetValue(user));
            cmd.Parameters.AddWithValue("@password", newuser[2].GetValue(user));
            cmd.Parameters.AddWithValue("@salt", newuser[3].GetValue(user));
            cmd.Parameters.AddWithValue("@email", newuser[4].GetValue(user));

            cmd.ExecuteNonQuery();

            conn._connection.Close();

            return new { message = "Siketes hozzáadás." };

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}