namespace API.MangaShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Favorites
    {
        [Key]
        public int FavoriteId { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public virtual Products Products { get; set; }

        public virtual Users Users { get; set; }
    }
}
