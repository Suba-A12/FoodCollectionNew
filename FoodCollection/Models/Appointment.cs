using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodCollection.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public DateOnly Date {  get; set; }
        public TimeOnly Time { get; set; }
        public string Status { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public int StaffId { get; set; }
        [ForeignKey("StaffId")]
        public Staff Staff { get; set; }
    }
}
