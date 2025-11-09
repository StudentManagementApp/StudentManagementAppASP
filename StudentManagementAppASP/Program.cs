using StudentManagementApp.Infrastructure;
using StudentManagementApp.Application.Mapping;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// Serilog Configuration
// -------------------------
builder.Host.UseSerilog((ctx, services, cfg) =>
{
    cfg.ReadFrom.Configuration(ctx.Configuration)
       .ReadFrom.Services(services)
       .Enrich.FromLogContext()
       .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
       .WriteTo.Console() // خروجی به کنسول
       .WriteTo.File(
            path: "logs/app-.log",          // مسیر فایل لاگ
            rollingInterval: RollingInterval.Day, // هر روز فایل جدید
            retainedFileCountLimit: 14,     // نگهداری ۱۴ فایل اخیر
            shared: true
        );
});

// -------------------------
// Services Registration
// -------------------------
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(DomainToDtoProfile).Assembly);

var app = builder.Build();

// -------------------------
// Middleware Configuration
// -------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// -------------------------
// Serilog Clean Shutdown
// -------------------------
app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

app.Run();
