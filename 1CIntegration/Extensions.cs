using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace _1CIntegration
{
    public static class Extensions
    {
        public static bool IsNull(this object obj)
        {
            try
            {
                return obj == null;
            }
            catch (Exception e)
            {
                return true;
            }

        }

        public static double AsDouble(this object obj)
        {
            try
            {
                return Convert.ToDouble(obj.ToString());
            }
            catch (Exception e)
            {
                return 0.0;
            }

        }

        public static double AsDouble(this string str)
        {
            try
            {
                return Convert.ToDouble(str);
            }
            catch (Exception e)
            {
                return 0.0;
            }

        }

        public static int AsInteger(this object obj)
        {
            try
            {
                return Convert.ToInt32(obj.ToString());
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        public static int AsInteger(this string str)
        {
            try
            {
                return Convert.ToInt32(str);
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        public static object ToList(this DataTable dt)
        {
            try
            {
                return
                    (from DataRow dr in dt.Rows
                        select dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col]))
                        .ToList();
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}