using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace _1CIntegrationDB
{
    public class SQLiteProvider
    {
        private static readonly string DatabaseName = "C:\\Users\\Дмитрий\\db.sqlite";
        //C:\\Users\\r.karimov\\Downloads\\db.sqlite
        //C:\\Users\\Дмитрий\\db.sqlite

        static SQLiteProvider()
        {
            if (!File.Exists(DatabaseName))
            {
                SQLiteConnection.CreateFile(DatabaseName);
            }
        }

        public static void DoSql(string sql)
        {
            try
            {
                SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0}; Version=3;", DatabaseName));
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
            catch (SQLiteException)
            {
            }
        }

        public static DataTable OpenSql(string sqlString)
        {
            try
            {
                SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", DatabaseName));
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(sqlString, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                connection.Close();
                return dt;
            }
            catch (SQLiteException e)
            {
                throw;
            }
        }

        public static int ExecSql(string sqlString)
        {
            try
            {
                SQLiteConnection connection = new SQLiteConnection(string.Format("Data Source={0};", DatabaseName));
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(sqlString, connection);
                var res = command.ExecuteNonQuery();
                connection.Close();

                return res;
            }
            catch (SQLiteException e)
            {
                throw;
            }
        }
    }
}
