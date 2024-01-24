using MISA.AMIS.WEB08.PNNHAI.Core;
using Microsoft.Extensions.DependencyInjection;
using MISA.AMIS.WEB08.PNNHAI.Infrastructure.UnitOfWork;
using MISA.AMIS.WEB08.PNNHAI.Infrastructure;
using MISA.AMIS.WEB08.PNNHAI.Core.Managements;
using Microsoft.AspNetCore.Mvc;

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
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin();
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// Dki DI
// hàm khỏi tạo có params nên truyền kiểu factory
builder.Services.AddScoped<IUnitOfWork>(provider => new UnitOfWork(builder.Configuration["ConnectionString"]));

// Ko có params đầu vào nên tiêm bth
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IEmployeeManagement, EmployeeManagement>();


builder.Services.AddScoped<IEmployeeExcelRepository, EmployeeExcelRepository>();
builder.Services.AddScoped<IEmployeeExcelService, EmployeeExcelService>();

builder.Services.AddScoped<IExcelImportTemplateSettingRepository, ExcelImportTemplateSettingRepository>();

builder.Services.AddMemoryCache();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionMiddleware>();

app.Run();
