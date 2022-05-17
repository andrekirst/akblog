using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        public Task<Blog> Get(string organization, string name, CancellationToken cancellationToken = default);
    }
}
