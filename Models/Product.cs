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
    using System.ComponentModel.DataAnnotations;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Carts = new HashSet<Cart>();
            this.CommentProducts = new HashSet<CommentProduct>();
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    
        public int ID { get; set; }
        [Required(ErrorMessage = "Please select product category!")]
        public int CategoryID { get; set; }
        [Required(ErrorMessage = "Please select product type!")]
        public int TypeID { get; set; }
        [Required(ErrorMessage = "Please enter product name!")]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Please choose an image for the product")]
        public string Image { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }
        public Nullable<int> Status { get; set; }
        [Required(ErrorMessage = "Please enter product quantity !")]
        public int Quantity { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommentProduct> CommentProducts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual Type Type { get; set; }
    }
}
