using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text_based_roguelike.Database
{
    internal class DatabaseServices
    {
        private static string connectionString;
        private static string table;
        private static string queryParameters;

        public static void DBConnectionCheck(string connectionString)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Sikeres csatlakozás a DB-hez");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sikertelen kapcsolódás");
                Console.WriteLine(ex);
            }
        }
        public static DataTable GetAllData(string tableName, string connectionString)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            using var command = new MySqlCommand($"SELECT * FROM {tableName}", connection);
            using var reader = command.ExecuteReader();
            var dataTable = new DataTable();
            dataTable.Load(reader);

            return dataTable;
        }
    }
}
