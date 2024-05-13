using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlowifyBeauty.Models;

namespace GlowifyBeauty.Models
{
    public class HomePageViewModel
    {
        public List<Products> Makeup { get; set; }
        public List<Products> Perfume { get; set; }
        public List<Products> Nails { get; set; }
        public List<Products> Skincare { get; set; }
        public List<Products> Haircare { get; set; }
        public List<Products> Eyes { get; set; }
        public List<Products> Maskara { get; set; }
        public List<Products> Lips { get; set; }
        public List<Products> Cheeks { get; set; }
        public HomePageViewModel()
        {
            // Initialize lists to avoid null reference errors
            Makeup = new List<Products>();
            Perfume = new List<Products>();
            Nails = new List<Products>();
            Skincare = new List<Products>();
            Haircare = new List<Products>();
            Eyes = new List<Products>();
            Maskara = new List<Products>();
            Lips = new List<Products>();
            Cheeks = new List<Products>();
        }
    }





}