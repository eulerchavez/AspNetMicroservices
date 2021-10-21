using System.Net;
using System.Threading.Tasks;
using Basket.Api.Entities;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;

        public BasketController(IBasketRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var shoppingCart = await _repository.GetBasket(userName);

            return Ok(shoppingCart ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart shoppingCart)
        {
            shoppingCart = await _repository.UpdateBasket(shoppingCart);

            return Ok(shoppingCart);
        }

        [HttpDelete]
        public async Task DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
        }
    }
}