using Blazored.LocalStorage;
using CuttingOptimizer.AppLogic.Services;
using CuttingOptimizer.Infrastructure;
using CuttingOptimizer.Infrastructure.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<ICalculatorService, CalculatorService>();
builder.Services.AddDbContext<CuttingOptimizerDbContext>();
//builder.Services.AddScoped<ISawRepository, SawRepository>();
//builder.Services.AddScoped<IPlateRepository, PlateRepository>();
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddBlazoredLocalStorage();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
