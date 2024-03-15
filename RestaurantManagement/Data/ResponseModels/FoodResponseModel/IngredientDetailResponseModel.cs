namespace RestaurantManagement.Data.ResponseModels.FoodResponseModel
{
    public class IngredientDetailResponseModel
    {
        public long Id { get; set; }
        public RecipeMapper Recipe {  get; set; }
        public IngredientMapper Ingredient { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public DateTime? NgayTao { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public string? NguoiTao { get; set; }
        public string? NguoiCapNhat { get; set; }
    }

    public class RecipeMapper
    {
        public long Id { get; set; }
        public string RecipeName { get; set; }
        public string? Step { get; set; }
        public string? Description { get; set; }
    }

    public class IngredientMapper
    {
        public long Id { get; set; }
        public string IngredientName { get; set; }
        public DateTime? Exp { get; set; }
    }
}
