using Microsoft.AspNetCore.Mvc;
using PortfolioApp.Infrastructure.Helpers;
using StackExchange.Exceptional;

namespace Tambien.Controllers
{
    [ApiController]
    public class ExceptionsController : ControllerBase
    {
        private readonly IBackofficeUserAccessor _backofficeUserAccessor;

        public ExceptionsController(IBackofficeUserAccessor backofficeUserAccessor)
        {
            _backofficeUserAccessor = backofficeUserAccessor;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("exceptions/{*guid}")]
        public async Task Exceptions()
        {
            if (_backofficeUserAccessor.isLoggedIn)
            {
                await ExceptionalMiddleware.HandleRequestAsync(HttpContext);
            }
        }
    }
}
