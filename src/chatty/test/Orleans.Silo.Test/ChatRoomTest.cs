using Chatty.Silo;
using Chatty.Silo.Primitives;
using FluentAssertions;
using NodaTime.Extensions;
using Orleans.Silo.Test.Configuration;

namespace Orleans.Silo.Test;

[Collection(nameof(OrleansCollection))]
public class ChatRoomTest
{
    private readonly ChattySiloFixture _fixture;

    public ChatRoomTest(ChattySiloFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GivenEmpty_WhenPostMessage_ThenMessageExists()
    {
        // Arrange
        var chatRoom = _fixture.Cluster.GrainFactory.GetGrain<IChatRoom>("all");
        var now = DateTimeOffset.UtcNow;
        var message = new ChatMessage
        {
            Username = new Username("user"),
            Message = "hello",
            Timestamp = now,
            ChatRoomId = "all",
            TimeStampNoda = now.ToZonedDateTime()
        };

        // Act
        await chatRoom.PostMessage(message);

        // Assert
        var messages = await chatRoom.GetHistory();
        messages.Should().NotBeEmpty();
        messages.Should().ContainEquivalentOf(message);
    }

    [Fact]
    public async Task GivenMessage_WhenJoin_ThenMessageExists()
    {
        // Arrange
        var chatRoom = _fixture.Cluster.GrainFactory.GetGrain<IChatRoom>("all");
        var now = DateTimeOffset.UtcNow;
        var message = new ChatMessage
        {
            Username = new Username("user"),
            Message = "hello",
            Timestamp = now,
            ChatRoomId = "all",
            TimeStampNoda = now.ToZonedDateTime()
        };
        await chatRoom.PostMessage(message);

        // Act
        var testObserver = new ChatRoomTestObserver();
        var observer = _fixture.Cluster.GrainFactory.CreateObjectReference<IChatRoomObserver>(testObserver);
        var messages = await chatRoom.Join(observer);

        // Assert
        messages.Should().NotBeEmpty();
        messages.Should().ContainEquivalentOf(message);
    }

    [Fact]
    public async Task GivenEmpty_WhenPostMessage_ThenObserverReceivesMessage()
    {
        // Arrange
        var testObserver = new ChatRoomTestObserver();
        var chatRoom = _fixture.Cluster.GrainFactory.GetGrain<IChatRoom>("all");
        var observer = _fixture.Cluster.GrainFactory.CreateObjectReference<IChatRoomObserver>(testObserver);
        await chatRoom.Join(observer);
        var message = new ChatMessage
        {
            Username = new Username("user"),
            Message = "hello",
            Timestamp = DateTimeOffset.UtcNow,
            ChatRoomId = "all",
            TimeStampNoda = DateTimeOffset.UtcNow.ToZonedDateTime()
        };  
        
        // Act
        await chatRoom.PostMessage(message);
        
        // Assert
        testObserver.ReceivedMessages.Should().ContainEquivalentOf(message);
    }
    
    [Fact]
    public async Task GivenMessage_WhenDeleteMessage_ThenMessageIsDeleted()
    {
        // Arrange
        var chatRoom = _fixture.Cluster.GrainFactory.GetGrain<IChatRoom>("all");
        var message = new ChatMessage
        {
            Username = new Username("user"),
            Message = "hello",
            Timestamp = DateTimeOffset.UtcNow,
            ChatRoomId = "all",
            TimeStampNoda = DateTimeOffset.UtcNow.ToZonedDateTime()
        };
        await chatRoom.PostMessage(message);
        
        // Act
        await chatRoom.DeleteMessage(message);
        
        // Assert
        var messages = await chatRoom.GetHistory();
        messages.Should().BeEmpty();
    }
}