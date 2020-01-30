using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnLineShop.Models
{
    [MetadataType(typeof(UserMetaData))]
    public partial class tb_Users
    {

    }

    public class UserMetaData
    {
        
        [Required]
        [Display(Name = "الإسم")]
        public string fullname { get; set; }

        [Required]
        [Display(Name = "إسم المستخدم")]    
        public string username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "البريد الإلكتروني")]
        public string email { get; set; }


        [Display(Name = "نوع الحساب")]
        public string type { get; set; }

        [Required]
        [Display(Name = "كلمة السر")]
        [DataType(DataType.Password)]
        public string password { get; set; }

    }

    

}