using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlowifyBeauty.Models
{
    public class Products
    {
        public int Product_Id { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductDescription { get; set; }
        public byte[] Img1 { get; set; }
        public string Img1type { get; set; }
        public byte[] Img2 { get; set; }
        public string Img2type { get; set; }
        public byte[] Img3 { get; set; }
        public string Img3type { get; set; }
        public byte[] Img4 { get; set; }
        public string Img4type { get; set; }
        public byte[] Img5 { get; set; }
        public string Img5type { get; set; }
        public int ProductPrice { get; set; }
        public int Qty { get; set; }
        public int Cart_ID { get; set; }
        public int WishList_ID { get; set; }
        public int WishStatus { get; set; }
        public int Account_id { get; set; }
        public string Status { get; set; }
    }
}