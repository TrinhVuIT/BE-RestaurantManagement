using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities
{
    public class Ingredient : BaseEntityCommons
    {
        //Tên nguyên liệu
        public string IngredientName { get; set; }
        public DateTime? Exp {  get; set; }
    }
}
