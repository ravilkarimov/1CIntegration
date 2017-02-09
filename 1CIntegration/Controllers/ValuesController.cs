using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Script.Serialization;
using _1CIntegrationDB;

namespace _1CIntegration.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public object Get()
        {
            var o = new List<string>() {"dfgfbgvdf", "adgailfugf"};
            //var json = new JavaScriptSerializer().Serialize(o);

            DBProgram db_program = new DBProgram();

            return o;
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