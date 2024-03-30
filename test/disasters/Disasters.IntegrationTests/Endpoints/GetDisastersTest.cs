using Disasters.Api.Services;
using Xunit.Abstractions;

namespace Disasters.IntegrationTests.Endpoints;

public class GetDisastersTest : IClassFixture<MyWebApplicationFactory>
{
    private readonly MyWebApplicationFactory _factory;

    public GetDisastersTest( 
        MyWebApplicationFactory factory,
        ITestOutputHelper outputHelper)
    {
        _factory = factory;
        factory.OutputHelper = outputHelper;
    }

    [Fact]
    public async Task GetDisasters_ReturnsDisasters()
    {
        // Arrange
        var disasterVms = new DisasterVm[]
        {
            new("Heavy rain", "Lousiana"),
            new("Earthquake", "California")
        };

        _factory.MockedDisastersService
            .Setup(m => m.GetDisasters())
            .Returns(Task.FromResult(disasterVms.AsEnumerable()));
        
        using var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/disasters");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        await VerifyJson(content);
    }
}