using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using _1CIntegrationDB;
using System.IO;

namespace _1CIntegration.Controllers
{
    public class StoreController : AsyncController
    {
        //
        // GET: /Store/
        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Store/GetImgProduct?good_id=&width=&height=
        [HttpGet]
        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.Client)]
        public async Task<ActionResult> GetImgProductMin(string good_id)
        {
            return await Task<FileStreamResult>.Factory.StartNew(() =>
            {
                if (good_id.IsNullOrEmpty()) return null;

                var imgPath = SQLiteProvider.OpenSql("select img_path from goods where good_id = " + good_id).Rows[0]["img_path"].ToString();
 
                if (imgPath.IsNullOrEmpty()) return null;
                using (var fs = System.IO.File.OpenRead("h:/root/home/djinaroshop-001/www/webdata/" + imgPath.Replace(".jpg", "_min.jpg")))
                {
                    using (var image = Image.FromStream(fs, true))
                    {
                        //C:/Users/r.karimov/Downloads/Temp/webdata/
                        //h:/root/home/djinaroshop-001/www/webdata/
                        return new FileStreamResult(image.ToStream(ImageFormat.Jpeg), "image/jpeg");
                    }
                }
            });
        }

        // GET: /Store/GetImgProduct?good_id=&width=&height=
        [HttpGet]
        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.Client)]
        public async Task<ActionResult> GetImgProduct(string good_id)
        {
            return await Task<FileStreamResult>.Factory.StartNew(() =>
            {
                if (good_id.IsNullOrEmpty()) return null;

                var imgPath = SQLiteProvider.OpenSql("select img_path from goods where good_id = " + good_id).Rows[0]["img_path"].ToString();

                if (imgPath.IsNullOrEmpty()) return null;
                using (var fs = System.IO.File.OpenRead("h:/root/home/djinaroshop-001/www/webdata/" + imgPath))
                {
                    using (var image = Image.FromStream(fs, true))
                    {
                        //C:/Users/r.karimov/Downloads/Temp/webdata/
                        //h:/root/home/djinaroshop-001/www/webdata/
                        return new FileStreamResult(image.ToStream(ImageFormat.Jpeg), "image/jpeg");
                    }
                }
            });
        }

        // GET: /Store/getgroups
        [HttpGet]
        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
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

        // GET: /Store/getbrands
        [HttpGet]
        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public JsonResult GetBrands()
        {
            try
            {
                var sql = "SELECT b.brand_id, b.brand, COUNT(*) as count " +
                          "FROM d_brands b, goods g " +
                          "WHERE b.brand_id = g.brand_id " +
                          "AND g.group_id in (1,2,3)  " +
                          "GROUP BY 1,2 " +
                          "ORDER BY brand";

                return Json(SQLiteProvider.OpenSql(sql).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception eBrands)
            {
                throw;
            }

        }

        // GET: /Store/getsizesgood
        [HttpGet]
        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.Client)]
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
        [HttpGet]
        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
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
                    .Select(x => new { x.group_id, x.size })
                    .ToList();

                return Json(listSizes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception eSize)
            {
                throw;
            }
        }

        // GET: /Store/getshoes
        [HttpGet]
        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public JsonResult GetShoes(int groups, string sizes, string sorting, string brands)
        {
            try
            {
                var sortingValue = "";
                switch (sorting)
                {
                    case "1_asc":
                    case "2_asc":
                        sortingValue = "ASC";
                        break;
                    case "3_desc":
                        sortingValue = "DESC";
                        break;
                    default:
                        sortingValue = "ASC";
                        break;
                }

                string sql =
                    " SELECT gr.group_id, gr.group_name, g.good_id, g.good, g.good_key, " +
                    " MAX(o.price) as price, o.currency, o.feature, " +
                    " (CASE WHEN o.amount > 0 THEN 'Есть в наличии' ELSE 'Нет в наличии' END) as amount " +
                    " FROM goods g, groups gr, offers o " +
                    " WHERE 1 = 1 " +
                    " AND g.group_id = gr.group_id " +
                    " AND g.good_key = o.good_key " +
                    " AND g.group_id = " + groups + " " +
                    " AND g.img_path != '' " +
                    " AND o.amount > 0 " +
                    (sizes != "0" && sizes.Length > 0 ? " AND o.size in (" + sizes + ") " : "") +
                    (brands != "0" && brands.Length > 0 ? " AND g.brand_id in (" + brands + ") " : "") +
                    " GROUP BY 1,2,3,4,5 " +
                    " ORDER BY price " + sortingValue + ", feature ";
                var dt = SQLiteProvider.OpenSql(sql);

                return Json(dt.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        // GET: /Store/getshoescount
        [HttpGet]
        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public JsonResult GetShoesCount(int groups, string sizes, string brands)
        {
            try
            {
                string sql =
                    " SELECT count(distinct g.good_id) count " +
                    " FROM goods g, offers o " +
                    " WHERE 1 = 1 " +
                    " AND g.good_key = o.good_key " +
                    (sizes != "0" && sizes.Length > 0 ? " AND o.size in (" + sizes + ") " : "") +
                    (brands != "0" && brands.Length > 0 ? " AND g.brand_id in (" + brands + ") " : "") +
                    " AND g.img_path != '' " +
                    " AND g.group_id = " + groups;
                var dt = SQLiteProvider.OpenSql(sql);
                return Json(dt.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                throw error;
            }
        }

    }
}
