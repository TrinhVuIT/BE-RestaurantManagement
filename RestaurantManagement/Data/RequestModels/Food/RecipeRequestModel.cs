namespace RestaurantManagement.Data.RequestModels.Food
{
    public class RecipeRequestModel
    {
        public long FoodId { get; set; }
        public string RecipeName { get; set; }
        public string Step { get; set; }
        public string Description { get; set; }
    }
}
