using Blog.Features.CreateBlog;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly IMediator _mediator;

    public BlogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{organization}/{name}")]
    [ProducesResponseType(typeof(Models.Blog), StatusCodes.Status200OK)]
    public Task<Models.Blog?> Get(string organization, string name, CancellationToken cancellationToken = default)
        => _mediator.Send(new Features.GetBlog.Request(organization, name), cancellationToken);

    [HttpPost("{organization}/{name}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Post(string organization, string name, CancellationToken cancellationToken = default)
    {
        var successful = await _mediator.Send(new Request(organization, name), cancellationToken);
        return successful ? CreatedAtAction(nameof(Get), new { organization, name }, null) : BadRequest();
    }
}