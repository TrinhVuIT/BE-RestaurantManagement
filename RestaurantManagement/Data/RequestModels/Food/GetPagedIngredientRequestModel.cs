namespace RestaurantManagement.Data.RequestModels.Food
{
    public class GetPagedIngredientRequestModel : BasePaginationRequestModel
    {
        public DateTime? Exp {  get; set; }
    }
}
