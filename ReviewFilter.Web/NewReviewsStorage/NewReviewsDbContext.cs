using Microsoft.EntityFrameworkCore;

namespace ReviewFilter.Web.NewReviewsStorage;

public class NewReviewsDbContext(DbContextOptions<NewReviewsDbContext> options) 
    : DbContext(options)
{
    public DbSet<NewReview> NewReviews { get; set; }
}