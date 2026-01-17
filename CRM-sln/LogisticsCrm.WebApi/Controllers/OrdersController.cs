using LogisticsCrm.Application.Abstractions;
using LogisticsCrm.Application.Services;
using LogisticsCrm.Domain.Entities;
using LogisticsCrm.Domain.Enums;
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

        public OrdersController(
            IOrderRepository orderRepository,
            IClientRepository clientRepository,
            ITrackingNumberGenerator trackingNumberGenerator,
            IOrderStatusHistoryRepository historyRepository)
        {
            _orderRepository = orderRepository;
            _clientRepository = clientRepository;
            _trackingNumberGenerator = trackingNumberGenerator;
            _historyRepository = historyRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderResponseDto>>> GetAll(CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllAsync(cancellationToken);
            return Ok(orders.Select(o => o.ToDto()).ToList());
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

            var dto = order.ToDto();
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
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

            switch (newStatus)
            {
                case OrderStatus.Assigned: order.Assign(); break;
                case OrderStatus.PickedUp: order.MarkPickedUp(); break;
                case OrderStatus.InTransit: order.MarkInTransit(); break;
                case OrderStatus.Delivered: order.MarkDelivered(); break;
                case OrderStatus.Canceled: order.Cancel(); break;
                case OrderStatus.Created:
                default:
                    return BadRequest("Invalid status transition.");
            }

            // пишемо в історію
            var record = new OrderStatusHistory(
                order.Id,
                fromStatus,
                order.Status,
                comment: null);

            await _historyRepository.AddAsync(record, cancellationToken);

            // один SaveChanges достатній, бо репозиторії використовують той самий DbContext
            await _orderRepository.SaveChangesAsync(cancellationToken);

            return Ok(order.ToDto());
        }
        [HttpGet("{id:guid}/history")]
        public async Task<ActionResult<List<object>>> GetHistory(Guid id, CancellationToken cancellationToken)
        {
            var history = await _historyRepository.GetByOrderIdAsync(id, cancellationToken);

            var result = history.Select(x => new
            {
                x.Id,
                x.OrderId,
                FromStatus = (int)x.FromStatus,
                ToStatus = (int)x.ToStatus,
                x.ChangedAtUtc,
                x.Comment
            }).ToList();

            return Ok(result);
        }

    }
}
