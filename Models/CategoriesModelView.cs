using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnLineShop.Models
{
    [MetadataType(typeof(CategoriesMetaData))]
    public partial class tb_Categories
    {
    }

    public class CategoriesMetaData
    {
        [Required]
        [Display(Name = "إسم القسم")]
        public string cat_name { get; set; }

        [Required]
        [Display(Name = "أيقونة القسم")]
        public string cat_icon { get; set; }

        [Required]
        [Display(Name = "وصف القسم")]
        public string cat_desc { get; set; }

        public virtual ICollection<tb_products> tb_products { get; set; }
    }
}