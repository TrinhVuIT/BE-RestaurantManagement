using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities
{
    public class EmailConfig : BaseEntityCommons
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Host {  get; set; }
        public int Port { get; set; }
        public string Tittle {  get; set; }
        public string? Content { get; set; }
        public bool UseSSL { get; set; }
        public bool UseStartTls { get; set; }
        public string? ServerName { get; set; }
    }
}
