using main_dotnet_api.CQRS.Carts.Commands;
using main_dotnet_api.CQRS.Carts.Queries;
using main_dotnet_api.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace main_dotnet_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string GetUserId() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        [HttpGet]
        [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CartDto>> GetCart()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            var cart = await _mediator.Send(new GetCartByUserIdQuery(userId));

            if (cart == null)
            {
                return Ok(new CartDto { UserId = userId });
            }

            return Ok(cart);
        }

        [HttpPost("add-item")]
        [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<ActionResult<CartDto>> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User not authenticated");

                var cart = await _mediator.Send(new AddToCartCommand(userId, addToCartDto));
                return Ok(cart);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", details = ex.Message });
            }
        }

        [HttpPut("update-item")]
        [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CartDto>> UpdateCartItem([FromBody] UpdateCartItemDto updateCartItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User not authenticated");

                var cart = await _mediator.Send(new UpdateCartItemCommand(userId, updateCartItemDto));
                return Ok(cart);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("remove-item/{cartItemId}")]
        [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartDto>> RemoveFromCart(int cartItemId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            try
            {
                var cart = await _mediator.Send(new RemoveFromCartCommand(userId, cartItemId));
                return Ok(cart);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("clear")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ClearCart()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            var result = await _mediator.Send(new ClearCartCommand(userId));
            return Ok(new { success = result, message = "Cart cleared successfully" });
        }

        [HttpGet("items-count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int>> GetCartItemsCount()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            var count = await _mediator.Send(new GetCartItemsCountQuery(userId));
            return Ok(count);
        }

        [HttpGet("total")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<decimal>> GetCartTotal()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            var total = await _mediator.Send(new GetCartTotalQuery(userId));
            return Ok(total);
        }
    }
}