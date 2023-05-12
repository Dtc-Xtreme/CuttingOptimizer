using CuttingOptimizer.Infrastructure;
using CuttingOptimizer.Infrastructure.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;

Log.Logger = new LoggerConfiguration().WriteTo.File("log.txt").CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

if (builder.Environment.IsDevelopment()){
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
}
else
{
    builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
}

// Add services to the container.
builder.Services.AddDbContext<CuttingOptimizerDbContext>();
builder.Services.AddScoped<ISawRepository, SawRepository>();
builder.Services.AddScoped<IPlateRepository, PlateRepository>();
builder.Services.AddScoped<IQuotationRepository, QuotationRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.UseExceptionHandler(
//    options =>
//    {
//        options.Run(
//            async context =>
//            {
//                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                var ex = context.Features.Get<IExceptionHandlerFeature>();
//                if (ex != null)
//                {
//                    await context.Response.WriteAsync(ex.Error.Message);
//                }
//            });
//    });

//app.UseCors(x => x
//            .SetIsOriginAllowed(origin => true)
//            .AllowAnyMethod()
//            .AllowAnyHeader()
//            .AllowCredentials());

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();