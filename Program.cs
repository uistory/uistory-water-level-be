using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StatusApi.Models;
using Swashbuckle.AspNetCore.Filters;
using water_level_dotnetcore_api.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);
var defaultConnection = "Server=tcp:water-level.database.windows.net,1433;Initial Catalog=water-level-db;Persist Security Info=False;User ID=petres;Password=F1c15925;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ISensorStatusService, SensorStatusService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddDbContext<DataContext>(options =>
     options.UseSqlServer(defaultConnection));
// builder.Services.AddDbContext<DataContext>(opt =>
//   opt.UseInMemoryDatabase("TestDb"));

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 0;
}).AddEntityFrameworkStores<DataContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.{
app.UseSwagger();
app.UseSwaggerUI();

app.MapIdentityApi<IdentityUser>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
