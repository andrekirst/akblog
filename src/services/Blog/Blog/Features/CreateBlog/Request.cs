using Blog.Database.Repositories;
using Blog.Messages;
using Infrastructure.Database;
using MassTransit;
using MediatR;

namespace Blog.Features.CreateBlog;

public record Request(string Organization, string Name) : IRequest<bool>;

public class RequestHandler : IRequestHandler<Request, bool>
{
    private readonly IBlogRepository _blogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public RequestHandler(
        IBlogRepository blogRepository,
        IUnitOfWork unitOfWork,
        IPublishEndpoint publishEndpoint)
    {
        _blogRepository = blogRepository;
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
    {
        var (organization, name) = request;
        var blog = new Database.Models.Blog
        {
            Organization = organization,
            Name = name
        };
        await _blogRepository.AddAsync(blog, cancellationToken);
        var affectedRows = await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _publishEndpoint.Publish(new BlogCreated(organization, name), cancellationToken);
        return affectedRows is 1;
    }
}