using System.Web;
using System.Web.Mvc;

namespace WebApiSecurity.SSL
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
