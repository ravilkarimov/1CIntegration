using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI;
using _1CIntegrationDB;

namespace _1CIntegration.Controllers
{
    [Compress]
    public class MobileController : Controller
    {
        //
        // GET: /mobile/
        [Compress]
        [OutputCache(Duration = 600, Location = OutputCacheLocation.Client)]
        public ActionResult Index()
        {
            return View();
        }

         private string path_web_data = ConfigurationManager.AppSettings["FileWatcher"];

        // GET: /Mobile/getshoes
        [Compress]
        [HttpGet]
        [OutputCache(Duration = 300, Location = OutputCacheLocation.None)]
        public JsonResult GetShoes(int groups, string sizes, string brands, string search, string price_1, string price_2)
        {
            try
            {
                string sql = "";

                var Sizes = sizes.Split(',').ToList();
                var Brands = brands.Split(',').Select(x => Regex.Match(x, @"\d+").AsInteger()).ToList();

                var filter = search.Split(new Char[] { ',', ' ' }).ToList()
                    .Where(filter_word => !filter_word.IsNullOrEmpty())
                    .Aggregate("", (current, filter_word) => current + string.Format(" and (upper(good) LIKE '{0}%' OR upper(good) LIKE '%{0}%' OR upper(good) LIKE '%{0}') ", filter_word.ToUpper()));

                sql =
                    " SELECT DISTINCT g.group_id, " +
                    " max(g.good_id) as good_id, " +
                    " g.good, g.good_key, " +
                    " MAX(o.price) as price, " +
                    " o.currency, g.img_path, " +
                    " (CASE WHEN max(CAST(r.receipt_date as DATE)) = (select max(CAST(receipt_date as DATE)) from receipts) THEN 1 ELSE 0 END) as new_good, " +
                    " MAX(CAST(r.receipt_date as DATE)) as receipt_date," +
                    " stuff " +
                    "   ( " +
                    "       ( " +
                    "           SELECT DISTINCT os.size  + ' | ' FROM offers os where os.good_key = g.good_key AND os.amount > 0 FOR XML PATH (''),type  " +
                    "       ).value('.','nvarchar(250)')," +
                    "       1,0,'' " +
                    "   ) as sizes " +
                    " FROM goods g " +
                    " INNER JOIN offers o ON g.good_key = o.good_key " +
                    " FULL OUTER JOIN receipts r ON r.good_key = g.good_key " +
                    " WHERE 1 = 1 " +
                    " AND g.group_id = " + (groups > 0 ? groups : 1) + " " +
                    " AND g.img_path != '' " +
                    " AND o.amount > 0 " +
                    " AND o.price >= " + price_1 + " AND price <= " + price_2 + " " +
                    filter +
                    (Sizes.Count > 0 && Sizes.Any(x => !x.IsNullOrEmpty()) ? " AND o.size in (" + string.Join(",", Sizes.Where(x => !x.IsNullOrEmpty())) + ") " : "") +
                    (Brands.Count > 0 && Brands.Any(x => x > 0) ? " AND g.brand_id in (" + string.Join(",", Brands) + ") " : "") +
                    " GROUP BY g.group_id, g.good, g.good_key, o.currency, g.img_path " + //g.good_id,
                    " ORDER BY receipt_date DESC, price ASC, g.good " +
                    " OFFSET 0 ROWS " +
                    " FETCH NEXT 50 ROWS ONLY";
                var dt = SQLiteProvider.OpenSql(sql);

                var countElementInRow = 2; //Количество товаров в строке
                var itemElement = 0;
                var totalCountRows = dt.Rows.Count;
                var countRow = Math.Floor((double)(totalCountRows / countElementInRow));
                var listRow = new List<string>();
                for (var i = 0; i <= countRow; i++)
                {
                    var stringElements = "";
                    for (var j = 0; j < countElementInRow; j++)
                    {
                        if (totalCountRows > itemElement)
                        {
                            var data = dt.Rows[itemElement];
                            if (data == null) continue;
                            var pathMinImg = data["img_path"].ToString().Replace(".jpg", "_min.jpg");
                            var isFile = System.IO.File.Exists(path_web_data + "/" + pathMinImg);
                            data["img_path"] = string.Format("../webdata/{0}", (isFile ? pathMinImg : data["img_path"].ToString()));

                            var spanNew = (data["new_good"].AsInteger() == 1
                                ? "<span class='product-label'>NEW</span>"
                                : "");
                            stringElements +=
                                String.Format(
                                    "<div class='col-xs-6 boot-mobile' id='shop-product-{0}'> " +
                                    "<div class='shop-product shop-mobile'> " +
                                    "<div class='overlay-wrapper'> " +
                                    "<img src='{1}' alt='{2}'> {5}" +
                                    "</div> " +
                                    "<div class='shop-product-info'> " +
                                    "<h5 class='product-name-mobile'>{2}</h5> " +
                                    "<p class='product-price pricelist'>{3} ₽</p> " +
                                    "<p class='product-sizes'>Размеры в наличии: <br/>{4}</p> " +
                                    "</div> " +
                                    "</div> " +
                                    "</div>",
                                    data["good_key"].ToString().Trim(), data["img_path"].ToString().Trim(),
                                    data["good"].ToString().Trim(), data["price"].ToString().Trim(),
                                    data["sizes"].ToString().Trim(), spanNew);
                            itemElement++;
                        }
                    }
                    listRow.Add(stringElements);
                }

                return Json(listRow, JsonRequestBehavior.AllowGet);
            }
            catch (Exception error)
            {
                throw error;
            }
        }

    }
}
