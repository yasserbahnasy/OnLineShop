using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnLineShop.Models
{
    [MetadataType(typeof(ProductsMetaData))]
    public partial class tb_products
    {
    }

    public class ProductsMetaData
    {
        [Required]
        [Display(Name = "إسم المنتج")]
        public string title { get; set; }

        [Display(Name = "التاريخ")]
        public string date { get; set; }

        [Required]
        [Display(Name = "إسم القسم")]
        public string category_id { get; set; }

        [Display(Name = "صورة المنتج")]
        public string image { get; set; }

        [Required]
        [AllowHtml]
        [Display(Name = "وصف المنتج")]
        public string description { get; set; }

        [Required]
        [Display(Name = "سعر المنتج")]
        public string price { get; set; }

        [Required]
        [Display(Name = "العلامة التجارية")]
        public string brand_id { get; set; }

        [Required]
        [Display(Name = "كمية المنتج")]
        public string quantity { get; set; }
    }
}