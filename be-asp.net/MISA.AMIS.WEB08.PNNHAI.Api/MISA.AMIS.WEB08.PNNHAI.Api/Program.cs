using MISA.AMIS.WEB08.PNNHAI.Core;
using Microsoft.Extensions.DependencyInjection;
using MISA.AMIS.WEB08.PNNHAI.Infrastructure.UnitOfWork;
using MISA.AMIS.WEB08.PNNHAI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(option =>
    {
        option.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState.Values.Reverse().SelectMany(x => x.Errors);
            string errorMessage = string.Join(", ", errors.Select(x => x.ErrorMessage));
            return new BadRequestObjectResult(new BaseNotifyException()
            {
                ErrorCode = 400,
                UserMessage = errorMessage,
                DevMessage = errorMessage,
                TraceId = context.HttpContext.TraceIdentifier,
                MoreInfo = "",
                Error = errors
            });

        };
    })
    .AddJsonOptions(options =>
    {
        // Giữ tên được viết khi khai báo để hiển thị lên api
        //options.JsonSerializerOptions.PropertyNamingPolicy = null;

        // Sẽ bỏ qua với dữ liệu bị null
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Add Cors
builder.Services.AddCors();

builder.Services.AddHttpContextAccessor();


// Cache
builder.Services.AddMemoryCache();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Xác thực jwt
var secretKey = builder.Configuration["AppSettings:SecretKey"];
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Tự cấp token ko sử dụng của các bên cấp token
            ValidateIssuer = false,
            ValidateAudience = false,

            // Kiểm tra nếu expired thì thông báo
            ValidateLifetime = true,
            // Kí vào token
            ValidateIssuerSigningKey = true,
            //ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            //ValidAudience = builder.Configuration["AppSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

            ClockSkew = TimeSpan.Zero   // Kiểm tra thời gian của token có quá hạn không
        };
    });

// Dki DI
// hàm khỏi tạo có params nên truyền kiểu factory
builder.Services.AddScoped<IUnitOfWork>(provider => new UnitOfWork(builder.Configuration["ConnectionString"]));

// Ko có params đầu vào nên tiêm bth
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();


builder.Services.AddScoped<IEmployeeExcelRepository, EmployeeExcelRepository>();
builder.Services.AddScoped<IEmployeeExcelService, EmployeeExcelService>();

builder.Services.AddScoped<IEmployeeStatisticalRepository, EmployeeStatisticalRepository>();
builder.Services.AddScoped<IEmployeeStatisticalService, EmployeeStatisticalService>();

builder.Services.AddScoped<IExcelImportTemplateSettingRepository, ExcelImportTemplateSettingRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILoginLogRepository, LoginLogRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder
                .WithOrigins("http://localhost:8080")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()); // allow credentials

app.UseHttpsRedirection();

// Authen trc author
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionMiddleware>();

//app.UseCookiePolicy();

app.Run();
