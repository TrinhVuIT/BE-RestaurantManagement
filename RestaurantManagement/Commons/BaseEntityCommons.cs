using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace RestaurantManagement.Commons
{
    public class BaseEntityCommons : BaseEntity
    {
        public bool IsDeleted { get; set; }
        public string? NguoiTao { get; set; }
        public DateTime? NgayTao { get; set; }
        public string? NguoiCapNhat { get; set; }
        public DateTime? NgayCapNhat { get; set; }

        public virtual void PrepareSave(IHttpContextAccessor httpContextAccessor, EntityState state)
        {
            var identityName = httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            var now = DateTime.Now;
            string nguoiTao = string.IsNullOrEmpty(NguoiTao) ? "Unknown" : NguoiTao;
            if(state == EntityState.Added)
            {
                NguoiTao = identityName ?? nguoiTao;
                NgayTao = now;
            }
            string nguoiCapNhat = string.IsNullOrEmpty(NguoiCapNhat) ? "Unknown" : NguoiCapNhat;
            NguoiCapNhat = identityName ?? nguoiCapNhat;
            NgayCapNhat = now;
        }
    }
}
