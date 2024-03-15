using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities
{
    //Chi tiết nguyên liệu trong công thức
    public class IngredientDetail : BaseEntityCommons
    {
        public virtual Recipe Recipe { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        //Số lượng nguyên liệu
        public int Quantity { get; set; }
        //Đơn vị tính của nguyên liệu: gram, kg, spoon,...
        public string Unit { get; set; }
    }
}
