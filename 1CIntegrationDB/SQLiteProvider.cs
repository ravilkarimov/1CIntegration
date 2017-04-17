using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Web.Configuration;

namespace _1CIntegrationDB
{
    public class SQLiteProvider
    {
        private static Object thisLock = new Object();
        private static SqlConnection connect;

        private static string GetConnectionString(string name)
        {
            return WebConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        private static void Connection()
        {
            if (connect == null)
            {
                connect = new SqlConnection(GetConnectionString("DefaultConnection"));
                connect.Open();
            }
            else if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
        }

        public static DataTable OpenSql(string sqlString)
        {
            try
            {
                var inv = new DataTable();
                Connection();
                using (SqlCommand cmd = new SqlCommand(sqlString, connect))
                {
                    cmd.CommandTimeout = 300;
                    try
                    {
                        SqlDataReader dr = cmd.ExecuteReader();
                        inv.Load(dr);
                        dr.Close();
                    }
                    catch (SqlException e)
                    {
                        throw e;
                    }
                    catch (Exception e1)
                    {
                        throw e1;
                    }
                }
                return inv;
            }
            catch (Exception et2)
            {
                new FileLogger("Log.txt").LogMessage(et2.Message);
                return null;
            }
        }

        public static decimal InsertSqlScopeID(string sqlString)
        {
            try
            {
                lock (thisLock)
                {
                    Connection();
                    sqlString += " SELECT SCOPE_IDENTITY()";
                    using (var cmd = new SqlCommand(sqlString, connect))
                    {
                        decimal identity = 0;
                        using (var transaction = connect.BeginTransaction(IsolationLevel.RepeatableRead))
                        {
                            try
                            {

                                cmd.Transaction = transaction;
                                identity = (decimal) cmd.ExecuteScalar();
                                transaction.Commit();
                            }
                            catch (SqlException eT)
                            {
                                transaction.Rollback();
                                throw eT;
                            }
                            catch (Exception eT1)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                        return identity;
                    }

                }
            }
            catch (SqlException e)
            {

                if (e.Number == -2 || e.Number == 40)
                {
                    return InsertSqlScopeID(sqlString);
                }
                else if (e.Number == 1205)
                {
                    Thread.Sleep(10000);
                    return InsertSqlScopeID(sqlString);
                }
                return InsertSqlScopeID(sqlString);
            }
            catch (Exception e1)
            {
                new FileLogger("Log.txt").LogMessage(e1.Message);
                return 0;
            }
        }


        public static void ExecSql(string sqlString)
        {
            try
            {
                Connection();
                using (var cmd = new SqlCommand(sqlString, connect))
                {
                    lock (thisLock)
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                if (e.Number == -2 || e.Number == 40)
                {
                    ExecSql(sqlString);
                }
                else if (e.Number == 1205)
                {
                    Thread.Sleep(10000);
                    ExecSql(sqlString);
                }
                throw;
            }
            catch (Exception e1)
            {
                new FileLogger("Log.txt").LogMessage(e1.Message);
            }
        }

        public static void ExecSql(List<string> listSql)
        {
            try
            {
                Connection();
                using (var cmd = new SqlCommand(string.Join("; ", listSql), connect))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException e)
            {
                new FileLogger("Log.txt").LogMessage(e.Message + " , " + e.Number);
            }
        }

        public static void DoSql(string sqlString)
        {
            try
            {
                Connection();
                using (var cmd = new SqlCommand(sqlString, connect))
                {
                    lock (thisLock)
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                new FileLogger("Log.txt").LogMessage(e.Message);
            }
        }
    }
}
