using RestaurantManagement.Commons;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Data.Entities
{
    public class LogsSystem : BaseEntityCommons
    {
        [MaxLength(455)]
        public string? LogName { get; set; }
        [MaxLength(255)]
        public string? LogGroup { get; set; }
        public long? LogAmount { get; set; }
        public string? LogDescriptions { get; set; }
    }
}
