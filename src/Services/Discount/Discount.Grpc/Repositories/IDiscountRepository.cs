using System.Threading.Tasks;
using Discount.Grpc.Entities;

namespace Discount.Grpc.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);

        Task<bool> Create(Coupon coupon);
        
        Task<bool> Update(Coupon coupon);
        
        Task<bool> Delete(string productName);
    }
}