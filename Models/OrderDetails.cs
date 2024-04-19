namespace API.MangaShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetails
    {
        [Key]
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int UserId { get; set; }

        public int UserDetailsId { get; set; }

        public int Quantity { get; set; }

        public virtual Orders Orders { get; set; }

        public virtual Products Products { get; set; }

        public virtual Users Users { get; set; }

        public virtual UserDetails UserDetails { get; set; }
    }
}
