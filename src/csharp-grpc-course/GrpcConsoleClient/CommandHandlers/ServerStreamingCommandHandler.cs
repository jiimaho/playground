using System.CommandLine.Invocation;
using Calculator;
using Grpc.Net.Client;

namespace Client.CommandHandlers;

public class ServerStreamingCommandHandler : ICommandHandler
{
    public int Invoke(InvocationContext context)
    {
        throw new NotImplementedException();
    }

    public async Task<int> InvokeAsync(InvocationContext context)
    {
        Console.WriteLine($"Running {nameof(ServerStreamingCommandHandler)}");
        
        var channel =
            GrpcChannel.ForAddress(new Uri("http://localhost:5252", UriKind.Absolute), new GrpcChannelOptions());
        var client = new Calculator.Calculator.CalculatorClient(channel);
        using var call = client.GetPrimes(new PrimeNumberRequest { Prime = 100 });

        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            Console.WriteLine($"Prime number received: {call.ResponseStream.Current.PrimePart}");
        }

        Console.WriteLine($"Finished {nameof(ServerStreamingCommandHandler)}");
        return 0;
    }
}