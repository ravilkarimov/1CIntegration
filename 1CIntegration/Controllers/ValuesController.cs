using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Script.Serialization;
using _1CIntegrationDB;
using System.Data;

namespace _1CIntegration.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public object Get()
        {
            string sql = "SELECT * FROM goods";
            DataTable dt = new DataTable();
            dt = SQLiteProvider.OpenSql(sql);
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
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
            //return rows;
            return serializer.Serialize(rows);
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