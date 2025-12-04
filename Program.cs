using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopWeb.Data;
using ShopWeb.Models;
using ShopWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// ===========================
// 1. Lấy PORT do Render cung cấp
// ===========================
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";

// ===========================
// 2. Lựa chọn DB (MySQL)
// ===========================
var databaseProvider = builder.Configuration["DatabaseProvider"] ?? "MySql";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (databaseProvider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
    {
        var sqliteConnection = builder.Configuration.GetConnectionString("SqliteConnection") 
                               ?? "Data Source=pcshop_dev.db";
        options.UseSqlite(sqliteConnection);
    }
    else
    {
        var mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection is not configured.");

        options.UseMySql(
            mySqlConnection,
            new MySqlServerVersion(new Version(8, 0, 32)),
            mySqlOptions =>
            {
                mySqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            }
        );
    }
});

// ===========================
// 3. Identity
// ===========================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ===========================
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// ===========================
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ===========================
// AUTO MIGRATE DATABASE ON STARTUP
// ===========================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        logger.LogInformation("Checking database connection...");
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Try to apply migrations, but don't fail if tables already exist
        var pendingMigrations = context.Database.GetPendingMigrations().ToList();
        if (pendingMigrations.Any())
        {
            logger.LogInformation($"Found {pendingMigrations.Count} pending migrations. Attempting to apply...");
            try
            {
                context.Database.Migrate();
                logger.LogInformation("Database migration completed successfully.");
            }
            catch (MySqlConnector.MySqlException ex) when (ex.ErrorCode == MySqlConnector.MySqlErrorCode.TableAccessDenied || ex.ErrorCode == MySqlConnector.MySqlErrorCode.DuplicateKeyName)
            {
                logger.LogWarning(ex, "Migration skipped - tables may already exist from previous setup.");
            }
        }
        else
        {
            logger.LogInformation("No pending migrations. Database is up to date.");
        }
        
        // Seed data if needed
        logger.LogInformation("Starting data seeding...");
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        DbSeeder.SeedAsync(context, userManager, roleManager).GetAwaiter().GetResult();
        logger.LogInformation("Data seeding completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "ERROR during database setup: {Message}", ex.Message);
        // Don't throw - let app start anyway so we can debug
    }
}

// ===========================
// 4. BẮT BUỘC: Kestrel lắng nghe PORT Render
// ===========================
app.Urls.Clear();
app.Urls.Add($"http://0.0.0.0:{port}");

// ===========================
// 5. Error handling - Always use production mode on Render
// ===========================
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // Log all unhandled exceptions
    app.Use(async (context, next) =>
    {
        try
        {
            await next();
        }
        catch (Exception ex)
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Unhandled exception for request {Path}", context.Request.Path);
            throw;
        }
    });
}

// ===========================
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ===========================
// 6. Endpoint CHECK DB TEST
// ===========================
app.MapGet("/check-db", async (ApplicationDbContext context) =>
{
    try
    {
        await context.Database.OpenConnectionAsync();
        await context.Database.CloseConnectionAsync();
        
        // Check if tables exist
        var canConnect = await context.Database.CanConnectAsync();
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
        
        return Results.Text($"MySQL Connection OK!\n" +
            $"Can Connect: {canConnect}\n" +
            $"Applied Migrations: {appliedMigrations.Count()}\n" +
            $"Pending Migrations: {pendingMigrations.Count()}\n" +
            $"Migrations: {string.Join(", ", appliedMigrations)}");
    }
    catch (Exception ex)
    {
        return Results.Text($"Error: {ex.Message}\n{ex.StackTrace}");
    }
});

// ===========================
// 7. Routing
// ===========================
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}",
    defaults: new { area = "Admin" });

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
