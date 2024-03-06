using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Data.RequestModels.Email
{
    public class SendToEmailRequestModel
    {
        [Required]
        [EmailAddress]
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
