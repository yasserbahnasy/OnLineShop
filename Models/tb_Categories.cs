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
    
    public partial class tb_Categories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_Categories()
        {
            this.tb_products = new HashSet<tb_products>();
        }
    
        public int cat_id { get; set; }
        public string cat_name { get; set; }
        public string cat_icon { get; set; }
        public string cat_desc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_products> tb_products { get; set; }
    }
}
