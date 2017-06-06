using System.Web.Http;
using Swashbuckle.Application;

namespace NewsCentralizer.API
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration httpConfig)
        {
            GlobalConfiguration.Configuration.EnableSwagger(c => { c.SingleApiVersion("v1", "NewsCentralizer.API"); });
        }
    }
}