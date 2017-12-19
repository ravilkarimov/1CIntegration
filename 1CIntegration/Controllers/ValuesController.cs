using System;
using System.Web.Http;
using _1CIntegrationDB;
using System.Data;
using System.Linq;

namespace _1CIntegration.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public object Get()
        {
            try
            {
                string sql =
                    " SELECT gr.group_id, gr.group_name, g.good_id, g.good, g.good_key, g.img_path, MAX(o.price) as price, (CASE WHEN o.amount > 0 THEN 'Есть в наличии' ELSE 'Нет в наличии' END) as amount " +
                    " FROM goods g, groups gr, offers o " +
                    " WHERE 1 = 1 " +
                    " AND g.brand_id != 3 AND g.good not like '%NB%'" +
                    " AND g.group_id = gr.group_id " +
                    " AND g.good_key = o.good_key " +
                    " AND g.group_id = 1 " +
                    " GROUP BY 1,2,3,4,5 " +
                    " ORDER BY price, feature " +
                    " LIMIT 18 OFFSET 0 ";
                var dt = SQLiteProvider.OpenSql(sql);

                return (from DataRow dr in dt.Rows select dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col])).ToList();
            }
            catch (Exception error)
            {
                throw error;
            }
        }


        // GET api/values/5
        public object Get(int id)
        {
            try
            {
                string sql =
                    " SELECT gr.group_id, gr.group_name, g.good_id, g.good, g.img_path, MAX(o.price) as price, (CASE WHEN o.amount > 0 THEN 'Есть в наличии' ELSE 'Нет в наличии' END) as amount " +
                    " FROM goods g, groups gr, offers o " +
                    " WHERE 1 = 1 " +
                    " AND g.brand_id != 3 AND g.good not like '%NB%'" +
                    " AND g.group_id = gr.group_id " +
                    " AND g.good_key = o.good_key " +
                    " AND g.group_id = 1 " +
                    " GROUP BY 1,2,3,4,5 " +
                    " ORDER BY good_id, feature " +
                    " LIMIT 18 OFFSET 0 ";
                var dt = SQLiteProvider.OpenSql(sql);

                return (from DataRow dr in dt.Rows select dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col])).ToList();
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        public void TestDB()
        {
        }
    }
}