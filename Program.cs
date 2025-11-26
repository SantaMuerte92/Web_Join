using Web_Join.Models;
using Web_Join.Repositories;
using Web_Join.Securitiy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Web_Join.Models.SecurityOptions>(builder.Configuration.GetSection("Security"));
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BigTicketSystemContext>();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(30);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

// Repository DI
builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IPriorityRepository, PriorityRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "admin-short",
    pattern: "Admin",
    defaults: new { controller = "Home", action = "Admin" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// OPTIONAL: einmaliges Seeding eines Admin-Users (nur temporär, danach entfernen)

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<BigTicketSystemContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    if (!ctx.Users.Any(u => u.Name == "Admin"))
    {
        var (hash, salt) = hasher.HashPassword("0000");
        ctx.Users.Add(new User
        {
            Name = "Admin",
            Email = "admin@example.local",
            Position = "Administrator",
            PasswordHash = hash,
            PasswordSalt = salt,
            PasswordChangedAt = DateTime.UtcNow
        });
        ctx.SaveChanges();
    }
}


app.Run();