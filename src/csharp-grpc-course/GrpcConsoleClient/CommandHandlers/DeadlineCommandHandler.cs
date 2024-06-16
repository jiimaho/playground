using System.CommandLine;
using System.CommandLine.Invocation;
using Calculator;
using Grpc.Net.Client;

namespace Client.CommandHandlers;

public static class DeadlineCommandHandler
{
    public static void SetDeadlineHandler(this Command command)
    {
        command.SetHandler(async () =>
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5252",
                new GrpcChannelOptions { UnsafeUseInsecureChannelCallCredentials = true });

            var client = new Calculator.Calculator.CalculatorClient(channel);
            await client.RunDeadlineAsync(new RunDeadlineRequest(), deadline: DateTime.UtcNow.AddSeconds(4));
        });
    }
}