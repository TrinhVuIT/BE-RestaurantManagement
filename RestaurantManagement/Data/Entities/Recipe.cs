using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities
{
    //Công thức chế biến
    public class Recipe : BaseEntityCommons
    {
        public virtual Food Food { get; set; }
        public virtual Ingredient Ingredient { get; set; }
        // Số lượng nguyên liệu
        public int Quantity { get; set; }
        // Đơn vị tính của nguyên liệu
        public string Unit {  get; set; }
        // Các bước thực hiện để chế biến món
        public string Step { get; set; }
        public string Description { get; set; }
    }
}
