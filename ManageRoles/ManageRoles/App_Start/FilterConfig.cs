using System.Web;
using System.Web.Mvc;
using ManageRoles.Filters;

namespace ManageRoles
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomErrorHandler());
        }
    }
}
