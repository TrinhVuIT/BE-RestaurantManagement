namespace RestaurantManagement.Data.ResponseModels.FoodResponseModel
{
    public class RecipeResponseModel
    {
        public long Id { get; set; }
        public string RecipeName { get; set; }
        public FoodMapper Food {  get; set; }
        public List<IngredientDetailMapper> IngredientDetail { get; set; }
        public string? Step { get; set; }
        public string? Description { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public string? NguoiTao { get; set; }
        public string? NguoiCapNhat { get; set; }
    }
    public class FoodMapper
    {
        public long Id { get; set; }
        public string FoodName { get; set; }
        public string? FoodDescription { get; set; }
        public decimal? FoodPrice { get; set; }
    }
    public class IngredientDetailMapper
    {
        public long Id { get; set; }
        public string IngredientName { get; set; }
        public DateTime? Exp { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
}
