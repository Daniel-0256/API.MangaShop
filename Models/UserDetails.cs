namespace API.MangaShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserDetails
    {
        [Key]
        public int UserDetailsId { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(510)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(255)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string Province { get; set; }

        [Required]
        [StringLength(50)]
        public string CardNumber { get; set; }

        [Required]
        public DateTime CardExpiryDate { get; set; }

        [Required]
        [StringLength(50)]
        public string CardCVV { get; set; }

        public bool IsActive { get; set; }

        public virtual Users Users { get; set; }
    }
}
