using System.ComponentModel.DataAnnotations;

namespace CarBook.Models
{
    public class Category
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [DataType("nvarchar(50)")]
        public string Name { get; set; }

    }
}
