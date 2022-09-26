using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPPMAC.ConnectDB
{
    public class Connect
    {
        public string connectionString { get; set; }
        public SqlConnection sqlConnection { get; set; }
        public void Connected()
        {
            sqlConnection = new SqlConnection(connectionString);

            if (sqlConnection != null && sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void DisConnected()
        {
            if (sqlConnection != null && sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public SqlDataReader Select(string sqlQuery)
        {
            SqlCommand command = new SqlCommand(sqlQuery, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            command.CommandTimeout = 0;
            command.Dispose();

            return reader;
        }

        public SqlCommand ExcuteStoreProceduce(string sqlquery)
        {
            SqlCommand sqlCommand = new SqlCommand(sqlquery, sqlConnection);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 0;
            sqlCommand.Clone();

            return sqlCommand;

        }

        public int ExcuteNonQuery(string sqlQuery)
        {
            SqlCommand command = new SqlCommand(sqlQuery, sqlConnection);
            return command.ExecuteNonQuery();
        }

        public Connect()
        {
            connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        }
    }
}
