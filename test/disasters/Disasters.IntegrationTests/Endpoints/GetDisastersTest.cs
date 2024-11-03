using Disasters.Api.Services;
using Moq;
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
            new("Earthquake", "California"),
            new("Tornado", "Kansas")
        };

        _factory.MockedDisastersService
            .Setup(m => m.GetDisasters(null, null))
            .Returns(Result(disasterVms));

        using var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/disasters");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        await VerifyJson(content);
    }

    [Fact]
    public async Task GetDisasters_Paging_ReturnsDisasters()
    {
        // Arrange
        var disasterVms = new DisasterVm[]
        {
            new("Heavy rain", "Lousiana"),
            new("Earthquake", "California"),
            new("Tornado", "Kansas")
        };

        _factory.MockedDisastersService
            .Setup(m => m.GetDisasters(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Result(disasterVms));

        using var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/disasters?page=1&pageSize=5");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        await VerifyJson(content);
    }

    private Task<DisasterResult> Result(IEnumerable<DisasterVm> disasterVms) =>
        Task.FromResult<DisasterResult>(new DisasterResult.Success(disasterVms));
}