using Chatty.Silo.Features.Chatroom.Grains;
using Chatty.Silo.Features.Chatroom.Observers;
using FluentAssertions;
using Orleans.Silo.Test.Configuration;

namespace Orleans.Silo.Test.Grains.ChatRoom;

[Collection(nameof(OrleansCollection))]
public class JoinTest(ChattySiloFixture fixture)
{
    [Fact]
    public async Task GivenMessage_WhenJoin_ThenMessageExists()
    {
        // Arrange
        var chatRoom = fixture.Cluster.GrainFactory.GetGrain<IChatRoom>("all");
        var message = fixture.ChatMessageAutoFaker.Generate();
        await chatRoom.PostMessage(message);

        // Act
        var testObserver = new ChatRoomTestObserver();
        var observer = fixture.Cluster.GrainFactory.CreateObjectReference<IChatRoomObserver>(testObserver);
        var messages = await chatRoom.Join(observer);

        // Assert
        messages.Should().NotBeEmpty();
        messages.Should().ContainEquivalentOf(message);
    }
}