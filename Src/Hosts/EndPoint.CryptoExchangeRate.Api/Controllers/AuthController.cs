using System.Security.Claims;
using System.Text;
using Core.CryptoExchangeRate.Application.AuthServices.Login;
using Core.CryptoExchangeRate.Application.AuthServices.Signup;
using EndPoint.CryptoExchangeRate.Api.Framework;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.CryptoExchangeRate.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : BaseController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommandRequest request)
    {
        var result = await _mediator.Send(request);

        return EXResult(result);
    }

    [HttpPost("signup")]
    public async Task<IActionResult> signup([FromBody] SignUpCommandRequest request)
    {
        var result = await _mediator.Send(request);

        return EXResult(result);
    }
}