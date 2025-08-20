using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodCollection.Models
{
    public class Pickup
    {
        [Key]
        public int PickupId { get; set; }
        public DateOnly Date {  get; set; }
        public string PickupStatus { get; set; }
        public int BookPickupId { get; set; }
        [ForeignKey("BookPickupId")]
        public BookPickup BookPickup { get; set; }
        public int StaffId { get; set; }
        [ForeignKey("StaffId")]
        public Staff Staff { get; set; }

    }
}
