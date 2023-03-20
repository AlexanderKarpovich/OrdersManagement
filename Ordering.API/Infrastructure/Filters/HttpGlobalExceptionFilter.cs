namespace Ordering.API.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment environment;
        private readonly ILogger<HttpGlobalExceptionFilter> logger;

        public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.environment = env;
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogError(new EventId(context.Exception.HResult), context.Exception, context.Exception.Message);

            if (context.Exception is OrderingDomainException)
            {
                var problemDetails = new ValidationProblemDetails()
                {
                    Instance = context.HttpContext.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Refer to the errors property to see additional details"
                };

                problemDetails.Errors.Add("DomainValidations", new string[] { context.Exception.Message });

                context.Result = new BadRequestObjectResult(problemDetails);
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else
            {
                const string serverError = "Internal server error. Try again";
                
                if (environment.IsDevelopment())
                {
                    context.Result = new ObjectResult(
                        new 
                        { 
                            Message = serverError,
                            DeveloperMessage = context.Exception.Message
                        });
                }
                else
                {
                    context.Result = new ObjectResult(new { Message = serverError });
                }

                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            context.ExceptionHandled = true;
        }
    }
}