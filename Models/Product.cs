//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GardenShopOnline.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Carts = new HashSet<Cart>();
            this.CommentProducts = new HashSet<CommentProduct>();
            this.OrderDetails = new HashSet<OrderDetail>();
            this.Pictures = new HashSet<Picture>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Place { get; set; }
        public string Address { get; set; }
        public int TypeID { get; set; }
        public int CategoryID { get; set; }
        public Nullable<byte> Amount { get; set; }
        public Nullable<System.DateTime> CreateAtTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual Categogy Categogy { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommentProduct> CommentProducts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Picture> Pictures { get; set; }
        public virtual Type Type { get; set; }
    }
}
