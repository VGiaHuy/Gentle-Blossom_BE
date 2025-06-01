using Ganss.Xss;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Infrastructure;
using GentleBlossom_BE.Middleware;
using GentleBlossom_BE.Services;
using GentleBlossom_BE.Services.AnalysisService;
using GentleBlossom_BE.Services.GoogleService;
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

// Đăng ký Caching
builder.Services.AddResponseCaching();
builder.Services.AddMemoryCache();

// Đăng ký Google Drive
builder.Services.AddHttpClient(); // dùng để gọi API Google OAuth
builder.Services.AddScoped<GoogleDriveService>();

// Đăng ký HtmlSanitizer
builder.Services.AddSingleton<IHtmlSanitizer, HtmlSanitizer>();

// Đăng ký AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Đăng ký các Repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Đăng ký các Service
builder.Services.AddSingleton<InMemoryQueue<ExpertConnectionRequest>>(); // Hàng đợi trong bộ nhớ
builder.Services.AddHostedService<ExpertConnectionService>(); // Đăng ký Background Service
builder.Services.AddScoped<KeywordAnalysisService>(); // Dịch vụ phân tích từ khóa
builder.Services.AddScoped<HuggingFaceNlpService>(); // Dịch vụ Hugging Face API
builder.Services.AddScoped<PostAnalysisService>(); // Dịch vụ phân tích bài viết
builder.Services.AddScoped<ExpertConnectionService>(); // Dịch vụ kết nối chuyên gia (cho scope)

builder.Services.AddScoped<GoogleDriveService>();
builder.Services.AddScoped<UserAuthService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<UserProfileService>();
builder.Services.AddScoped<HealthJourneyService>();

builder.Services.AddScoped<ILoginUserRepository, LoginUserRepository>();
builder.Services.AddScoped<ICommentPostRepository, CommentPostRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IHealthJourneyRepository, HealthJourneyRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCaching();

// Đăng ký ExceptionMiddleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
