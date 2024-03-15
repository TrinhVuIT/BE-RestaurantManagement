namespace RestaurantManagement.Data.RequestModels.Food
{
    public class GetPagedIngredientDetailRequestModel : BasePaginationRequestModel
    {
        public long RecipeId { get; set; }
        public string? IngredientName { get; set; }
        public DateTime? Exp {  get; set; }
    }
}
