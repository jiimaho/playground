using System.Diagnostics;
using System.Diagnostics.Metrics;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace Disasters.Lambda.Functions;

public class DisasterFunction
{
    private readonly ActivitySource _activity; // Equivalent to RootSpan in OpenTelemetry
    private readonly Meter _meter; // Equivalent to Meter in OpenTelemetry
    private readonly Counter<int> _getCounter;
    private readonly Histogram<int> _responseCounter;

    public DisasterFunction()
    {
        _activity = new ActivitySource("Disasters.Api", "1.0.0");
        _meter = new Meter("Disasters.Api", "1.0.0");
        _getCounter = _meter.CreateCounter<int>("number_of_get_requests", "requests", "Number of GET requests made to the Get Lambda function");
        _responseCounter = _meter.CreateHistogram<int>("response_time", "milliseconds", "Response time in milliseconds for the Get Lambda function");
    }

    public async Task<APIGatewayProxyResponse> Delete(APIGatewayProxyRequest apiRequest, ILambdaContext context)
    {
        return new APIGatewayProxyResponse{ StatusCode = 200 };
    }
    
    public async Task<APIGatewayProxyResponse> Post(APIGatewayProxyRequest apiRequest, ILambdaContext context)
    {
        return new APIGatewayProxyResponse{ StatusCode = 200 };
    }

    public async Task<APIGatewayProxyResponse> Get(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return new APIGatewayProxyResponse{ StatusCode = 200 };
    }
}