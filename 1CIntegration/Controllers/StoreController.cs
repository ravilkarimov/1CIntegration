using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using _1CIntegrationDB;
using System.Configuration;
using System.Text.RegularExpressions;

namespace _1CIntegration.Controllers
{
    public class StoreController : AsyncController
    {
        // GET: /Store/
        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Store/GetImgProductMin?good_id
        [HttpGet]
        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.Client)]
        public async Task<ActionResult> GetImgProductMin(Int64 good_id)
        {
            return await Task<FileStreamResult>.Factory.StartNew(() =>
            {
                if (good_id.IsNullOrEmpty()) return null;

                var imgPath = EntitiesMethods.GetGood(good_id).img_path;

                if (imgPath.IsNullOrEmpty()) return null;
                using (var fs = System.IO.File.OpenRead(path_web_data + imgPath.Replace(".jpg", "_min.jpg")))
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

        private const string query_get_img_path = "select img_path from goods where good_id = ";
        private string path_web_data = ConfigurationManager.AppSettings["FileWatcher"];

        // GET: /Store/GetImgProduct?good_id=
        [HttpGet]
        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.Client)]
        public async Task<ActionResult> GetImgProduct(Int64 good_id)
        {
            return await Task<FileStreamResult>.Factory.StartNew(() =>
            {
                if (good_id.IsNullOrEmpty()) return null;

                var imgPath = EntitiesMethods.GetGood(good_id).img_path;

                if (imgPath.IsNullOrEmpty()) return null;
                using (var fs = System.IO.File.OpenRead(path_web_data + imgPath))
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
        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.Server)]
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
        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.Server)]
        public JsonResult GetBrands()
        {
            try
            {
                var sql = "SELECT b.brand_id, b.brand, COUNT(*) as count " +
                          "FROM d_brands b, goods g " +
                          "WHERE b.brand_id = g.brand_id " +
                          "AND g.group_id in (1,2,3)  " +
                          "GROUP BY b.brand_id, b.brand " +
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
                var sql = "SELECT DISTINCT size FROM offers where good_key = N'" + id + "' ORDER BY size DESC ";

                return Json(SQLiteProvider.OpenSql(sql).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception eSize)
            {
                throw;
            }
        }

        // GET: /Store/getsizes
        [HttpGet]
        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.Server)]
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

                var dt = SQLiteProvider.OpenSql("select distinct o.size  from offers o, goods g where o.good_key = g.good_key and g.group_id = 1 and o.size <> ''");
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
        public JsonResult GetShoes(int groups, string sizes, string brands, string search, string price_1, string price_2)
        {
            try
            {
                string sql = "";

                var Sizes = sizes.Split(',').ToList();
                var Brands = brands.Split(',').Select(x => Regex.Match(x, @"\d+").AsInteger()).ToList();

                var filter = search.Split(new Char[] {',', ' '}).ToList()
                    .Where(filter_word => !filter_word.IsNullOrEmpty())
                    .Aggregate("", (current, filter_word) => current + string.Format(" and (upper(good) LIKE '{0}%' OR upper(good) LIKE '%{0}%' OR upper(good) LIKE '%{0}') ", filter_word.ToUpper()));

                sql =
                    " SELECT DISTINCT gr.group_id, gr.group_name, g.good_id, g.good, g.good_key, " +
                    " MAX(o.price) as price, o.currency, " +
                    " (CASE WHEN o.amount > 0 THEN N'Есть в наличии' ELSE N'Нет в наличии' END) as amount, " +
                    " g.img_path " +
                    " FROM goods g, groups gr, offers o " +
                    " WHERE 1 = 1 " +
                    " AND g.group_id = gr.group_id " +
                    " AND g.good_key = o.good_key " +
                    " AND g.group_id = " + (groups > 0 ? groups : 1) + " " +
                    " AND g.img_path != '' " +
                    " AND o.amount > 0 " +
                    " AND o.price >= " + price_1 + " AND price <= " + price_2 + " " +
                    filter +
                    (Sizes.Count > 0 && Sizes.Any(x => !x.IsNullOrEmpty()) ? " AND o.size in (" + string.Join(",", Sizes.Where(x => !x.IsNullOrEmpty())) + ") " : "") +
                    (Brands.Count > 0 && Brands.Any(x => x > 0) ? " AND g.brand_id in (" + string.Join(",", Brands) + ") " : "") +
                    " GROUP BY gr.group_id, gr.group_name, g.good_id, g.good, g.good_key, o.currency, o.amount, g.img_path " +
                    " ORDER BY price ASC, g.good ";
                var dt = SQLiteProvider.OpenSql(sql);

                foreach (DataRow row in dt.Rows)
                {
                    var imgPath = "../webdata/" + row["img_path"].ToString().Replace(".jpg", "_min.jpg");
                    row["img_path"] = imgPath;
                }

                return Json(dt.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        // GET: /Store/search
        [HttpGet]
        public JsonResult Search(string term)
        {
            try
            {
                string filter = term.Split(new Char[] {',', ' '}).Aggregate(" 1 = 1 ", (current, filter_word) => current + string.Format(" OR upper(good) LIKE '{0}%' OR upper(good) LIKE '%{0}%' OR upper(good) LIKE '%{0}' ", filter_word.ToUpper()));

                var sql = "SELECT DISTINCT good FROM goods WHERE " + filter + " ORDER BY good ASC ";

                var resultQuery = SQLiteProvider.OpenSql(sql);

                List<string> listWord = new List<string>();

                foreach (DataRow result in resultQuery.Rows)
                {
                    listWord.AddRange(term.Split(new Char[] {',', ' '}).Select(word => result["good"].ToString().Split(' ').Where(x => x.ToUpper().Contains(word.ToUpper())).Select(x => x).FirstOrDefault()).Where(slovo => !slovo.IsNullOrEmpty()));
                }

                return Json(listWord.Distinct().OrderBy(x => x).Take(10), JsonRequestBehavior.AllowGet);
            }
            catch (Exception eSearch)
            {
                throw;
            }
        }

    }
}
