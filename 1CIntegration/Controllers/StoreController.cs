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
                var sql = "SELECT gr.group_id, gr.group_name, COUNT(*) as count " +
                          "FROM groups gr, goods g " +
                          "WHERE gr.group_id = g.group_id " +
                          "AND gr.group_id in (1,2,3)  " +
                          "GROUP BY 1,2 " +
                          "ORDER BY group_name";

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
                var sql = "SELECT DISTINCT size FROM offers where good_key = '" + id + "' ORDER BY size DESC ";

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

        // GET: /Store/getshoes
        public JsonResult GetShoes(int groups, int sizes, int page)
        {
            try
            {
                string sql =
                    " SELECT gr.group_id, gr.group_name, g.good_id, g.good, g.good_key, g.img_path, " +
                    " MAX(o.price) as price, " +
                    " (CASE WHEN o.amount > 0 THEN 'Есть в наличии' ELSE 'Нет в наличии' END) as amount " +
                    " FROM goods g, groups gr, offers o " +
                    " WHERE 1 = 1 " +
                    " AND g.group_id = gr.group_id " +
                    " AND g.good_key = o.good_key " +
                    " AND g.group_id = " + groups + " " +
                    " GROUP BY 1,2,3,4,5 " +
                    " ORDER BY price, feature " +
                    " LIMIT 18 OFFSET "+ ((page * 18) - 18) +" ";
                var dt = SQLiteProvider.OpenSql(sql);

                return Json(dt.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        // GET: /Store/getshoescount
        public JsonResult GetShoesCount(int groups, int sizes)
        {
            try
            {
                string sql =
                    " SELECT count(distinct g.good_id) count " +
                    " FROM goods g, offers o " +
                    " WHERE 1 = 1 " +
                    " AND g.good_key = o.good_key " +
                    " AND g.group_id = " + groups;
                var dt = SQLiteProvider.OpenSql(sql);
                var countPage = Math.Ceiling((double)dt.Rows[0]["count"].ToString().AsInteger() / 18);
                dt.Rows[0]["count"] = countPage;
                return Json(dt.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                throw error;
            }
        }

    }
}
