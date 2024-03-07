namespace RestaurantManagement.Data.RequestModels.Email
{
    public class EmailConfigRequestModel
    {
        public string Tittle { get; set; }
        public string Content { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
