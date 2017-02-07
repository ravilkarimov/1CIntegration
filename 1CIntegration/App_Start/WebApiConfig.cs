using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;

namespace _1CIntegration
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Раскомментируйте следующую строку кода, чтобы включить поддержку запросов для действий с типом возвращаемого значения IQueryable или IQueryable<T>.
            // Чтобы избежать обработки неожиданных или вредоносных запросов, используйте параметры проверки в QueryableAttribute, чтобы проверять входящие запросы.
            // Дополнительные сведения см. по адресу http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // Чтобы отключить трассировку в приложении, закомментируйте или удалите следующую строку кода
            // Дополнительные сведения см. по адресу: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();

            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.Formatters.Add(new BrowserJsonFormatter());
        }

        public class BrowserJsonFormatter : JsonMediaTypeFormatter
        {
            public BrowserJsonFormatter()
            {
                this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
                this.SerializerSettings.Formatting = Formatting.Indented;
            }

            public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
            {
                base.SetDefaultContentHeaders(type, headers, mediaType);
                headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
        }
    }
}
