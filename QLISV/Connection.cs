using System;
using Microsoft.Data.SqlClient;

namespace QLISV
{
    internal class Connection
    {
        private static string stringConnection = @"Data Source=localhost;Initial Catalog=QLISV;Integrated Security=True";

        public SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(stringConnection);
            return connection;
        }
    }
}

