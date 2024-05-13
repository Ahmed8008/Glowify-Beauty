using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
namespace GlowifyBeauty.Models
{
    public class DataBase
    {
        //public static string constr = @"workstation id=GlowifyBeauty.mssql.somee.com;packet size=4096;user id=ahmedu69_SQLLogin_1;pwd=99m8edcqn2;data source=GlowifyBeauty.mssql.somee.com;persist security info=False;initial catalog=GlowifyBeauty;TrustServerCertificate=True";
        public static string constr = @"Data Source=sql.bsite.net\MSSQL2016;Initial Catalog=glowifybeauty_gb;User ID=glowifybeauty_gb;Password=Ahmed12@";
        //public static string constr = @"Server=sql.bsite.net\MSSQL2016;Database=glowifybeauty_;User Id=glowifybeauty_;password=Ahmed12@;";
        //  public static string constr = @"Data Source=DESKTOP-68SGKHV\SQLEXPRESS;Initial Catalog=CosmeticWebsite;Integrated Security=true";

        public SqlConnection con = new SqlConnection(constr);
    }
}