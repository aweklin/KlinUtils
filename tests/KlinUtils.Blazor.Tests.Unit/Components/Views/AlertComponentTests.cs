using System.Reflection;

using Bunit;

using KlinUtils.Blazor.Components.Models;
using KlinUtils.Blazor.Components.Views;

namespace KlinUtils.Blazor.Tests.Unit.Components.Views;

public class AlertComponentTests : TestContext
{
    [Fact]
    public void SetType_ShouldUpdateTypeProperty()
    {
        AlertComponent component = new();
        AlertComponent result = component.SetType(AlertType.Warning);

        Assert.Equal(AlertType.Warning, component.Type);
        Assert.Same(component, result);
    }

    [Fact]
    public void SetMessage_ShouldUpdateMessageProperty()
    {
        AlertComponent component = new();
        AlertComponent result = component.SetMessage("This is a test message.");

        Assert.Equal("This is a test message.", component.Message);
        Assert.Same(component, result);
    }

    [Fact]
    public void SetData_ShouldUpdateDataProperty()
    {
        List<string> data = ["Item 1", "Item 2"];
        AlertComponent component = new();
        AlertComponent result = component.SetData(data);

        Assert.Equal(data, component.Data);
        Assert.Same(component, result);
    }

    [Theory]
    [InlineData(AlertType.Info, null, "text-blue-700 bg-blue-200")]
    [InlineData(AlertType.Warning, null, "text-orange-700 bg-orange-200")]
    [InlineData(AlertType.Error, null, "text-red-700 bg-red-200")]
    [InlineData(AlertType.Success, null, "text-green-700 bg-green-300")]
    [InlineData(AlertType.None, null, "")]
    [InlineData(AlertType.Info, "custom-class", "custom-class")]
    public void GetStyles_ShouldReturnCorrectClass(AlertType type, string? overrideClass, string expected)
    {
        AlertComponent component = new AlertComponent().SetType(type);

        // Use reflection to call private method
        MethodInfo? method = typeof(AlertComponent).GetMethod("GetStyles", BindingFlags.NonPublic | BindingFlags.Instance);
        object? result = method?.Invoke(component, [overrideClass]);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetStyles_InvalidAlertType_ShouldThrow()
    {
        AlertComponent component = new();

        // Set an invalid enum value using casting
        typeof(AlertComponent).GetProperty("Type")?.SetValue(component, (AlertType)999);

        MethodInfo? method = typeof(AlertComponent).GetMethod("GetStyles", BindingFlags.NonPublic | BindingFlags.Instance);

        Assert.Throws<TargetInvocationException>(() => method?.Invoke(component, [null]));
    }

    [Fact]
    public void ShouldRenderAlertWithMessageAndNoData()
    {
        // Arrange
        string message = "This is an info alert";

        // Act
        IRenderedComponent<AlertComponent> cut = RenderComponent<AlertComponent>(parameters => parameters
            .Add(p => p.Type, AlertType.Info)
            .Add(p => p.Message, message)
            .Add(p => p.Data, []));

        // Assert
        cut.MarkupMatches($"""
            <div class="p-4 flex flex-col rounded-md my-4 text-blue-700 bg-blue-200">
                <h4 class="font-medium">{message}</h4>
            </div>
        """);
    }

    [Fact]
    public void ShouldRenderAlertWithDataList()
    {
        // Arrange
        IEnumerable<string> data = ["Point A", "Point B"];

        // Act
        IRenderedComponent<AlertComponent> cut = RenderComponent<AlertComponent>(parameters => parameters
            .Add(p => p.Type, AlertType.Warning)
            .Add(p => p.Message, "Warning with bullets")
            .Add(p => p.Data, data));

        // Assert
        cut.MarkupMatches($"""
            <div class="p-4 flex flex-col rounded-md my-4 text-orange-700 bg-orange-200">
                <h4 class="font-medium">Warning with bullets</h4>
                <ul>
                    <li>Point A</li>
                    <li>Point B</li>
                </ul>
            </div>
        """);
    }

    [Fact]
    public void ShouldRenderEmptyClassWhenTypeIsNone()
    {
        // Act
        IRenderedComponent<AlertComponent> cut = RenderComponent<AlertComponent>(parameters => parameters
            .Add(p => p.Type, AlertType.None)
            .Add(p => p.Message, "Neutral alert"));

        // Assert
        cut.MarkupMatches($"""
            <div class="p-4 flex flex-col rounded-md my-4 ">
                <h4 class="font-medium">Neutral alert</h4>
            </div>
        """);
    }
}