using BusProyectApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// // Add authentication services to the container
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,

//             ValidIssuer = "Cerberus", // Hardcoded Issuer
//             ValidAudience = "Omega", // Hardcoded Audience
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MirandaLawsonIsTheBestCharacterInMassEffect")) // Hardcoded secret key
//         };
//     });
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("AdminOnly", policy => policy.RequireClaim("IsAdmin", "true"));
// });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
