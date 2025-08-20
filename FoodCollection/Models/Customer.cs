using System.ComponentModel.DataAnnotations;

namespace FoodCollection.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string Lname { get; set; }
        public string Fname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public ICollection<BookPickup> BookPickup { get; set; }
        public ICollection<Appointment> Appointment { get; set; }
    }
}
