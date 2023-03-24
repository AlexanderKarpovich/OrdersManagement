namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private const string RequestHeader = "x-requestid";
        private readonly IMediator mediator;
        private readonly ILogger<OrdersController> logger;

        public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderSummary>), StatusCodes.Status200OK, "application/json")]
        public async Task<IEnumerable<OrderSummary>> GetOrdersAsync([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null, 
            [FromQuery] string? number = null, [FromQuery] int? providerId = null)
        {
            logger.LogInformation("---> Sending query {query}...", typeof(GetOrdersQuery));

            var orders = (await mediator.Send(new GetOrdersQuery())).AsQueryable();

            logger.LogInformation("---> Filter values: {startDate}, {endDate}, {number}, {providerId}", startDate, endDate, number, providerId);

            return FilterOrders(orders, startDate, endDate, number, providerId);
        }

        [HttpGet("numbers")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK, "application/json")]
        public async Task<IEnumerable<string>> GetOrdersNumbersAsync()
        {
            logger.LogInformation("---> Sending query {query}...", typeof(GetOrdersNumbersQuery));

            return await mediator.Send(new GetOrdersNumbersQuery());
        }

        [HttpGet("{orderId:int}")]
        [ActionName(nameof(GetOrderAsync))]
        [ProducesResponseType(typeof(OrderDetails), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDetails>> GetOrderAsync(int orderId)
        {
            try
            {
                var request = new GetOrderQuery(orderId);

                logger.LogInformation("---> Sending query {request} with parameter {parameterName} = {id}...",
                    request.GetType().Name, nameof(request.Id), request.Id);

                var order = await mediator.Send(request);

                return order;
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{orderId:int}/orderItems")]
        [ProducesResponseType(typeof(IEnumerable<OrderItemSummary>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderItemSummary>>> GetOrderItemsAsync(int orderId,
            [FromQuery] string? name = null, [FromQuery] string? unit = null)
        {
            try
            {
                var request = new GetOrderItemsQuery(orderId);

                logger.LogInformation("---> Sending query {request} with parameter {parameterName} = {id}...",
                    request.GetType().Name, nameof(request.OrderId), request.OrderId);

                var orderItems = (await mediator.Send(request)).AsQueryable();

                return Ok(FilterOrderItems(orderItems, name, unit));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{orderId:int}/names")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderItemSummary>>> GetOrderItemsNamesAsync(int orderId)
        {
            try
            {
                var request = new GetOrderItemsNamesQuery(orderId);

                logger.LogInformation("---> Sending query {request} with parameter {parameterName} = {id}...",
                    request.GetType().Name, nameof(request.OrderId), request.OrderId);

                var orderItemsNames = await mediator.Send(request);

                return Ok(orderItemsNames);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("{orderId:int}/units")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK, "application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderItemSummary>>> GetOrderItemsUnitsAsync(int orderId)
        {
            try
            {
                var request = new GetOrderItemsUnitsQuery(orderId);

                logger.LogInformation("---> Sending query {request} with parameter {parameterName} = {id}...",
                    request.GetType().Name, nameof(request.OrderId), request.OrderId);

                var orderItemsUnits = await mediator.Send(request);
                
                return Ok(orderItemsUnits);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderDetailsDto), StatusCodes.Status201Created, "application/json")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateOrderAsync(CreateOrderCommand createOrderCommand, [FromHeader(Name = RequestHeader)] Guid requestId)
        {
            if (requestId == Guid.Empty)
            {
                return BadRequest();
            }

            var request = new IdentifiedCommand<CreateOrderCommand, OrderDetailsDto>(createOrderCommand, requestId);

            logger.LogInformation("---> Sending command: {commandName}...", createOrderCommand.GetType().Name);

            var order = await mediator.Send(request);

            return order is null ?
                BadRequest() :
                CreatedAtAction(nameof(GetOrderAsync), new { orderId = order.Id }, order);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateOrderAsync(UpdateOrderCommand updateOrderCommand, [FromHeader(Name = RequestHeader)] Guid requestId)
        {
            var request = new IdentifiedCommand<UpdateOrderCommand, bool>(updateOrderCommand, requestId);

            logger.LogInformation("---> Sending command: {commandName}...", updateOrderCommand.GetType().Name);

            bool commandResult = false;
            try
            {
                commandResult = await mediator.Send(request);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(ex);
            }

            return !commandResult ? NotFound() : NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOrderAsync(DeleteOrderCommand deleteOrderCommand, [FromHeader(Name = RequestHeader)] Guid requestId)
        {
            var request = new IdentifiedCommand<DeleteOrderCommand, bool>(deleteOrderCommand, requestId);

            logger.LogInformation("---> Sending command: {commandName}...", deleteOrderCommand.GetType().Name);

            bool commandResult = false;
            try
            {
                commandResult = await mediator.Send(request);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Conflict(ex);
            }

            return !commandResult ? NotFound() : Ok();
        }

        private IEnumerable<OrderSummary> FilterOrders(IQueryable<OrderSummary> orders, 
            DateTime? startDate, DateTime? endDate, 
            string? number, int? providerId)
        {
            orders = startDate is null ? orders : orders.Where(o => o.Date >= startDate);
            orders = endDate is null ? orders : orders.Where(o => o.Date <= endDate);
            orders = number is null ? orders : orders.Where(o => o.Number == number);
            orders = providerId is null ? orders : orders.Where(o => o.ProviderId == providerId);

            return orders.ToList();
        }

        private IEnumerable<OrderItemSummary> FilterOrderItems(IQueryable<OrderItemSummary> orderItems, string? name, string? unit)
        {
            orderItems = name is null ? orderItems : orderItems.Where(oi => oi.Name == name);
            orderItems = unit is null ? orderItems : orderItems.Where(oi => oi.Unit == unit);

            return orderItems.ToList();
        }
    }
}