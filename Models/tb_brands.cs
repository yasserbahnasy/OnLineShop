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
    
    public partial class tb_brands
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_brands()
        {
            this.tb_products = new HashSet<tb_products>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string englishname { get; set; }
        public string logo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_products> tb_products { get; set; }
    }
}