using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using Serilog;
using Serilog.Sinks.SystemConsole;
using Serilog.Sinks.MSSqlServer;
using Microsoft.Extensions.Hosting;
using Serilog.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// add     // Register IMemoryCache
builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StargateContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("StarbaseApiDatabase")));

builder.Services.AddMediatR(cfg =>
{
    cfg.AddRequestPreProcessor<CreateAstronautDutyPreProcessor>();
    cfg.AddRequestPreProcessor<CreatePersonPreProcessor>();
    cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder => builder.WithOrigins("http://localhost:4200") // Replace with the actual origin of your Angular app
                              .AllowAnyMethod()
                              .AllowAnyHeader());
    });
// Read the logging settings
var connectionString = builder.Configuration.GetConnectionString("Serilog");
var tableName = builder.Configuration["Logging:Serilog:TableName"];

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    // .WriteTo.MSSqlServer(
    //     connectionString: connectionString,
    //     sinkOptions: new MSSqlServerSinkOptions
    //     {
    //         TableName = tableName,
    //         AutoCreateSqlTable = true
    //     })
    .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information) // Log only warnings and errors to the console
    .CreateLogger();

builder.Host.UseSerilog();

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

