using Services.User.Application;
using Services.User.Application.Services;
using Services.User.Domain.Repositories;
using Services.User.Infrastructure;
using Services.User.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Infrastructure katmanını ekle
builder.Services.AddInfrastructure(builder.Configuration);

// Application katmanını ekle
builder.Services.AddApplication();
builder.Services.AddScoped<IUserAddressService, UserAddressService>();
builder.Services.AddScoped<IUserAddressRepository, UserAddressRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
