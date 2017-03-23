using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace _1CIntegrationDB
{
    public class SQLiteProvider
    {
        private static readonly string DatabaseName = ConfigurationManager.AppSettings["DbPath"];
        //C:\\Users\\r.karimov\\Downloads\\db.sqlite
        //C:\\Users\\Дмитрий\\db.sqlite
        //h:\\root\\home\\djinaroshop-001\\www\\db\\db.sqlite

        private static readonly string ConnectionString = string.Format("Data Source={0}; Version=3; Compress=True; UseUTF16Encoding=True; Pooling=True; Max Pool Size=1000;", DatabaseName);

        static SQLiteProvider()
        {
            try
            {
                if (!File.Exists(DatabaseName))
                {
                    try
                    {
                        SQLiteConnection.CreateFile(DatabaseName);
                    }
                    catch (SQLiteException eLite)
                    {
                        throw eLite;
                    }
                }
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        public static void DoSql(string sql)
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException)
            {
            }
        }

        public static SQLiteConnection GetConnection()
        {
            try
            {
                return new SQLiteConnection(ConnectionString);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static DataTable OpenSql(string sqlString, SQLiteConnection con)
        {
            try
            {
                con.OpenAsync();
                using (SQLiteCommand cmd = new SQLiteCommand(sqlString, con))
                {
                    var dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    return dt;
                }
            }
            catch (Exception e)
            {
                new FileLogger("Log.txt").LogMessage("Ошибка при OpenSQL (" + sqlString + "): " + e.Message);
                throw new Exception("Ошибка при OpenSQL (" + sqlString + "): " + e.Message);
            }
        }

        public static DataTable OpenSql(string sqlString)
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
                { 
                    c.OpenAsync();
                    using (SQLiteCommand cmd = new SQLiteCommand(sqlString, c))
                    {
                        var dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());
                        return dt;
                    }
                }
            }
            catch (Exception e)
            {
                new FileLogger("Log.txt").LogMessage("Ошибка при OpenSQL (" + sqlString + "): " + e.Message);
                throw new Exception("Ошибка при OpenSQL (" + sqlString + "): " + e.Message);
            }
        }

        public static int ExecSql(string sqlString)
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
                {
                    c.OpenAsync();
                    using (SQLiteCommand cmd = new SQLiteCommand(sqlString, c))
                    {
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException e)
            {
                throw new Exception("Ошибка при ExecSQL ("+sqlString+"): " + e.Message);
            }
        }

        public static int ExecSql(List<string> listSql)
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(ConnectionString))
                {
                    c.OpenAsync();
                    using (var cmd = new SQLiteCommand(c))
                    {
                        cmd.CommandText = string.Join("; ", listSql);
                        cmd.ExecuteNonQuery();
                    }
                    c.Close();
                }

                return 1;
            }
            catch (SQLiteException e)
            {
                throw new Exception("Ошибка при ExecSQL(List): " + e.Message);
            }
        }
    }
}
