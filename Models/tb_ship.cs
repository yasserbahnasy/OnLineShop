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
    
    public partial class tb_ship
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_ship()
        {
            this.tb_checkout = new HashSet<tb_checkout>();
        }
    
        public int id { get; set; }
        public string company_name { get; set; }
        public string fullname { get; set; }
        public string address { get; set; }
        public Nullable<int> postal_code { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        public string region { get; set; }
        public Nullable<int> user_id { get; set; }
        public string city { get; set; }
    
        public virtual tb_Users tb_Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_checkout> tb_checkout { get; set; }
    }
}
