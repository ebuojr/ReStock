using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ReStockApi.Models;
using ReStockApi.Services.Inventory;
using ReStockApi.Services.Product;
using ReStockApi.Services.ReorderLog;
using ReStockApi.Services.SalesOrder;
using ReStockApi.Services.Store;
using ReStockApi.Services.Threshold;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// fluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();
builder.Services.AddScoped<IThresholdService, ThresholdService>();
builder.Services.AddScoped<IReorderLogService, ReorderLogService>();

// database
builder.Services.AddDbContext<ReStockDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
