namespace RestaurantManagement.Data.ResponseModels.FoodResponseModel
{
    public class RecipeResponseModel
    {
        public long Id { get; set; }
        public string RecipeName { get; set; }
        public FoodMapper Food {  get; set; }
        public IngredientDetailMapper IngredientDetail { get; set; }
        public string? Step { get; set; }
        public string? Description { get; set; }
    }
    public class FoodMapper
    {
        public long Id { get; set; }
        public string FoodName { get; set; }
        public string FoodDescription { get; set; }
        public decimal FoodPrice { get; set; }
    }
    public class IngredientDetailMapper
    {
        public long Id { get; set; }
        public string IngredientName { get; set; }
        public DateTime Exp { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
}
