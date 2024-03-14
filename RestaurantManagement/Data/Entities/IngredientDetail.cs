using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities
{
    //Chi tiết nguyên liệu trong công thức
    public class IngredientDetail : BaseEntity
    {
        public Recipe Recipe { get; set; }
        public Ingredient Ingredient { get; set; }
        //Số lượng nguyên liệu
        public int Quantity { get; set; }
        //Đơn vị tính của nguyên liệu: gram, kg, spoon,...
        public string Unit { get; set; }
    }
}
