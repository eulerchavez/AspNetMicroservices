using System.Net;
using System.Threading.Tasks;
using Discount.Api.Entities;
using Discount.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Discount.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountController(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        [HttpGet("{{productName}}", Name = "GetDiscount")]
        [ProducesResponseType(typeof(Coupon), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            var coupon = await _discountRepository.GetDiscount(productName);

            return Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> Create([FromBody] Coupon coupon)
        {
            await _discountRepository.Create(coupon);

            return CreatedAtRoute("GetDiscount", new {coupon.ProductName});
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(Coupon), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> Update([FromBody] Coupon coupon)
        {
            await _discountRepository.Update(coupon);

            return Ok(coupon);
        }
        
        [HttpDelete("{{productName}}")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> Delete(string productName)
        {
            var response= await _discountRepository.Delete(productName);

            return Ok(response);
        }
    }
}