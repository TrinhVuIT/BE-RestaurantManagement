namespace RestaurantManagement.Data.RequestModels.Food
{
    public class CreateRecipeRequestModel
    {
        public long FoodId { get; set; }
        public List<AddIngredientDetail> ListIngredientDetail { get; set; }
        public string Step { get; set; }
        public string Description { get; set; }
    }
    public class AddIngredientDetail
    {
        public long RecipeId { get; set; }
        public long IngredientId { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
    }
}
