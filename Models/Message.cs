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
    
    public partial class Message
    {
        public int ID { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string Message1 { get; set; }
        public int Status { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateViewed { get; set; }
        public Nullable<int> Type { get; set; }
        public string Image { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual AspNetUser AspNetUser1 { get; set; }
    }
}
