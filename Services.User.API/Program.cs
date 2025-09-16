using Services.User.Application;
using Services.User.Application.Services;
using Services.User.Domain.Repositories;
using Services.User.Infrastructure;
using Services.User.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;    
using Microsoft.IdentityModel.Tokens;
using System.Text; 
using Shared.Common.Configuration; 

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<Shared.Common.Configuration.JwtSettings>();

if (jwtSettings == null)
{
    throw new InvalidOperationException("JWT ayarları bulunamadı! appsettings.json'da JwtSettings bölümünü kontrol edin.");
}

builder.Services.Configure<Shared.Common.Configuration.JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,          
            ValidateAudience = true,    
            ValidateLifetime = true,           
            ValidateIssuerSigningKey = true,   
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.Zero          
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();


var app = builder.Build();

app.UseAuthentication(); 
app.UseAuthorization();  

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
