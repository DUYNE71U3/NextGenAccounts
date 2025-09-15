using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using main_dotnet_api.CQRS.Orders.Commands;
using main_dotnet_api.CQRS.Orders.Queries;
using main_dotnet_api.DTOs;
using System.Security.Claims;

namespace main_dotnet_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _mediator.Send(new GetAllOrdersQuery());
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(int id)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(id));
            if (order == null)
                return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && order.UserId != currentUserId)
                return Forbid();

            return Ok(order);
        }

        [HttpGet("number/{orderNumber}")]
        public async Task<ActionResult<OrderDto>> GetOrderByNumber(string orderNumber)
        {
            var order = await _mediator.Send(new GetOrderByNumberQuery(orderNumber));
            if (order == null)
                return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && order.UserId != currentUserId)
                return Forbid();

            return Ok(order);
        }

        [HttpGet("my-orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetMyOrders()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized();

            var orders = await _mediator.Send(new GetOrdersByUserIdQuery(currentUserId));
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized();

            try
            {
                var order = await _mediator.Send(new CreateOrderCommand(currentUserId, orderDto));
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create-from-cart")]
        public async Task<ActionResult<OrderDto>> CreateOrderFromCart([FromBody] CreateOrderFromCartRequest request)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized();

            try
            {
                var order = await _mediator.Send(new CreateOrderFromCartCommand(currentUserId, request.Notes));
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<OrderDto>> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto statusDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var order = await _mediator.Send(new UpdateOrderStatusCommand(id, statusDto));
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _mediator.Send(new DeleteOrderCommand(id));
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    public class CreateOrderFromCartRequest
    {
        public string? Notes { get; set; }
    }
}