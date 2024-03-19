using RestaurantManagement.Commons;

namespace RestaurantManagement.Data.Entities
{
    public class PurchaseOrderFood : BaseEntityCommons
    {
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        //Món ăn
        public virtual Food Food { get; set; }
        //Số lượng món ăn
        public int Quantity { get; set; }
    }
}
