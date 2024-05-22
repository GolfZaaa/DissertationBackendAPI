using Autofac;
using Autofac.Extensions.DependencyInjection;
using BackendAPI.Data;
using BackendAPI.Models;
using BackendAPI.Services;
using BackendAPI.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SendGrid;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

//builder.Services.AddDbContext<DataContext>();

// เพิ่มขึ้นมา
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});


builder.Services.AddMemoryCache();

#region Start JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new
    TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = false,
        IssuerSigningKey = new
    SymmetricSecurityKey(Encoding.UTF8
    .GetBytes(builder.Configuration["JWTSettings:TokenKey"]))
    };
});
builder.Services.AddScoped<TokenService>();
#endregion End JWT
builder.Services.AddHttpContextAccessor();

#region Sendgrid Start
// เพิ่มการกำหนดค่า SendGridClient
builder.Services.AddTransient<SendGridClient>(c =>
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    return new SendGridClient(configuration.GetSection("SendGrid:SendGridKey").Value);
});


#endregion SendGrid End

#region Identityสร้างเซอร์วิส User,Role (ระวังการเรียงล าดับ)
builder.Services.AddIdentityCore<ApplicationUser>(opt =>
{
    opt.User.RequireUniqueEmail = true;
}).AddRoles<IdentityRole>()
.AddEntityFrameworkStores<DataContext>();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
#endregion


builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));


//ใช้ AutoRefac ลงทะเบียนโดยอัตโนมัติกรณีมีหลายๆ Service
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(containerBuilder =>
{
    containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
    .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Test"))
    .AsImplementedInterfaces();
}));
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

#region สร้างฐานข้อมูลอัตโนมัติ
using var scope = app.Services.CreateScope(); //using หลังท างานเสร็จจะถูกท าลายจากMemory
var context = scope.ServiceProvider.GetRequiredService<DataContext>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync(); //สร้าง DB ให้อัตโนมัติถ้ายังไม่มี
}
catch (Exception ex)
{
    logger.LogError(ex, "Problem migrating data");
}
#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//ต้องใส่ ถ้าไม่ใส่จะหาไฟล์รูปภาพไม่เจอ
app.UseStaticFiles();
app.UseCors("corsapp");

// เพิ่มขึ้นมา
app.MapFallbackToController("Index", "Fallback");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
