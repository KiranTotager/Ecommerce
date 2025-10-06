using ECommerce.Data;
using ECommerce.Interfaces.IRepository;
using ECommerce.Interfaces.IRepository.ICMSRepos;
using ECommerce.Interfaces.IRepository.IUserRepos;
using ECommerce.Interfaces.IServices;
using ECommerce.Interfaces.IServices.ICMSServices;
using ECommerce.Interfaces.IServices.ICustomerServices;
using ECommerce.Interfaces.IUtils;
using ECommerce.Middlewares;
using ECommerce.Models;
using ECommerce.Repository;
using ECommerce.Repository.CMSRepos;
using ECommerce.Repository.UserRepos;
using ECommerce.Services;
using ECommerce.Services.CMSService;
using ECommerce.Services.UserServices;
using ECommerce.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.EnableAnnotations();
        c.SwaggerDoc("Authentication", new OpenApiInfo
        {
            Title = "Authentication API",
            Version = "v1",
            Description = "This controller provides authentication related APIs including user login with password or OTP, customer and staff registration, token refresh, email confirmation, and OTP generation.  These endpoints ensure secure access and user identity verification in the system."
        });
        c.SwaggerDoc("CMS", new OpenApiInfo
        {
            Title="CMS Api's",
            Version="v1",
            Description= "This set of CMS controllers provides APIs for managing the e-commerce content system. It includes endpoints to manage categories, coupons, staff, and products, allowing for creation, updating, retrieval, and deletion of CMS data. These APIs ensure secure and organized content management for the platform."
        });
        c.SwaggerDoc("Customer", new OpenApiInfo
        {
            Title="customer api's",
            Version="v1",
            Description= "This set of Customer controllers provides APIs for managing the customer-related operations in the e-commerce platform. It includes endpoints to handle customer profile management, cart operations, orders, and payments. These APIs ensure a seamless shopping experience, secure transactions, and efficient customer service across the platform"
        });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            In=ParameterLocation.Header,
            Name="Authorization",
            Description="jwt authorization using the bearer scheme",
            Scheme="Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference=new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer",
                    }
                },
                Array.Empty<string>()
            }
        });
        c.AddSecurityDefinition("GuestId",new OpenApiSecurityScheme
        {
            Type=SecuritySchemeType.ApiKey,
            Name="GuestId",
            In=ParameterLocation.Header,
            Description="Optional guest id for the anonymous user"
        });
    }
    );
builder.Services.AddVersionedApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}
    );
builder.Services.AddDbContext<ApplicationDbContext>(
    options=>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
    options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = true;
        options.User.RequireUniqueEmail = true;

    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IStaffRepository,StaffRepository>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IMailService,MailService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICoupenService,CoupenService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<ICartItemService,CartItemService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICoupenDetailRepository,CoupenDetailRepository>();
builder.Services.AddScoped<IUserConextService, UserContextService>();
builder.Services.AddScoped<IGuestRepository,GuestRepository>();
builder.Services.AddScoped<IWishListRepository, WishListRepository>();
builder.Services.AddScoped<IWishListService, WishListService>();
builder.Services.AddScoped<IContactUsRepository, ContactUsRepository>();
builder.Services.AddScoped<IContactUsService, ContactUsService>();
builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
    ).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime =true,
            ValidateIssuerSigningKey=true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                return Task.CompletedTask;
            }
        };
    }
    );

builder.Services.AddAuthorization(
    options =>
    {
        options.AddPolicy("AdminOrMangerOrHRPolicy", policy =>
        {
            policy.RequireRole("admin", "Manager", "HR");
        });
    }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        c =>
        {
            c.SwaggerEndpoint("/swagger/Authentication/swagger.json", "authentication");
            c.SwaggerEndpoint("/swagger/CMS/swagger.json", "CMS");
            c.SwaggerEndpoint("/swagger/Customer/swagger.json", "Customer");
            c.InjectStylesheet("/Swagger-custom/Swagger_custom.css");
            c.InjectJavascript("/Swagger-custom/swagger_custom.js");
        }

        );

}
app.UseExceptionHandlerMiddleware();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
