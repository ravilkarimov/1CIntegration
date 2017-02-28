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
        public JsonResult GetGroups()
        {
            try
            {
                var sql = "select group_id, group_name from groups order by group_name";

                return Json(SQLiteProvider.OpenSql(sql).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception eGroups)
            {
                throw;
            }
            
        }

        // GET: /Store/getsizesgood
        public JsonResult GetSizesGood(string id)
        {
            try
            {
                var sql = "select distinct size from offers where good_key = '" + id + "' order by size ";

                return Json(SQLiteProvider.OpenSql(sql).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception eSize)
            {
                throw;
            }
        }

        // GET: /Store/getsizes
        public JsonResult GetSizes()
        {
            try
            {
                //"XS" (eXtra small - очень маленький), 
                //"S" (small - маленький), 
                //"M" (medium - средний), 
                //"L"  (large - большой размер), 
                //"XL" (eXtra Large - очень большой), 
                //"XXL" (eXtra eXtra Large - супер-большой), 
                //"XXXL" и так далее (подробнее - в таблице).
                var dictSizes = new Dictionary<string, int>
                {
                    {"XS", 100},
                    {"S", 200},
                    {"M", 300},
                    {"L", 400},
                    {"XL", 500},
                    {"XXL", 600},
                    {"2XL", 700},
                    {"XXXL", 800},
                    {"3XL", 900},
                    {"XXXXL", 1000},
                    {"4XL", 1100}
                };

                var dtGroups = SQLiteProvider.OpenSql("select distinct g.group_id, o.size from goods g, offers o where o.good_key = g.good_key");
                var dictGroups = dtGroups.AsEnumerable()
                    .Select(x => new
                    {
                        group_id = x["group_id"],
                        size = x["size"].ToString().ToUpper()
                    })
                    .GroupBy(x => x.size)
                    .ToDictionary(x => x.Key, y => y.Select(z => z.group_id).FirstOrDefault());

                var dt = SQLiteProvider.OpenSql("select distinct size from offers where size <> ''");
                var listSizes = dt.AsEnumerable()
                    .Select(x => new
                    {
                        size = x["size"].ToString().ToUpper()
                    })
                    .Select(x => new
                    {
                        x.size,
                        ordering = dictSizes.Keys.Any(y => y == x.size) ? dictSizes[x.size] : x.size.AsInteger(),
                        group_id = dictGroups.Keys.Any(y => y == x.size) ? dictGroups[x.size] : x.size.AsInteger()
                    })
                    .OrderBy(x => x.ordering)
                    .Select(x => new {x.group_id, x.size})
                    .ToList();

                return Json(listSizes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception eSize)
            {
                throw;
            }
        }

    }
}
