using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using _1CIntegrationDB;

namespace _1CIntegration.Controllers
{
    public class StoreController : Controller
    {
        //
        // GET: /Store/
        [OutputCache(Duration = 30, Location = System.Web.UI.OutputCacheLocation.Server)]
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Store/GetImgProduct?good_key=&width=&height=
        [HttpGet]
        public ActionResult GetImgProduct(string good_key, int? width, int? height)
        {
            try
            {
                if (good_key.IsNullOrEmpty()) return null;

                var dataImgPath = SQLiteProvider.OpenSql("select img_path from goods where good_key = '" + good_key + "'");

                if (dataImgPath != null && dataImgPath.Rows.Count == 1 && !dataImgPath.Rows[0]["img_path"].IsNullOrEmpty())
                {
                    var image = Image.FromFile("h:/root/home/djinaroshop-001/www/webdata/" + dataImgPath.Rows[0]["img_path"]);
                    if (width != null && height != null)
                    {
                        return
                            new FileStreamResult(
                                image.ResizeBitmapUpto(width ?? 500, height ?? 500, InterpolationMode.HighQualityBicubic)
                                    .ToStream(ImageFormat.Jpeg), "image/jpeg");
                    }
                    else
                    {
                        return new FileStreamResult(image.ToStream(ImageFormat.Jpeg), "image/jpeg");
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // GET: /Store/getgroups
        [HttpGet]
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
        [HttpGet]
        public JsonResult GetShoes(int groups, string sizes, int page, string sorting, string brands)
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
                    " SELECT gr.group_id, gr.group_name, g.good_id, g.good, g.good_key, g.img_path, " +
                    " MAX(o.price) as price, o.currency, o.feature, " +
                    " (CASE WHEN o.amount > 0 THEN 'Есть в наличии' ELSE 'Нет в наличии' END) as amount " +
                    " FROM goods g, groups gr, offers o " +
                    " WHERE 1 = 1 " +
                    " AND g.group_id = gr.group_id " +
                    " AND g.good_key = o.good_key " +
                    " AND g.group_id = " + groups + " " +
                    (sizes != "0" && sizes.Length > 0 ? " AND o.size in ("+sizes+") "  : "") +
                    (brands != "0" && brands.Length > 0 ? " AND g.brand_id in (" + brands + ") " : "") +
                    " GROUP BY 1,2,3,4,5 " +
                    " ORDER BY price " + sortingValue + ", feature " +
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
        [HttpGet]
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
