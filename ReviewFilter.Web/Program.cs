using Microsoft.EntityFrameworkCore;
using ReviewFilter.ThirdParty.OpenApi;
using ReviewFilter.Web.NewReviewsStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddOpenApi(builder.Configuration["ApiKey"]!);
builder.Services.AddDbContext<NewReviewsDbContext>(opts=>
    opts.UseSqlite(builder.Configuration.GetConnectionString("NewReviews")));

var app = builder.Build();

app.Services.CreateScope().ServiceProvider.GetRequiredService<NewReviewsDbContext>().Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();