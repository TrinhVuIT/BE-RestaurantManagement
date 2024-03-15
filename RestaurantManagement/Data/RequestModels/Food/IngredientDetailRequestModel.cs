namespace RestaurantManagement.Data.RequestModels.Food
{
    public class IngredientDetailRequestModel
    {
            public long RecipeId { get; set; }
            public long IngredientId { get; set; }
            public int Quantity { get; set; }
            public string Unit { get; set; }
    }
    public class UpdateIngredientDetailRequestModel
    {
        public long IngredientId { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
}
