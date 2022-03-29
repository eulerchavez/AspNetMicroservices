using System.Threading.Tasks;
using Discount.Api.Entities;

namespace Discount.Api.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);

        Task<bool> Create(Coupon coupon);
        
        Task<bool> Update(Coupon coupon);
        
        Task<bool> Delete(string productName);
    }
}