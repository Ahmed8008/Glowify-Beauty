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
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        DataBase db = new DataBase();

        public ActionResult HomePage()
        {
            var model = new HomePageViewModel();  // Initialize the model

            // Open the database connection
            db.con.Open();

            // Fetch Makeup Products
            string q = "SELECT * FROM Products WHERE ProductSubCategory = 'Makeup'";
            SqlCommand cmd = new SqlCommand(q, db.con);
            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {
                model.Makeup.Add(new Products
                {
                    Product_Id = int.Parse(sdr["Product_Id"].ToString()),
                    ProductName = sdr["ProductName"].ToString(),
                    ProductPrice = int.Parse(sdr["ProductPrice"].ToString()),
                    Img1 = (byte[])sdr["Img1"],
                    Img1type = sdr["Img1type"].ToString(),
                });
            }

            sdr.Close();  // Always close SqlDataReader after use

            // Fetch Perfume Products
            cmd = new SqlCommand("SELECT * FROM Products WHERE ProductSubCategory = 'Lips'", db.con);
            sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                model.Lips.Add(new Products
                {
                    Product_Id = int.Parse(sdr["Product_Id"].ToString()),
                    ProductName = sdr["ProductName"].ToString(),
                    ProductPrice = int.Parse(sdr["ProductPrice"].ToString()),
                    Img1 = (byte[])sdr["Img1"],
                    Img1type = sdr["Img1type"].ToString(),
                });
            }

            sdr.Close();  // Close the SqlDataReader

            // Fetch Nails Products
            cmd = new SqlCommand("SELECT * FROM Products WHERE ProductSubCategory = 'Nails'", db.con);
            sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                model.Nails.Add(new Products
                {
                    Product_Id = int.Parse(sdr["Product_Id"].ToString()),
                    ProductName = sdr["ProductName"].ToString(),
                    ProductPrice = int.Parse(sdr["ProductPrice"].ToString()),
                    Img1 = (byte[])sdr["Img1"],
                    Img1type = sdr["Img1type"].ToString(),
                });
            }

            sdr.Close();  // Close the SqlDataReader

            // Fetch Skincare Products
            cmd = new SqlCommand("SELECT * FROM Products WHERE ProductSubCategory = 'Skin Care'", db.con);
            sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                model.Skincare.Add(new Products
                {
                    Product_Id = int.Parse(sdr["Product_Id"].ToString()),
                    ProductName = sdr["ProductName"].ToString(),
                    ProductPrice = int.Parse(sdr["ProductPrice"].ToString()),
                    Img1 = (byte[])sdr["Img1"],
                    Img1type = sdr["Img1type"].ToString(),
                });
            }

            sdr.Close();  // Close the SqlDataReader

            // Fetch Haircare Products
            cmd = new SqlCommand("SELECT * FROM Products WHERE ProductSubCategory = 'Hair Care'", db.con);
            sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                model.Haircare.Add(new Products
                {
                    Product_Id = int.Parse(sdr["Product_Id"].ToString()),
                    ProductName = sdr["ProductName"].ToString(),
                    ProductPrice = int.Parse(sdr["ProductPrice"].ToString()),
                    Img1 = (byte[])sdr["Img1"],
                    Img1type = sdr["Img1type"].ToString(),
                });
            }

            sdr.Close();  // Close the SqlDataReader
            db.con.Close();  // Close the database connection

            return View(model);
        }

        public ActionResult Shop(string Check)
        {
            var model = new HomePageViewModel(); // Initialize the view model

            using (var connection = new SqlConnection(@"Data Source=sql.bsite.net\MSSQL2016;Initial Catalog=glowifybeauty_gb;User ID=glowifybeauty_gb;Password=Ahmed12@"))
            {
                connection.Open(); // Open the database connection

                string query = "";
                switch (Check)
                {
                    case "Makeup":
                        query = "SELECT * FROM Products WHERE ProductSubCategory = 'Makeup'";
                        break;
                    case "Purfume":
                        query = "SELECT * FROM Products WHERE ProductSubCategory = 'Purfume'";
                        break;
                    case "Skin Care":
                        query = "SELECT * FROM Products WHERE ProductSubCategory = 'Skin Care'";
                        break;
                    case "Eyes":
                        query = "SELECT * FROM Products WHERE ProductSubCategory = 'Eyes'";
                        break;
                    case "Maskara":
                        query = "SELECT * FROM Products WHERE ProductSubCategory = 'Maskara'";
                        break;
                    case "Lips":
                        query = "SELECT * FROM Products WHERE ProductSubCategory = 'Lips'";
                        break;
                    case "Cheeks":
                        query = "SELECT * FROM Products WHERE ProductSubCategory = 'Cheeks'";
                        break;
                    default:
                        return new HttpStatusCodeResult(404, "Category not found");
                }

                using (var cmd = new SqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = new Products
                            {
                                Product_Id = reader.GetInt32(reader.GetOrdinal("Product_Id")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                ProductPrice = reader.GetInt32(reader.GetOrdinal("ProductPrice")),
                                Img1 = (byte[])reader["Img1"],
                                Img1type = reader.GetString(reader.GetOrdinal("Img1type"))
                            };

                            switch (Check)
                            {
                                case "Makeup":
                                    model.Makeup.Add(product);
                                    break;
                                case "Purfume":
                                    model.Perfume.Add(product);
                                    break;
                                case "Skin Care":
                                    model.Skincare.Add(product);
                                    break;
                                case "Eyes":
                                    model.Eyes.Add(product);
                                    break;
                                case "Maskara":
                                    model.Maskara.Add(product);
                                    break;
                                case "Lips":
                                    model.Lips.Add(product);
                                    break;
                                case "Cheeks":
                                    model.Cheeks.Add(product);
                                    break;
                            }
                        }
                    }
                }
            }

            return View(model);
        }

        public ActionResult ProductDetails(int id)
        {

            List<Products> files = new List<Products>();
            db.con.Open();
            string q = "select * from Products  where Product_Id=(@Product_Id)";
            SqlCommand cmd = new SqlCommand(q, db.con);
            cmd.Parameters.AddWithValue("@Product_Id", id);
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                files.Add(new Products
                {
                    Product_Id = int.Parse(sdr["Product_Id"].ToString()),
                    ProductName = sdr["ProductName"].ToString(),
                    ProductPrice = int.Parse(sdr["ProductPrice"].ToString()),
                    ProductDescription = sdr["ProductDescription"].ToString(),
                    Img1 = (byte[])sdr["Img1"],
                    Img1type = sdr["Img1type"].ToString(),
                });
            }
            db.con.Close();
            sdr.Close();
            return View(files);
        }

        public ActionResult Category()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

    }
}
