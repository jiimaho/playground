using System.CommandLine.Invocation;
using Calculator;
using Grpc.Core;
using Grpc.Net.Client;

namespace Client.CommandHandlers;

internal sealed class UnaryCommandHandler : ICommandHandler
{
    public int Invoke(InvocationContext context)
    {
        throw new NotImplementedException();
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        var channel =
            GrpcChannel.ForAddress(new Uri("http://localhost:5252", UriKind.Absolute), new GrpcChannelOptions());
        var calculatorClient = new Calculator.Calculator.CalculatorClient(channel);
        await RunUnary(calculatorClient);
        return 0;
    }

    async Task RunUnary(Calculator.Calculator.CalculatorClient client)
    {
        try
        {
            var response = await client.AddAsync(new Request { First = 20, Second = 287 });
            Console.WriteLine($"Response received from server: {response.Result} at time {DateTimeOffset.Now}");
        }
        catch (RpcException e) when (e.StatusCode == StatusCode.InvalidArgument)
        {
            Console.WriteLine($"Server replied with : {e.Status.Detail}");
        }
    }
}