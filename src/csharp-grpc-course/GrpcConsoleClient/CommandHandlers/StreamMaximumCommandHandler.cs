using System.CommandLine;
using Calculator;
using Grpc.Net.Client;

namespace Client.CommandHandlers;

public static class StreamMaximumCommandHandler
{
    public static void SetMaximumStreamingHandler(this Command command)
    {
        command.SetHandler(async Task (ctx) =>
        {
            var channel = GrpcChannel.ForAddress(new Uri("http://localhost:5252"), new GrpcChannelOptions());
            var client = new Calculator.Calculator.CalculatorClient(channel);

            var call = client.MaximumStream();

            var longRunning = Task.Run(() =>
            {
                var shallExit = false;
                while (!shallExit)
                {
                    var val = Console.ReadLine();
                    if (val == "exit")
                    {
                        shallExit = true;
                        continue;
                    }

                    if (!int.TryParse(val, out var parsed))
                    {
                        continue;
                    }

                    call.RequestStream.WriteAsync(new MaximumRequest { Number = parsed });
                }

                call.RequestStream.CompleteAsync();
            }, ctx.GetCancellationToken());

            while (!longRunning.IsCompleted && await call.ResponseStream.MoveNext(CancellationToken.None))
            {
                Console.WriteLine($"Max is {call.ResponseStream.Current.Maximum}");
            }
        });
    }
}