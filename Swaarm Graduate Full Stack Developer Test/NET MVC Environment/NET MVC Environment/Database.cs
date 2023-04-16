using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            string columns = string.Join(",", Enumerable.Range(0, values.Length).Select(i => $"@param{i}"));
            string query = $"INSERT INTO {tableName} VALUES ({columns})";
            SqlCommand command = new SqlCommand(query, _connection);
            for(int i = 0; i < values.Length; i++)
            {
                command.Parameters.AddWithValue($"@param{i}", values[i]);
            }
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

        public List<Dictionary<string, object>> GetAllData(string tableName)
        {
            string query = $"SELECT * FROM {tableName}";
            SqlDataAdapter adapter = new SqlDataAdapter(query, _connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, tableName);

            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            foreach(DataRow row in dataSet.Tables[tableName].Rows)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                foreach(DataColumn col in dataSet.Tables[tableName].Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                result.Add(dict);
            }

            return result;
        }

        public void RemoveData(string tableName, string condition)
        {
            string query = $"DELETE FROM {tableName} WHERE {condition}";
            SqlCommand command = new SqlCommand(query, _connection);
            command.ExecuteNonQuery();
        }
    }
}
