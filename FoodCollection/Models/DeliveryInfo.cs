using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodCollection.Models
{
    public class DeliveryInfo
    {
        [Key]
        public int DelilveryInfoId { get; set; }
        public DateOnly Date {  get; set; }
        public string DeliveryStatus { get; set; }
        public int PickupId { get; set; }
        [ForeignKey("PickupId")]
        public Pickup Pickup { get; set; }
        public int OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public Organization Organization { get; set; }
    }
}
