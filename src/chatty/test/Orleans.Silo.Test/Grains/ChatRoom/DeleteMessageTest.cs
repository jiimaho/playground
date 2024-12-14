using Chatty.Silo;
using FluentAssertions;
using Orleans.Silo.Test.Configuration;

namespace Orleans.Silo.Test.Grains.ChatRoom;

[Collection(nameof(OrleansCollection))]
public class DeleteMessageTest(ChattySiloFixture fixture)
{
    [Fact]
    public async Task GivenMessage_WhenDeleteMessage_ThenMessageIsDeleted()
    {
        // Arrange
        var chatRoom = fixture.Cluster.GrainFactory.GetGrain<IChatRoom>("all");
        var message = fixture.ChatMessageAutoFaker.Generate();
        await chatRoom.PostMessage(message);

        // Act
        await chatRoom.DeleteMessage(message);

        // Assert
        var messages = await chatRoom.GetHistory();
        messages.Should().BeEmpty();
    }
}