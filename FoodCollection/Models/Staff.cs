using System.ComponentModel.DataAnnotations;

namespace FoodCollection.Models
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }
        public string Lname { get; set; }
        public string Fname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Addres { get; set; }
        public ICollection<Appointment> Appointment { get; set; }
        public ICollection<Report> Report { get; set; }
        public ICollection<Pickup> Pickup { get; set; }
    }
}
