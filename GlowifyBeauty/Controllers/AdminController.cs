using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlowifyBeauty.Models;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Web.SessionState;
using System.IO;

namespace GlowifyBeauty.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class AdminController : Controller
    {
        //

        DataBase db = new DataBase();

        // GET: /Admin/


        public ActionResult AdminDashboard()
        {

            if (Session["Email"] != null)
            {
                List<Products> files = new List<Products>();

                db.con.Open();
                string q = " SELECT p.Product_Id,p.ProductName,p.ProductPrice,p.Img1,p.Img1type,c.Qty,c.Cart_ID,c.Account_id,c.Status FROM Products p INNER JOIN  CartTable c on c.Product_Id=p.Product_Id where  c.Status ='Order Has been placed'";
                SqlCommand cmd = new SqlCommand(q, db.con);
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
                return RedirectToAction("Homepage", "Home");
            }
        }


        [HttpGet]
        public ActionResult TrendingProducts()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TrendingProducts(Products p, HttpPostedFileBase PostedFile1, HttpPostedFileBase PostedFile2, HttpPostedFileBase PostedFile3, HttpPostedFileBase PostedFile4, HttpPostedFileBase PostedFile5)
        {
            if (Session["Email"] != null)
            {
                string Category = "TrendingProducts";
                byte[] bytes1;
                BinaryReader br1 = new BinaryReader(PostedFile1.InputStream);
                bytes1 = br1.ReadBytes(PostedFile1.ContentLength);
                byte[] bytes2;
                BinaryReader br2 = new BinaryReader(PostedFile2.InputStream);
                bytes2 = br2.ReadBytes(PostedFile1.ContentLength);
                byte[] bytes3;
                BinaryReader br3 = new BinaryReader(PostedFile3.InputStream);
                bytes3 = br3.ReadBytes(PostedFile1.ContentLength);
                byte[] bytes4;
                BinaryReader br4 = new BinaryReader(PostedFile4.InputStream);
                bytes4 = br4.ReadBytes(PostedFile1.ContentLength);
                byte[] bytes5;
                BinaryReader br5 = new BinaryReader(PostedFile5.InputStream);
                bytes5 = br5.ReadBytes(PostedFile5.ContentLength);
                db.con.Open();
                string q = "insert into Products (ProductName,ProductCategory,ProductSubCategory,ProductDescription,Account_id,Img1,Img1type,Img2,Img2type,Img3,Img3type,Img4,Img4type,Img5,Img5type,ProductPrice) values(@ProductName,@ProductCategory,@ProductSubCategory,@ProductDescription,@Account_id,@Img1,@Img1type,@Img2,@Img2type,@Img3,@Img3type,@Img4,@Img4type,@Img5,@Img5type,@ProductPrice)";
                SqlCommand cmd = new SqlCommand(q, db.con);
                cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                cmd.Parameters.AddWithValue("@ProductCategory", Category);
                cmd.Parameters.AddWithValue("@ProductSubCategory", p.ProductSubCategory);
                cmd.Parameters.AddWithValue("@ProductDescription", p.ProductDescription);
                cmd.Parameters.AddWithValue("@Account_id", Session["Account_id"]);
                cmd.Parameters.AddWithValue("@Img1", bytes1);
                cmd.Parameters.AddWithValue("@Img1type", PostedFile1.ContentType);
                cmd.Parameters.AddWithValue("@Img2", bytes2);
                cmd.Parameters.AddWithValue("@Img2type", PostedFile2.ContentType);
                cmd.Parameters.AddWithValue("@Img3", bytes3);
                cmd.Parameters.AddWithValue("@Img3type", PostedFile3.ContentType);
                cmd.Parameters.AddWithValue("@Img4", bytes4);
                cmd.Parameters.AddWithValue("@Img4type", PostedFile4.ContentType);
                cmd.Parameters.AddWithValue("@Img5", bytes5);
                cmd.Parameters.AddWithValue("@Img5type", PostedFile5.ContentType);
                cmd.Parameters.AddWithValue("@ProductPrice", p.ProductPrice);
                cmd.ExecuteNonQuery();
                db.con.Close();
                return RedirectToAction("TrendingProducts");
            }
            else
            {
                return RedirectToAction("HomePage", "Home");
            }
        }








    }
}
