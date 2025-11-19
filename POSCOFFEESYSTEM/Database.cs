using System;
using System.Data.SqlClient;
using System.Configuration;

namespace POSCOFFEESYSTEM
{
    public class Database
    {
        private readonly string connectionString;

        public Database(string connectionName = "CafeDB")
        {
            var settings = ConfigurationManager.ConnectionStrings[connectionName];
            if (settings == null)
                throw new ConfigurationErrorsException($"Connection string '{connectionName}' not found in application configuration.");

            connectionString = settings.ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ConfigurationErrorsException($"Connection string '{connectionName}' is empty or whitespace in application configuration.");
        }

        // Returns a NEW connection every time
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // Optional helpers if you still want to manage a single connection
        public void OpenConnection(SqlConnection con)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
        }

        public void CloseConnection(SqlConnection con)
        {
            if (con.State != System.Data.ConnectionState.Closed)
                con.Close();
        }
    }
}
