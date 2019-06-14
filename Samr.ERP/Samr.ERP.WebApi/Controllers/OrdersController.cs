using System.Web.Http;
using Samr.ERP.WebApi.Infrastructure;

namespace Samr.ERP.WebApi.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        [Authorize(Roles = "IncidentResolvers")]
        [HttpPut]
        [Route("refund/{orderId}")]
        public IHttpActionResult RefundOrder([FromUri]string orderId)
        {
            return Ok();
        }

        [ClaimsAuthorization(ClaimType="FTE", ClaimValue="1")]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}
