using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using Serilog;
using Serilog.Sinks.SystemConsole;
using Serilog.Sinks.MSSqlServer;
using Microsoft.Extensions.Hosting;
using Serilog.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register IMemoryCache
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
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins(allowedOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();
var contentTypeProvider = new FileExtensionContentTypeProvider();

// Add or replace MIME type mappings
contentTypeProvider.Mappings[".json"] = "application/json"; // Example of replacing an existing mapping

// Configure StaticFileOptions with the custom provider
var options = new StaticFileOptions
{
    ContentTypeProvider = contentTypeProvider
};

// Configure default file mapping
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "index.html" }
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    // RequestPath = "/wwwroot"
});

Log.Information("Serving static files from: {StaticFilePath}", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));

app.MapControllers();

app.Run();

