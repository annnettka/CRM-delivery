using LogisticsCrm.Application.Abstractions;
using LogisticsCrm.Application.Services;
using LogisticsCrm.Domain.Entities;
using LogisticsCrm.Domain.Enums;
using LogisticsCrm.WebApi.Dtos.Common;
using LogisticsCrm.WebApi.Dtos.Orders;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsCrm.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ITrackingNumberGenerator _trackingNumberGenerator;
        private readonly IOrderStatusHistoryRepository _historyRepository;
        private readonly ICourierRepository _courierRepository;

        public OrdersController(
            IOrderRepository orderRepository,
            IClientRepository clientRepository,
            ITrackingNumberGenerator trackingNumberGenerator,
            IOrderStatusHistoryRepository historyRepository,
            ICourierRepository courierRepository)
        {
            _orderRepository = orderRepository;
            _clientRepository = clientRepository;
            _trackingNumberGenerator = trackingNumberGenerator;
            _historyRepository = historyRepository;
            _courierRepository = courierRepository;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<OrderResponseDto>>> GetAll(
            [FromQuery] GetOrdersQuery query,
            CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _orderRepository.SearchAsync(
                query.Status,
                query.CourierId,
                query.ClientId,
                query.Search,
                query.Page,
                query.PageSize,
                cancellationToken);

            return Ok(new PagedResult<OrderResponseDto>
            {
                Items = items.Select(o => o.ToDto()).ToList(),
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
            });
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
            if (order == null)
                return NotFound();

            return Ok(order.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> Create(
            [FromBody] CreateOrderRequest request,
            CancellationToken cancellationToken)
        {
            var client = await _clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
            if (client == null)
                return BadRequest($"ClientId '{request.ClientId}' not found.");

            var tracking = _trackingNumberGenerator.Generate();

            var order = new Order(
                request.ClientId,
                tracking,
                request.PickupAddress,
                request.DeliveryAddress,
                request.RecipientName,
                request.RecipientPhone,
                request.Price);

            await _orderRepository.AddAsync(order, cancellationToken);
            await _orderRepository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order.ToDto());
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<ActionResult<OrderResponseDto>> UpdateStatus(
            Guid id,
            [FromBody] UpdateOrderStatusRequest request,
            CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdForUpdateAsync(id, cancellationToken);
            if (order == null)
                return NotFound();

            var fromStatus = order.Status;
            var newStatus = (OrderStatus)request.Status;

            try
            {
                order.ChangeStatus(newStatus);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            var record = new OrderStatusHistory(
                order.Id,
                fromStatus,
                order.Status,
                comment: null);

            await _historyRepository.AddAsync(record, cancellationToken);
            await _orderRepository.SaveChangesAsync(cancellationToken);

            return Ok(order.ToDto());
        }

        [HttpGet("{id:guid}/history")]
        public async Task<ActionResult<List<object>>> GetHistory(Guid id, CancellationToken cancellationToken)
        {
            var history = await _historyRepository.GetByOrderIdAsync(id, cancellationToken);

            return Ok(history.Select(x => new
            {
                x.Id,
                x.OrderId,
                FromStatus = (int)x.FromStatus,
                ToStatus = (int)x.ToStatus,
                x.ChangedAtUtc,
                x.Comment
            }));
        }

        [HttpPatch("{id:guid}/assign-courier")]
        public async Task<ActionResult<OrderResponseDto>> AssignCourier(
            Guid id,
            [FromBody] AssignCourierRequest request,
            CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdForUpdateAsync(id, cancellationToken);
            if (order == null)
                return NotFound();

            var courier = await _courierRepository.GetByIdAsync(request.CourierId, cancellationToken);
            if (courier == null)
                return BadRequest($"CourierId '{request.CourierId}' not found.");

            try
            {
                order.AssignCourier(request.CourierId);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            await _orderRepository.SaveChangesAsync(cancellationToken);
            return Ok(order.ToDto());
        }
    }
}
