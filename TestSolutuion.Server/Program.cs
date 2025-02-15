using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TestSolutuion.Server.Domain.AuthService;
using TestSolutuion.Server.Domain.OrderManager;
using TestSolutuion.Server.Domain.ProductManager;
using TestSolutuion.Server.Domain.UserManager;
using TestSolutuion.Server.Database;
using TestSolutuion.Server.Database.Models;
using TestSolutuion.Server.Database.Repository.UnitOfWork;
using System.Security.Claims;
using TestSolutuion.Server.Domain.CustomerService;

var builder = WebApplication.CreateBuilder(args);

var specificoriginname = "AllowSpecificOrigins";

builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IOrderService,OrderService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRepositoryUnitOfWork,RepositoryUnitOfWork>();

builder.Services.AddDbContext<SQLiteDataBaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<SQLiteDataBaseContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var secretkey = builder.Configuration["JwtSettings:Key"];

    if (secretkey == null)
        throw new Exception("Secret key is null");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        RoleClaimType = ClaimTypes.Role,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey))
    };
});

var devbuild = builder.Environment.IsDevelopment();
if (devbuild)
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(specificoriginname, policy =>
        {
            policy.WithOrigins("https://localhost:58430")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });
}

builder.Services.AddControllers();

var app = builder.Build();

if (devbuild)
    app.UseCors(specificoriginname);

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

//who are you?
app.UseAuthentication();
//are you allowed?
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
