namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvidersController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<ProvidersController> logger;

        public ProvidersController(IMediator mediator, ILogger<ProvidersController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProviderSummary>), StatusCodes.Status200OK, "application/json")]
        public async Task<IEnumerable<ProviderSummary>> GetProvidersAsync()
        {
            logger.LogInformation("---> Sending query {query}...", typeof(GetProvidersQuery));

            return await mediator.Send(new GetProvidersQuery());
        }
    }
}