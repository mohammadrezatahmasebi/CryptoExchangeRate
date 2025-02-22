using System.Net;
using Core.CryptoExchangeRate.Domain.Framework;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.CryptoExchangeRate.Api.Framework;

[Route("[controller]")]
[ApiController]
public class BaseController: ControllerBase
{
    [NonAction]
    protected virtual ObjectResult EXResult(Result result)
    {
        Response.StatusCode = result.IsSuccess ? HttpStatusCode.OK.GetHashCode() : result.Error.Code.GetHashCode();
        return StatusCode(Response.StatusCode, result);
    }
}
