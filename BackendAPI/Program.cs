using BackendAPI.Data;
using BackendAPI.Models;
using BackendAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SendGrid;
using Swashbuckle.AspNetCore.Filters;
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

builder.Services.AddDbContext<DataContext>();
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

builder.Services.AddScoped<IAccountService, AccountService>();


#region Sendgrid Start
// ������á�˹���� SendGridClient
builder.Services.AddTransient<SendGridClient>(c =>
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    return new SendGridClient(configuration.GetSection("SendGrid:SendGridKey").Value);
});


#endregion SendGrid End

#region Identity���ҧ������� User,Role (���ѧ������§� ҴѺ)
builder.Services.AddIdentityCore<ApplicationUser>(opt =>
{
    opt.User.RequireUniqueEmail = true;
}).AddRoles<IdentityRole>()
.AddEntityFrameworkStores<DataContext>();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
#endregion
var app = builder.Build();

#region ���ҧ�ҹ�������ѵ��ѵ�
using var scope = app.Services.CreateScope(); //using ��ѧ� ҧҹ���稨ж١� ���¨ҡMemory
var context = scope.ServiceProvider.GetRequiredService<DataContext>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync(); //���ҧ DB ����ѵ��ѵԶ���ѧ�����
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
