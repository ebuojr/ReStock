using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ReStockApi.BackroundService;
using ReStockApi.Models;
using ReStockApi.Services.DataGeneration;
using ReStockApi.Services.Inventory;
using ReStockApi.Services.JobLastRunService;
using ReStockApi.Services.Product;
using ReStockApi.Services.Reorder;
using ReStockApi.Services.ReorderLog;
using ReStockApi.Services.SalesOrder;
using ReStockApi.Services.Store;
using ReStockApi.Services.Threshold;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// allow all CORS requests
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// fluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();
builder.Services.AddScoped<IThresholdService, ThresholdService>();
builder.Services.AddScoped<IReorderService, ReorderService>();
builder.Services.AddScoped<IReorderLogService, ReorderLogService>();
builder.Services.AddScoped<IJobLastRunService, JobLastRunService>();

// register the data generation service
builder.Services.AddScoped<IDataGenerationService, DataGenerationService>();

// register the background service
builder.Services.AddHostedService<ReorderingService>();

// database
builder.Services.AddDbContext<ReStockDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
}
app.UseCors("AllowAllOrigins");
app.MapOpenApi();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
