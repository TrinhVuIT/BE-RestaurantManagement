using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities
{
    public class Food : BaseEntityCommons
    {
        public string FoodName { get; set; }
        public string? FoodDescription { get; set;}
        public decimal? FoodPrice { get; set;}
    }
}
