using NET_MVC_Environment.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NET_MVC_Environment
{
    public class Database
    {
        private SqlConnection _connection;

        public static bool desc;
        public static int element;

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
            if(_connection != null)
            {
                _connection.Close();
            }
        }

        public void SetVariables(bool descend, int el)
        {

            Database.desc = descend;
            Database.element = el;
        }

        public List<Data> GetDataForList(string tableName)
        {
            List<Data> data = new List<Data>();

            string query = $"SELECT * FROM {tableName}";
            using(SqlCommand command = new SqlCommand(query, _connection))
            {
                using(SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        data.Add(new Data()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            UploadDate = reader.GetDateTime(2),
                            LastUpdatedDate = reader.GetDateTime(3)
                        });
                    }
                }
            }

            return data;
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

        public void AddData(string tableName, string dataName, string date)
        {
            string query = $"INSERT INTO {tableName} (Name, CreatedDate, LastUpdatedDate) VALUES (@Name, @CreatedDate, @LastUpdatedDate)";

            using(SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@Name", dataName);
                command.Parameters.AddWithValue("@CreatedDate", date);
                command.Parameters.AddWithValue("@LastUpdatedDate", date);
                command.ExecuteNonQuery();
            }
        }

        public DataRow SelectRow(string tableName, string condition)
        {
            string query = $"SELECT * FROM {tableName} WHERE {condition}";
            SqlDataAdapter adapter = new SqlDataAdapter(query, _connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, tableName);

            if(dataSet.Tables[tableName].Rows.Count > 0)
            {
                return dataSet.Tables[tableName].Rows[0];
            }
            else
            {
                return null;
            }
        }

        public void UpdateData(string tableName, string name, string newName, string date)
        {
            string query = $"UPDATE {tableName} SET Name = @Name, LastUpdatedDate = @LastUpdatedDate WHERE Name = '{name}'";

            using(SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@Name", newName);
                command.Parameters.AddWithValue("@LastUpdatedDate", date);
                command.ExecuteNonQuery();
            }
        }

        public List<Dictionary<string, object>> GetAllData(string tableName)
        {
            string query = $"SELECT * FROM {tableName}";

            if (desc)
            {
                Debug.WriteLine("\n\n" + Database.desc + " | " + Database.element + "\n\n");
               /* query = $"SELECT * FROM {tableName} ORDER BY Name DESC";*/

                if(element == 1)
                {
                    Debug.WriteLine("desc");
                    query = $"SELECT * FROM {tableName} ORDER BY Name DESC";
                }
                else if(element == 2)
                {
                    query = $"SELECT * FROM {tableName} ORDER BY CreatedDate DESC";
                }
                else if(element == 3)
                {
                    query = $"SELECT * FROM {tableName} ORDER BY LastUpdatedDate DESC";
                }
            }
            else
            {
                Debug.WriteLine("\n\n" + desc + " | " + element + "\n\n");
                /*query = $"SELECT * FROM {tableName} ORDER BY Name ASC";*/

                if(element == 1)
                {
                    query = $"SELECT * FROM {tableName} ORDER BY Name ASC";
                }
                else if(element == 2)
                {
                    query = $"SELECT * FROM {tableName} ORDER BY CreatedDate ASC";
                }
                else if(element == 3)
                {
                    query = $"SELECT * FROM {tableName} ORDER BY LastUpdatedDate ASC";
                }
            }

            SqlDataAdapter adapter = new SqlDataAdapter(query, _connection);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, tableName);

            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

            foreach(DataRow row in dataSet.Tables[tableName].Rows)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();

                for(int i = 1; i < dataSet.Tables[tableName].Columns.Count; i++)
                {
                    dict[dataSet.Tables[tableName].Columns[i].ColumnName] = row[i];
                }

                result.Add(dict);
            }

            return result;
        }

        public void RemoveData(string tableName, string name = "")
        {
            string query = "";
            if(name != "")
            {
                query = $"DELETE FROM {tableName} WHERE Name = '{name}'";
            }
            else
            {
                query = $"DELETE FROM {tableName}";
            }

            SqlCommand command = new SqlCommand(query, _connection);
            command.ExecuteNonQuery();
        }
    }
}
