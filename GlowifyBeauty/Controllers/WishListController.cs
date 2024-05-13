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
    public class WishListController : Controller
    {
        //
        // GET: /WishList/

        DataBase db = new DataBase();

        public ActionResult AddFavourite(int id)
        {

            if (Session["Email"] != null)
            {
                db.con.Open();
                int qty = 1;
                string q = "insert into WhisListTable (Account_id,Product_Id) values (@Account_id,@Product_Id)";
                SqlCommand cmd = new SqlCommand(q, db.con);
                cmd.Parameters.AddWithValue("@Account_id", Session["Account_id"]);
                cmd.Parameters.AddWithValue("@Product_Id", id);
                cmd.ExecuteNonQuery();
                db.con.Close();
                return RedirectToAction("ShowFavourite", "WishList");

            }
            else
            {
                return RedirectToAction("SignIn", "Accounts");
            }
        }

        public ActionResult ShowFavourite()
        {
            if (Session["Email"] != null)
            {
                List<Products> files = new List<Products>();

                db.con.Open();
                string q = "  SELECT p.Product_Id,p.ProductName,p.ProductPrice,p.Img1,p.Img1type,w.Account_id,WishList_ID FROM Products p INNER JOIN  WhisListTable w on w.Product_Id=p.Product_Id where w.Account_id=(@Account_id)";
                SqlCommand cmd = new SqlCommand(q, db.con);
                cmd.Parameters.AddWithValue("@Account_id", Session["Account_id"]);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    files.Add(new Products
                    {
                        Product_Id = int.Parse(sdr["Product_Id"].ToString()),
                        ProductName = sdr["ProductName"].ToString(),
                        ProductPrice = int.Parse(sdr["ProductPrice"].ToString()),
                        Img1type = sdr["Img1type"].ToString(),
                        Img1 = (byte[])sdr["Img1"],
                        Account_id = int.Parse(sdr["Account_id"].ToString()),
                        WishList_ID = int.Parse(sdr["WishList_ID"].ToString()),

                    });
                }
                db.con.Close();
                sdr.Close();
                return View(files);
            }
            else
            {
                return RedirectToAction("SignIn", "Accounts");
            }
        }


        public ActionResult Remove(int id)
        {
            db.con.Open();
            SqlCommand cmd = new SqlCommand("delete from WhisListTable  where WishList_ID=(@WishList_ID) ", db.con);
            cmd.Parameters.AddWithValue("@WishList_ID", id);
            cmd.ExecuteNonQuery();
            db.con.Close();
            return RedirectToAction("ShowFavourite");
        }


    }
}
