using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlowifyBeauty.Models;

namespace GlowifyBeauty.Controllers
{
    public class OrderController : Controller
    {
        //
        // GET: /Order/

        DataBase db = new DataBase();


        public ActionResult OrderStatus(int id, int TotalPrice)
        {
            db.con.Open();
            string q = "Update [CartTable] set Status =(@Status) where Account_id='" + id + "'";
            SqlCommand cmd = new SqlCommand(q, db.con);
            cmd.Parameters.AddWithValue("@Status", "Order Has been placed");
            cmd.ExecuteNonQuery();
            db.con.Close();
            ViewBag.Result = "Your order has been placed successfully";
            return RedirectToAction("ShowCart", "Cart");
        }





    }
}
