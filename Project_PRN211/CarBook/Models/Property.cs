using System.ComponentModel.DataAnnotations;

namespace CarBook.Models
{
    public class Property
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public int Milerage { get; set; }
        public string Transmission { get; set; }
        public int Seats { get; set; }
        public int Luggage { get; set; }
        public string Fuel { get; set; }
    }
}
