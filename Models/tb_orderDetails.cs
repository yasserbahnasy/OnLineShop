//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnLineShop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_orderDetails
    {
        public int id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> product_id { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<int> check_id { get; set; }
        public string type { get; set; }
    
        public virtual tb_products tb_products { get; set; }
        public virtual tb_Users tb_Users { get; set; }
        public virtual tb_checkout tb_checkout { get; set; }
    }
}