namespace API.MangaShop.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            Cart = new HashSet<Cart>();
            Favorites = new HashSet<Favorites>();
            OrderDetails = new HashSet<OrderDetails>();
        }

        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(255)]
        public string NameProduct { get; set; }

        [Required]
        [StringLength(510)]
        public string Image { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool Availability { get; set; }

        [Required]
        [StringLength(255)]
        public string Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Cart { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [JsonIgnore]
        public virtual ICollection<Favorites> Favorites { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
