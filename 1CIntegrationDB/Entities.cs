using System;
using System.Collections.Generic;

namespace _1CIntegrationDB
{
    public static class EntitiesMethods
    {
        public static List<Good> GetAllGood()
        {
            try
            {
                return SQLiteProvider.OpenSql("select * from goods").DataTableToList<Good>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Good GetGood(Int64 id)
        {
            try
            {
                var listGood = SQLiteProvider.OpenSql("select * from goods where good_id = " + id).DataTableToList<Good>();
                if (listGood.Count == 0) return null;
                if (listGood.Count == 1 && listGood[0].IsNull()) return null;
                return listGood[0];
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
