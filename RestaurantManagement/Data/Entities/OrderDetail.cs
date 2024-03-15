using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities
{
    public class OrderDetail : BaseEntityCommons
    {
        public virtual Order Order { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        public int Quantity { get; set; }
    }
}
