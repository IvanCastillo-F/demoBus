using BusProyectApi.Data;
using BusProyectApi.Data.Interfaces;
using BusProyectApi.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Test

builder.Services.AddControllers();
// Add authentication services to the container
builder.Services.AddAuthentication("Bearer") //JwtBearerDefaults.AuthenticationScheme?
    .AddJwtBearer(options =>
    {

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                // Skip the default behavior
                context.HandleResponse();

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    StatusCode = 401,
                    Message = "Unauthorized access: Bearer token is missing or invalid."
                };

                var errorJson = JsonSerializer.Serialize(errorResponse);

                return context.Response.WriteAsync(errorJson);
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "Cerberus", // Hardcoded Issuer
            ValidAudience = "Omega", // Hardcoded Audience
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MirandaLawsonIsTheBestCharacterInMassEffect")) // Hardcoded secret key
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("IsAdmin", "true"));
});

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500") // Reminder: Frontend URL goes here
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//
//This is a comment
builder.Services.AddDbContext<ApplicationDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
.EnableSensitiveDataLogging());

//Add Route Methods to the proyect
builder.Services.AddTransient<IRouteRepository, RouteRepository>();

//Add Booking Methods to the proyect
builder.Services.AddTransient<IBookingRepository, BookingRepository>();

//

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
