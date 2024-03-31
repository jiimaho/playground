using Disasters.Api.Services;
using Moq;
using Xunit.Abstractions;

namespace Disasters.IntegrationTests.Endpoints;

public class MarkSafeTest : IClassFixture<MyWebApplicationFactory>
{
    private readonly MyWebApplicationFactory _factory;

    public MarkSafeTest(
        MyWebApplicationFactory factory,
        ITestOutputHelper outputHelper)
    {
        _factory = factory;
        factory.OutputHelper = outputHelper;
    }

   [Fact] 
   public async Task MarkSafe_ReturnsSuccess()
   {
       // Arrange
       var markSafeVm = new MarkSafeVm("Heavy rain");
       
       _factory.MockedDisastersService
           .Setup(m => m.MarkSafe(markSafeVm))
           .Returns(Task.CompletedTask);
       
       using var client = _factory.CreateClient();
       
       // Act
       var response = await client.PostAsJsonAsync("/disasters/mark-safe", markSafeVm);
       
       // Assert
       response.EnsureSuccessStatusCode();
       _factory.MockedDisastersService.Verify(p => p.MarkSafe(It.IsAny<MarkSafeVm>()), Times.Once());
   }
}