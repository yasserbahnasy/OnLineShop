using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnLineShop.Models
{
    [MetadataType(typeof(BillinfoMetaData))]
    public partial class tb_billinfo
    {
    }

    public class BillinfoMetaData
    {

        public int id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "البريد الإلكتروني")]
        public string email { get; set; }

        [Required]
        [Display(Name = "إسم الشركة")]
        public string company_name { get; set; }

        [Required]
        [Display(Name = "الإسم كامل")]
        public string fullname { get; set; }

        [Required]
        [Display(Name = "العنوان")]
        public string address { get; set; }

        [Required]
        [Range(1000,9999)]
        [Display(Name = "الرمز البريدي")]
        public int postal_code { get; set; }

        [Required]
        [Display(Name = "المحافظة")]
        public string city { get; set; }

        [Required]
        [Display(Name = "الدولة")]
        public string country { get; set; }

        [Required]
        [Display(Name = "رقم الهاتف")]
        public string phone { get; set; }

        [Required]
        [Display(Name = "المنطقة")]
        public string region { get; set; }

        public Nullable<int> user_id { get; set; }

        public virtual tb_Users tb_Users { get; set; }
    }
}