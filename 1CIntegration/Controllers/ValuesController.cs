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
                    "SELECT gr.group_id, gr.group_name, g.good_id, g.good, g.img_path, o.feature, o.price, o.currency, o.amount " +
                    "FROM goods g " +
                    "LEFT OUTER JOIN offers o ON g.good_key = o.good_key " +
                    "LEFT OUTER JOIN groups gr ON g.group_id = gr.group_id " +
                    "ORDER BY price DESC, feature";
                //"WHERE g.img_path IS NOT NULL AND g.img_path <> '' ";
                var dt = SQLiteProvider.OpenSql(sql);

                return (from DataRow dr in dt.Rows select dt.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => dr[col])).ToList();
            }
            catch (Exception error)
            {
                throw error;
            }
        }


        // GET api/values/5
        public string Get(int id)
        {
            return "value";
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