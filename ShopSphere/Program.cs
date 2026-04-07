using Microsoft.EntityFrameworkCore;
using ShopSphere.API.Services;
using ShopSphere.Data;
using ShopSphere.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddTransient<ISellerService, SellerService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IInventoryService, InventoryService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<IShipmentService, ShipmentService>();
builder.Services.AddTransient<IReturnService, ReturnService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IProductAttributeService, ProductAttributeService>();
builder.Services.AddTransient<IDisputeService,DisputeService>();
builder.Services.AddTransient<IAnalyticsService, AnalyticsService>();
builder.Services.AddTransient<IAuditService, AuditService>();
builder.Services.AddTransient<ISellerStoreService, SellerStoreService>();
builder.Services.AddTransient<ICommissionService, CommissionService>();
builder.Services.AddTransient<IPolicyService, PolicyService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
