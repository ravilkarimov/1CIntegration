using System;
using System.Collections.Generic;
using System.Web.Http;
using _1CIntegrationDB;
using System.Data;

namespace _1CIntegration.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public Object Get()
        {
            string sql = "SELECT gr.group_id, gr.group_name, g.good_id, g.good, g.img_path, o.feature, o.price, o.currency, o.amount  FROM goods g " +
                         "LEFT OUTER JOIN offers o ON g.good_key = o.offer_key " +
                         "LEFT OUTER JOIN groups gr ON g.group_id = gr.group_id " +
                         "WHERE g.img_path IS NOT NULL AND g.img_path <> '' ";
            DataTable dt = new DataTable();
            dt = SQLiteProvider.OpenSql(sql);
            //System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }

            //var o = new List<string>() {"dfgfbgvdf", "adgailfugf"};
            //var json = new JavaScriptSerializer().Serialize(o);
            return rows;
            //return serializer.Serialize(rows);
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