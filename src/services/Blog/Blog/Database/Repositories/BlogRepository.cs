using Infrastructure.Database;

namespace Blog.Database.Repositories;

public interface IBlogRepository : IRepository<Models.Blog>
{
}

public class BlogRepository : BaseRepository<Models.Blog>, IBlogRepository
{
    public BlogRepository(AppDbContext dbContext)
        : base(dbContext.Blogs)
    {
    }
}