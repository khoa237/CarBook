using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarBook.Models
{
    public class Car
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [DataType("nvarchar(50)")]
        public string Name { get; set; }
        [DataType("ntext")]
        public string Description { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Price { get; set; }

        [ForeignKey("Category")]
        [Required]
        public int CategoryID { get; set; }
        public Category? Category { get; set; }

        [ForeignKey("Property")]
        [Required]
        public int PropertyID { get; set; }
        public Property? Property { get; set; }

        [ForeignKey("Feature")]
        [Required]
        public int FeatureID { get; set; }
        public Feature? Feature { get; set; }

        [ForeignKey("AppUser")]
        [DataType("nvarchar(450)")]
        public string OwnerID { get; set; }
        public IdentityUser? Owner { get; set; }
    }
}
