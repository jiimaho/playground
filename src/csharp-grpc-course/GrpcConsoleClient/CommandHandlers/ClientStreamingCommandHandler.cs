using System.CommandLine;
using System.Text;
using Calculator;
using Grpc.Net.Client;

namespace Client.CommandHandlers;

public static class ClientStreamingCommandHandler
{
    public static void SetClientStreamingHandler(this Command command, Option<string> numbersOption)
    {
        command.SetHandler(async numbers =>
        {
            var sb = new StringBuilder();
            sb.AppendLine("Hello and welcome");
            sb.AppendLine($"This is the {nameof(ClientStreamingCommandHandler)} running");
            var channel = GrpcChannel.ForAddress(new Uri("http://localhost:5252"), new GrpcChannelOptions());
            var client = new Calculator.Calculator.CalculatorClient(channel);

            var numberList = numbers.Split(',').Select(int.Parse);
            var call = client.ComputeAverageStream();
            foreach (var n in numberList)
            {
                await call.RequestStream.WriteAsync(new AverageRequest { Number = n });
            }

            await call.RequestStream.CompleteAsync();

            var response = await call.ResponseAsync;

            Console.WriteLine($"The average is {response.Average}");
        }, numbersOption);
    }
}