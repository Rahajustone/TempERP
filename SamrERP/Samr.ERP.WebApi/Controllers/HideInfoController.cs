using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller][action]")]
    [ApiController]
    //[Authorize]
    public class HideInfoController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> SecretInfo()
        {
            return Ok(Enumerable.Range(1, 5).Select(p => $"secret info {p}"));
        }
    }
}