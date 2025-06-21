using Ganss.Xss;
using GentleBlossom_BE.Data.Models;
using GentleBlossom_BE.Data.Repositories;
using GentleBlossom_BE.Data.Repositories.Interface;
using GentleBlossom_BE.Infrastructure;
using GentleBlossom_BE.Middleware;
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
        builder.WithOrigins("https://localhost:7271")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

// đăng ký dịch vụ JWT
builder.Services.AddAuthentication(option =>
{
    // Đặt mặc định schema xác thực là JwtBearer.Điều này đảm bảo mọi yêu cầu sẽ sử dụng JWT để xác thực.
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // Đặt mặc định schema thách thức là JwtBearer. Điều này được sử dụng khi xác thực thất bại(ví dụ: token không hợp lệ hoặc không được cung cấp).Hệ thống sẽ thách thức client bằng cách trả về mã 401 Unauthorized.
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    // Đặt mặc định schema chính, áp dụng cho cả xác thực và thách thức.
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    // Cho phép API hoạt động qua HTTP(không yêu cầu HTTPS).Điều này hữu ích khi phát triển hoặc debug, nhưng bạn nên bật HTTPS trong môi trường sản xuất.
    option.RequireHttpsMetadata = builder.Configuration.GetValue<bool>("JwtConfig:RequireHttps");
    // Lưu token đã xác thực vào HttpContext. Điều này có thể hữu ích nếu bạn cần sử dụng lại token trong quá trình xử lý.
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

app.MapControllers();

app.Run();
