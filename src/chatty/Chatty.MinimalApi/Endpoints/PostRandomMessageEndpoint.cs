using Chatty.Silo.Features.Chatroom.Grains;
using Chatty.Silo.Primitives;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Chatty.MinimalApi.Endpoints;

public static partial class EndpointExtensions
{
    public static WebApplication MapPostRandomMessageEndpoint(this WebApplication app)
    {
        app.MapPost(
                "rooms/{id}/randommessage",
                async (IClusterClient clusterClient, [FromRoute] string id) =>
                {
                    var chatRoom = clusterClient.GetGrain<IChatRoom>(id);
                    var chatMessage = GenerateRandomMessage(id);
                    await chatRoom.PostMessage(chatMessage);
                    return Results.Ok();
                })
            .WithName("RandomMessage")
            .WithOpenApi();
        return app;
    }

    private static ChatMessage GenerateRandomMessage(string roomName)
    {
        var random = Random.Shared.Next(Messages.Length);
        return ChatMessage.Create(Username.Create(GenerateRandomUsername()), Messages[random], roomName);
    }

    private static readonly string[] Messages =
    {
        "Life’s too short for bad coffee. But here you are, drinking it anyway. ☕",
        "Error 404: Motivation not found. Try again after coffee. 🔄",
        "You're one step away from greatness… but you tripped. 😆",
        "Your internet connection is stable. Your life? Not so much. 🌍💀",
        "Remember: If at first you don’t succeed, skydiving is not for you. 🪂",
        "Be the reason someone smiles today. Or the reason they check their firewall. 🔥",
        "Your code is compiling... which is a miracle. 🧙‍♂️",
        "Behind every great developer is a keyboard full of crumbs. 🍕⌨️",
        "Congratulations! You’ve scrolled enough today. Your thumb is now an athlete. 🏆",
        "If laughter is the best medicine, your sense of humor needs a prescription. 😂"
    };

    private static readonly string[] Adjectives =
    {
        "Witty", "Sleepy", "Sassy", "Goofy", "Spooky", "Salty", "Cheesy", "Bouncy", "Quirky", "Funky"
    };

    private static readonly string[] Nouns =
    {
        "Panda", "Taco", "Gnome", "Banana", "Unicorn", "Noodle", "Robot", "Sloth", "Chicken", "Potato"
    };

    public static string GenerateRandomUsername()
    {
        var random = Random.Shared;
        var adjective = Adjectives[random.Next(Adjectives.Length)];
        var noun = Nouns[random.Next(Nouns.Length)];
        var number = random.Next(10, 999); // Random number to make it unique

        return $"{adjective}{noun}{number}";
    }
}