using Blog.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Features.GetBlog;

public record Request(string Organization, string Name) : IRequest<Models.Blog?>;

public class RequestHandler : IRequestHandler<Request, Models.Blog?>
{
    private readonly AppDbContext _dbContext;

    public RequestHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Models.Blog?> Handle(Request request, CancellationToken cancellationToken)
    {
        var query = from blog in _dbContext.Blogs.TagWithCallSite().AsNoTracking()
                    where blog.Organization == request.Organization &&
                          blog.Name == request.Name
                    select new Models.Blog(blog.Id, blog.Organization, blog.Name);

        return query.FirstOrDefaultAsync(cancellationToken);
    }
}
