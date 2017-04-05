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

        private static string GetConnectionString(string name)
        {
            return WebConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public static DataTable OpenSql(string sqlString)
        {
            try
            {
                var inv = new DataTable();
                using (SqlConnection connect = new SqlConnection(GetConnectionString("DefaultConnection")))
                {
                    connect.Open();
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
                        connect.Close();
                    }
                    return inv;
                }
            }
            catch (SqlException e)
            {
                Thread.Sleep(500);
                return OpenSql(sqlString);
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
                decimal identity = 0;
                using (SqlConnection connect = new SqlConnection(GetConnectionString("DefaultConnection")))
                {
                    connect.Open();
                    sqlString += " SELECT SCOPE_IDENTITY()";
                    using (var cmd = new SqlCommand(sqlString, connect))
                    {
                        using (var transaction = connect.BeginTransaction(IsolationLevel.RepeatableRead))
                        {
                            try
                            {
                                lock (thisLock)
                                {
                                    cmd.Transaction = transaction;
                                    identity = (decimal)cmd.ExecuteScalar();
                                    transaction.Commit();
                                }
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
                        connect.Close();
                    }
                    return identity;
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
                using (SqlConnection connect = new SqlConnection(GetConnectionString("DefaultConnection")))
                {
                    connect.Open();
                    using (var cmd = new SqlCommand(sqlString, connect))
                    {
                        lock (thisLock)
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    connect.Close();
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
                using (SqlConnection connect = new SqlConnection(GetConnectionString("DefaultConnection")))
                {
                    connect.Open();
                    using (var cmd = new SqlCommand(string.Join("; ", listSql), connect))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    connect.Close();
                }
            }
            catch (Exception e)
            {
                new FileLogger("Log.txt").LogMessage(e.Message);
            }
        }

        public static void DoSql(string sqlString)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(GetConnectionString("DefaultConnection")))
                {
                    connect.Open();
                    using (var cmd = new SqlCommand(sqlString, connect))
                    {
                        lock (thisLock)
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    connect.Close();
                }
            }
            catch (Exception e)
            {
                new FileLogger("Log.txt").LogMessage(e.Message);
            }
        }
    }
}
