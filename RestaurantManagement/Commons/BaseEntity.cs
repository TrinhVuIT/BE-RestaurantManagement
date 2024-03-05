using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Commons
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
    }
}
