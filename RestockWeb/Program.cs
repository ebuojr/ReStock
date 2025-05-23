using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RestockWeb;
using RestockWeb.Services.Product;
using RestockWeb.Services.Store;
using RestockWeb.Services.Inventory;
using RestockWeb.Services.Reorder;
using RestockWeb.Services.SalesOrder;
using RestockWeb.Services.Threshold;
using Blazored.Toast;
using RestockWeb.Services.Reorderlogs;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Set the base address for the API
builder.Services.AddScoped(sp => new HttpClient 
                { 
                    BaseAddress = new Uri("http://localhost:5126") 
                });

builder.Services.AddBlazoredToast();

// Register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IReorderService, ReorderService>();
builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();
builder.Services.AddScoped<IThresholdService, ThresholdService>();
builder.Services.AddScoped<IReorderLogService, ReorderLogService>();

await builder.Build().RunAsync();
