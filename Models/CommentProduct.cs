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
    
    public partial class CommentProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CommentProduct()
        {
            this.CommentProduct1 = new HashSet<CommentProduct>();
        }
    
        public int ID { get; set; }
        public string Content { get; set; }
        public int ProductID { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Reply_coment { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Product Product { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommentProduct> CommentProduct1 { get; set; }
        public virtual CommentProduct CommentProduct2 { get; set; }
    }
}
