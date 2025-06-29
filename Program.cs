using Ganss.Xss;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Data.Settings;
using GentleBlossom_BE.Infrastructure;
using GentleBlossom_BE.Middleware;
using GentleBlossom_BE.Services.AdminServices;
using GentleBlossom_BE.Services.AnalysisService;
using GentleBlossom_BE.Services.GoogleService;
using GentleBlossom_BE.Services.Hubs;
using GentleBlossom_BE.Services.JWTService;
using GentleBlossom_BE.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Đọc cấu hình từ appsettings.json
var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
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
builder.Services.AddHttpClient();
builder.Services.AddScoped<GoogleDriveService>();

// Thêm CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins(apiSettings.AllowedOrigins)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials(); // Bắt buộc để SignalR hoạt động
    });
});

// đăng ký dịch vụ JWT
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.RequireHttpsMetadata = builder.Configuration.GetValue<bool>("JwtConfig:RequireHttps");
    option.SaveToken = true;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
    option.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers["Token-Expired"] = "true";
            }
            return Task.CompletedTask;
        },
    };
});

// Thêm SignalR
builder.Services.AddSignalR();

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

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<GoogleDriveService>();
builder.Services.AddScoped<UserAuthService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<UserProfileService>();
builder.Services.AddScoped<HealthJourneyService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<PregnancyCareService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<MentalHealthKeywordService>();
builder.Services.AddScoped<FriendsService>();

builder.Services.AddScoped<ILoginUserRepository, LoginUserRepository>();
builder.Services.AddScoped<ICommentPostRepository, CommentPostRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IHealthJourneyRepository, HealthJourneyRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
builder.Services.AddScoped<IChatRoomUserRepository, ChatRoomUserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IPostMediaRepository, PostMediaRepository>();
builder.Services.AddScoped<IPostAnalysisRepository, PostAnalysisRepository>();
builder.Services.AddScoped<IConnectionMedicalRepository, ConnectionMedicalRepository>();
builder.Services.AddScoped<IPsychologyDiaryRepository, PsychologyDiaryRepository>();
builder.Services.AddScoped<IPeriodicHealthRepository, PeriodicHealthRepository>();
builder.Services.AddScoped<IMonitoringFormRepository, MonitoringFormRepository>();
builder.Services.AddScoped<IExpertRepository, ExpertRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCaching();

// Sử dụng CORS
app.UseCors("AllowFrontend");

// Đăng ký ExceptionMiddleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Đăng ký SignalR Hub
app.MapHub<ChatHub>("/chatHub");
app.MapHub<VideoCallHub>("/videoCallHub");

app.MapControllers();

app.Run();
