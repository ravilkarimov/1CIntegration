using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _1CIntegrationDB;

namespace _1CIntegration.Controllers
{
    public class StoreController : Controller
    {
        //
        // GET: /Store/

        public ActionResult Index()
        {
            return View();
        }

        // GET: /Store/getgroups
        public object GetGroups()
        {
            return null;
        }

        // GET: /Store/getsizes
        public object GetSizes(decimal id)
        {
            return null;
        }

    }
}
