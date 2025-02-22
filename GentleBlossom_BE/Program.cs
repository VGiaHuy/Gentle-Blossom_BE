using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories;
using GentleBlossom_BE.Services.UserServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Đăng ký database
builder.Services.AddDbContext<GentleBlossomContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("GentleBlossom")),
    ServiceLifetime.Transient);

// Đăng ký các Repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Đăng ký các Service
builder.Services.AddScoped<UserAuthService>();


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
