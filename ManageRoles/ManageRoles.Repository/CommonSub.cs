using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ManageRoles.Models;
using Dapper;

namespace ManageRoles.Repository
{
    public static class CommonSub
    {
        public static List<SubMenuMaster> ShowSubMenu(int menuId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@MenuId", menuId);
                    return con.Query<SubMenuMaster>("Usp_GetAllSubMenuByMenuId", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}