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
    public class CartController : Controller
    {
        //
        // GET: /Cart/

        DataBase db = new DataBase();


        public ActionResult AddCart(int id)
        {

            if (Session["Email"] != null)
            {
                db.con.Open();
                int qty = 1;
                string q = "insert into CartTable (Account_id,Product_Id,Qty,Status) values(@Account_id,@Product_Id,@Qty,@Status)";
                SqlCommand cmd = new SqlCommand(q, db.con);
                cmd.Parameters.AddWithValue("@Account_id", Session["Account_id"]);
                cmd.Parameters.AddWithValue("@Product_Id", id);
                cmd.Parameters.AddWithValue("@Qty", qty);
                cmd.Parameters.AddWithValue("@Status", "Insert Into Cart");

                cmd.ExecuteNonQuery();
                db.con.Close();
                return RedirectToAction("ShowCart", "Cart");

            }
            else
            {
                return RedirectToAction("SignIn", "Accounts");
            }
        }


        public ActionResult ShowCart()
        {
            if (Session["Email"] != null)
            {
                List<Products> files = new List<Products>();

                db.con.Open();
                string q = " SELECT p.Product_Id,p.ProductName,p.ProductPrice,p.Img1,p.Img1type,c.Qty,c.Cart_ID,c.Account_id,c.Status FROM Products p INNER JOIN  CartTable c on c.Product_Id=p.Product_Id where c.Account_id=(@Account_id) and c.Status ='Insert Into Cart'";
                SqlCommand cmd = new SqlCommand(q, db.con);
                cmd.Parameters.AddWithValue("@Account_id", Session["Account_id"]);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    files.Add(new Products
                    {
                        Cart_ID = int.Parse(sdr["Cart_ID"].ToString()),
                        Product_Id = int.Parse(sdr["Product_Id"].ToString()),
                        ProductName = sdr["ProductName"].ToString(),
                        ProductPrice = int.Parse(sdr["ProductPrice"].ToString()),
                        Qty = int.Parse(sdr["Qty"].ToString()),
                        Img1type = sdr["Img1type"].ToString(),
                        Img1 = (byte[])sdr["Img1"],
                        Account_id = int.Parse(sdr["Account_id"].ToString()),
                        Status = sdr["Status"].ToString(),


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


        public ActionResult Increase(int id)
        {
            db.con.Open();
            SqlCommand cmd1 = new SqlCommand("select Qty from CartTable where Cart_ID=(@Cart_ID)", db.con);
            cmd1.Parameters.AddWithValue("@Cart_ID", id);
            SqlDataReader sdr = cmd1.ExecuteReader();
            sdr.Read();
            int qty = 0;
            qty = int.Parse(sdr["Qty"].ToString());
            qty = qty + 1;
            sdr.Close();
            db.con.Close();
            db.con.Open();
            SqlCommand cmd2 = new SqlCommand("Update CartTable set Qty =(@Qty) where Cart_ID=(@Cart_ID)", db.con);

            cmd2.Parameters.AddWithValue("@Cart_ID", id);
            cmd2.Parameters.AddWithValue("@Qty", qty);
            cmd2.ExecuteNonQuery();
            db.con.Close();

            return RedirectToAction("ShowCart");
        }


        public ActionResult Decrease(int id)
        {
            db.con.Open();
            SqlCommand cmd1 = new SqlCommand("select Qty from CartTable where Cart_ID=(@Cart_ID)", db.con);
            cmd1.Parameters.AddWithValue("@Cart_ID", id);
            SqlDataReader sdr = cmd1.ExecuteReader();
            sdr.Read();
            int qty = 0;
            qty = int.Parse(sdr["Qty"].ToString());
            qty = qty - 1;
            sdr.Close();
            db.con.Close();
            db.con.Open();

            if (qty < 0)
            {
                return RedirectToAction("ShowCart");
            }

            SqlCommand cmd2 = new SqlCommand("Update CartTable set Qty =(@Qty) where Cart_ID=(@Cart_ID)", db.con);
            cmd2.Parameters.AddWithValue("@Cart_ID", id);
            cmd2.Parameters.AddWithValue("@Qty", qty);
            cmd2.ExecuteNonQuery();
            db.con.Close();

            return RedirectToAction("ShowCart");
        }


        public ActionResult Remove(int id)
        {
            db.con.Open();
            SqlCommand cmd = new SqlCommand("delete from CartTable  where Cart_ID=(@Cart_ID) ", db.con);

            cmd.Parameters.AddWithValue("@Cart_ID", id);

            cmd.ExecuteNonQuery();
            db.con.Close();
            return RedirectToAction("ShowCart");
        }

    }
}
