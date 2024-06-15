using System.Collections.ObjectModel;
using Calculator;
using Grpc.Core;

namespace GrpcServer;

public class CalculatorServiceImpl : Calculator.Calculator.CalculatorBase
{
    public override Task<Response> Add(Request request, ServerCallContext context)
    {
        return Task.FromResult(new Response
        {
            Result = request.First + request.Second
        });
    }

    public override async Task GetPrimes(
        PrimeNumberRequest request,
        IServerStreamWriter<PrimeNumberResponse> responseStream,
        ServerCallContext context)
    {
        foreach (var prime in GetPrimes(request.Prime))
        {
            await responseStream.WriteAsync(new PrimeNumberResponse { PrimePart = prime });
        }
    }

    public override async Task<AverageResponse> ComputeAverageStream(
        IAsyncStreamReader<AverageRequest> requestStream,
        ServerCallContext context)
    {
        var numbers = new Collection<int>();
        while (await requestStream.MoveNext(context.CancellationToken))
        {
            numbers.Add(requestStream.Current.Number);
        }

        var average = numbers.Average();
        return new AverageResponse { Average = average };
    }

    public override async Task MaximumStream(IAsyncStreamReader<MaximumRequest> requestStream,
        IServerStreamWriter<MaximumResponse> responseStream, ServerCallContext context)
    {
        var max = int.MinValue;
        while (await requestStream.MoveNext(context.CancellationToken))
        {
            var number = requestStream.Current.Number;
            if (number > max)
            {
                max = number;
            }
            await responseStream.WriteAsync(new MaximumResponse { Maximum = max });
        }
    }

    private static IEnumerable<int> GetPrimes(int number)
    {
        for (var i = 2; i <= number; i++)
        {
            if (IsPrime(i))
            {
                yield return i;
            }
        }
    }

    private static bool IsPrime(int number)
    {
        if (number < 2) return false;
        for (int i = 2; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0) return false;
        }

        return true;
    }
}