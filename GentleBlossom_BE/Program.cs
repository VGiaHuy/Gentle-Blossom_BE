using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Middleware;
using GentleBlossom_BE.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
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

// Tắt tự động validate dữ liệu nhận từ BE
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Đăng ký AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Đăng ký các Repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Đăng ký các Service
builder.Services.AddScoped<UserAuthService>();
builder.Services.AddScoped<ILoginUserRepository, LoginUserRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Đăng ký ExceptionMiddleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
