using Blog.ValueObjects;

namespace Blog.Features.GetBlog;

public record Request(BlogOrganization Organization, BlogName Name);
