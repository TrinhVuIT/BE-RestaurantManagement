using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities
{
    //Công thức chế biến
    public class Recipe : BaseEntityCommons
    {
        public virtual Food Food { get; set; }
        public string RecipeName { get; set; }
        // Các bước thực hiện để chế biến món
        public string? Step { get; set; }
        public string? Description { get; set; }
    }
}
