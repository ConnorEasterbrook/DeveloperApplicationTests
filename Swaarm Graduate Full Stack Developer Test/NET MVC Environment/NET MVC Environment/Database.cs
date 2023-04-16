using System;
using System.Data;
using System.Data.SqlClient;

namespace NET_MVC_Environment
{
    public class Database
    {
        private SqlConnection _connection;

        public Database(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            _connection.Open();
        }

        public void CloseConnection()
        {
            if (_connection != null)
            {
                _connection.Close();
            }
        }

        public void AddTable(string tableName, string[] columns)
        {
            string query = $"CREATE TABLE {tableName} ({string.Join(",", columns)})";
            SqlCommand command = new SqlCommand(query, _connection);
            command.ExecuteNonQuery();
        }

        public bool TableExists(string tableName)
        {
            string query = $"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";
            SqlDataAdapter adapter = new SqlDataAdapter(query, _connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "Tables");
            return dataSet.Tables["Tables"].Rows.Count > 0;
        }

        public void RemoveTable(string tableName)
        {
            string query = $"DROP TABLE {tableName}";
            SqlCommand command = new SqlCommand(query, _connection);
            command.ExecuteNonQuery();
        }

        public void AddData(string tableName, string[] values)
        {
            string query = $"INSERT INTO {tableName} VALUES ({string.Join(",", values)})";
            SqlCommand command = new SqlCommand(query, _connection);
            command.ExecuteNonQuery();
        }

        public DataRow SelectRow(string tableName, string condition)
        {
            string query = $"SELECT * FROM {tableName} WHERE {condition}";
            SqlDataAdapter adapter = new SqlDataAdapter(query, _connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, tableName);

            if (dataSet.Tables[tableName].Rows.Count > 0)
            {
                return dataSet.Tables[tableName].Rows[0];
            }
            else
            {
                return null;
            }
        }

        public void RemoveData(string tableName, string condition)
        {
            string query = $"DELETE FROM {tableName} WHERE {condition}";
            SqlCommand command = new SqlCommand(query, _connection);
            command.ExecuteNonQuery();
        }
    }
}
