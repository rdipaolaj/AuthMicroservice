using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ssptb.pe.tdlt.auth.api.Configuration;
using ssptb.pe.tdlt.auth.command.Login;

namespace ssptb.pe.tdlt.auth.api.Controllers;

[ApiVersion(1)]
[ApiController]
[Route("ssptbpetdlt/auth/api/v{v:apiVersion}/[controller]")]
public class LoginController : CustomController
{
    private readonly ILogger<LoginController> _logger;
    private readonly IMediator _mediator;

    public LoginController(IMediator mediator, ILogger<LoginController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
    {
        _logger.LogInformation("Attempting to authenticate user...");
        var result = await _mediator.Send(command);
        return OkorBadRequestValidationApiResponse(result);
    }
}
