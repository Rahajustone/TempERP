using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Samr.ERP.WebApi.Controllers
{
    [Route("api/[controller][action]")]
    [ApiController]
    //[Authorize]
    public class HideInfoController : ControllerBase
    {
        [Route("secretinfo")]
        [HttpGet]
        public ActionResult<IEnumerable<string>> SecretInfo()
        {
            return Ok(Enumerable.Range(1, 5).Select(p => $"secret info {p}"));
        }
    }
}