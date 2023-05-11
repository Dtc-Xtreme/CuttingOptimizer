using CuttingOptimizer.Infrastructure;
using CuttingOptimizer.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

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
