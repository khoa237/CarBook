using System.ComponentModel.DataAnnotations;

namespace CarBook.Models
{
    public class Feature
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public bool IsAirconditions { get; set; }
        public bool IsChildSeat { get; set; }
        public bool IsGPS { get; set; }
        public bool IsLuggage { get; set; }
        public bool IsMusic { get; set; }
        public bool IsSeatBelt { get; set; }
        public bool IsSleepingBed { get; set; }
        public bool IsWater { get; set; }
        public bool IsBluetooth { get; set; }
        public bool IsOnboardComputer { get; set; }
        public bool IsAudioInput { get; set; }
        public bool IsLongTermTrips { get; set; }
        public bool IsCarKit { get; set; }
        public bool IsRemoteCentralLocking { get; set; }
        public bool IsClimateControl { get; set; }

    }
}
