namespace RestaurantManagement.Data.RequestModels.Food
{
    public class GetPagedRecipeRequestModel : BasePaginationRequestModel
    {
        public string? FoodName { get; set; }
        public string? IngredientName { get; set;}
    }
}
