using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FinalProjectBackend.Data;
using System.Text.Json.Serialization;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FinalProjectBackend.Model;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FinalProjectBackendContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChallengeTrackerConnection")).EnableSensitiveDataLogging());

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
var authkey = configuration.GetValue<string>("Appsettings:Token");
var _jwtSettings = configuration.GetSection("Appsettings");
builder.Services.Configure<JWTSettings>(_jwtSettings);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyMethod().
                        WithOrigins("http://localhost:3001", "http://localhost:3000",
                                              "http://www.contoso.com").AllowAnyHeader();
                      });
});


builder.Services.AddControllers()
.AddJsonOptions(opt =>
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(item =>
{
    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item =>
{
    item.RequireHttpsMetadata = true;
    item.SaveToken = true;
    item.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authkey)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

var app = builder.Build();
Console.WriteLine(authkey);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



//using Microsoft.EntityFrameworkCore;
//using System.Text.Json.Serialization;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container
//builder.Services.AddControllers()
//.AddJsonOptions(opt =>
//                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<PublisherData.PubContext>(
//    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("PubConnection"))
//    .EnableSensitiveDataLogging()
//    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseAuthorization();
//app.MapControllers();
//app.Run();
