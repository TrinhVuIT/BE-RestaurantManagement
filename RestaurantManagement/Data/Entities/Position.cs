using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities
{
    public class Position : BaseEntityCommons
    {
        //Tên chức vụ
        public string PositionName { get; set; }
        //Mô tả
        public string? Describe { get; set; }
    }
}
