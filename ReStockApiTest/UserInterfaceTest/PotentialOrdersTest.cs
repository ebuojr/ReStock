using System;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RestockWeb.Models;
using RestockWeb.Pages.Stores;
using RestockWeb.Services.Inventory;
using RestockWeb.Services.Reorder;
using RestockWeb.Services.Store;
using RestockWeb.Services.Threshold;
using Blazored.Toast.Services;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestockWeb.DTOs;
using AngleSharp.Dom;
using RestockWeb.Services.SalesOrder;

namespace ReStockApiTest.UserInterfaceTest;

public class PotentialOrdersTest : TestContext
{
    [Fact]
    public async Task CreatePotentialOrders_Button_CreatesPotentialOrdersAndShowsComponent()
    {
        // Arrange
        var storeNo = 123;
        var mockStoreService = new Mock<IStoreService>();
        var mockInventoryService = new Mock<IInventoryService>();
        var mockThresholdService = new Mock<IThresholdService>();
        var mockReorderService = new Mock<IReorderService>();
        var mockToastService = new Mock<IToastService>();
        var mockSalesOrderService = new Mock<ISalesOrderService>();

        mockStoreService.Setup(s => s.GetStoreAsync(storeNo)).ReturnsAsync(new Store { No = storeNo, Name = "Test Store", Country = "DK", Address = "Test Address" });
        mockInventoryService.Setup(i => i.GetStoreInventoryByStoreNoWithThresholdsAsync(storeNo)).ReturnsAsync(new List<StoresInventoryWithThresholdDTO>());
        mockReorderService.Setup(r => r.CreatePotentialOrdersByStoreNoAsync(storeNo)).ReturnsAsync(new List<RestockWeb.Models.Reorder> { new RestockWeb.Models.Reorder { StoreNo = storeNo, ItemNo = "ITEM001", Quantity = 5 } });

        Services.AddSingleton(mockStoreService.Object);
        Services.AddSingleton(mockInventoryService.Object);
        Services.AddSingleton(mockThresholdService.Object);
        Services.AddSingleton(mockReorderService.Object);
        Services.AddSingleton(mockToastService.Object);
        Services.AddSingleton(mockSalesOrderService.Object);

        // Act
        var cut = RenderComponent<StoreDetailsComponent>(parameters => parameters.Add(p => p.StoreNo, storeNo));

        // Wait for OnInitializedAsync
        await cut.InvokeAsync(() => Task.Delay(10));

        // Find and click the Create Potential Orders button
        var button = cut.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Create Potential Orders"));
        Assert.NotNull(button);
        button.Click();

        // Assert
        mockReorderService.Verify(r => r.CreatePotentialOrdersByStoreNoAsync(storeNo), Times.Once);
        Assert.Contains("Potential Orders", cut.Markup); // Updated assertion to check for heading text
        mockToastService.Verify(t => t.ShowSuccess(It.IsAny<string>(), null), Times.AtLeastOnce);
    }

    [Fact]
    public async Task CreatePotentialOrders_Button_ShowsInfoToast_WhenNoOrdersGenerated()
    {
        // Arrange
        var storeNo = 123;
        var mockStoreService = new Mock<IStoreService>();
        var mockInventoryService = new Mock<IInventoryService>();
        var mockThresholdService = new Mock<IThresholdService>();
        var mockReorderService = new Mock<IReorderService>();
        var mockToastService = new Mock<IToastService>();
        var mockSalesOrderService = new Mock<ISalesOrderService>();

        mockStoreService.Setup(s => s.GetStoreAsync(storeNo)).ReturnsAsync(new Store { No = storeNo, Name = "Test Store", Country = "DK", Address = "Test Address" });
        mockInventoryService.Setup(i => i.GetStoreInventoryByStoreNoWithThresholdsAsync(storeNo)).ReturnsAsync(new List<StoresInventoryWithThresholdDTO>());
        mockReorderService.Setup(r => r.CreatePotentialOrdersByStoreNoAsync(storeNo)).ReturnsAsync(new List<RestockWeb.Models.Reorder>()); // No orders

        Services.AddSingleton(mockStoreService.Object);
        Services.AddSingleton(mockInventoryService.Object);
        Services.AddSingleton(mockThresholdService.Object);
        Services.AddSingleton(mockReorderService.Object);
        Services.AddSingleton(mockToastService.Object);
        Services.AddSingleton(mockSalesOrderService.Object);

        // Act
        var cut = RenderComponent<StoreDetailsComponent>(parameters => parameters.Add(p => p.StoreNo, storeNo));
        await cut.InvokeAsync(() => Task.Delay(10));
        var button = cut.FindAll("button").FirstOrDefault(b => b.TextContent.Contains("Create Potential Orders"));
        Assert.NotNull(button);
        button.Click();

        // Assert
        mockReorderService.Verify(r => r.CreatePotentialOrdersByStoreNoAsync(storeNo), Times.Once);
        mockToastService.Verify(t => t.ShowInfo(It.Is<string>(msg => msg.Contains("No potential orders")), null), Times.Once);
    }

    [Fact]
    public async Task StoreDetailsComponent_LoadsStoreAndInventory_OnInit()
    {
        // Arrange
        var storeNo = 456;
        var mockStoreService = new Mock<IStoreService>();
        var mockInventoryService = new Mock<IInventoryService>();
        var mockThresholdService = new Mock<IThresholdService>();
        var mockReorderService = new Mock<IReorderService>();
        var mockToastService = new Mock<IToastService>();
        var mockSalesOrderService = new Mock<ISalesOrderService>();

        var store = new Store { No = storeNo, Name = "Store456", Country = "SE", Address = "Address456" };
        var inventory = new List<StoresInventoryWithThresholdDTO> {
            new StoresInventoryWithThresholdDTO { StoreNo = storeNo, ItemNo = "ITEMX", CurrentQuantity = 10, MinimumQuantity = 2, TargetQuantity = 15, ReorderQuantity = 5 }
        };
        mockStoreService.Setup(s => s.GetStoreAsync(storeNo)).ReturnsAsync(store);
        mockInventoryService.Setup(i => i.GetStoreInventoryByStoreNoWithThresholdsAsync(storeNo)).ReturnsAsync(inventory);

        Services.AddSingleton(mockStoreService.Object);
        Services.AddSingleton(mockInventoryService.Object);
        Services.AddSingleton(mockThresholdService.Object);
        Services.AddSingleton(mockReorderService.Object);
        Services.AddSingleton(mockToastService.Object);
        Services.AddSingleton(mockSalesOrderService.Object);

        // Act
        var cut = RenderComponent<StoreDetailsComponent>(parameters => parameters.Add(p => p.StoreNo, storeNo));
        await cut.InvokeAsync(() => Task.Delay(10));

        // Assert
        Assert.Contains("Store456", cut.Markup);
        Assert.Contains("ITEMX", cut.Markup);
    }
}
