using Chatty.Silo;
using FluentAssertions;
using Orleans.Silo.Test.Configuration;

namespace Orleans.Silo.Test.Grains.ChatRoom;

[Collection(nameof(OrleansCollection))]
public class PostMessageTes(ChattySiloFixture fixture)
{
    [Fact]
    public async Task GivenEmpty_WhenPostMessage_ThenMessageExists()
    {
        // Arrange
        var chatRoom = fixture.Cluster.GrainFactory.GetGrain<IChatRoom>("all");
        var message = fixture.ChatMessageAutoFaker.Generate();

        // Act
        await chatRoom.PostMessage(message);

        // Assert
        var messages = await chatRoom.GetHistory();
        messages.Should().NotBeEmpty();
        messages.Should().ContainEquivalentOf(message);
    }

    [Fact]
    public async Task GivenEmpty_WhenPostMessage_ThenObserverReceivesMessage()
    {
        // Arrange
        var testObserver = new ChatRoomTestObserver();
        var chatRoom = fixture.Cluster.GrainFactory.GetGrain<IChatRoom>("all");
        var observer = fixture.Cluster.GrainFactory.CreateObjectReference<IChatRoomObserver>(testObserver);
        await chatRoom.Join(observer);
        var message = fixture.ChatMessageAutoFaker.Generate();

        // Act
        await chatRoom.PostMessage(message);

        // Assert
        testObserver.ReceivedMessages.Should().ContainEquivalentOf(message);
    }
}