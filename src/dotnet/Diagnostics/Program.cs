using System.Diagnostics;
using System.Diagnostics.Metrics;

// Microsoft now follows OpenTelemetry
// Below is .NET BCL classes and in comments the equivalent in OpenTelemetry

 // Tracing
var activitySource = new ActivitySource("Diagnostics.Program", "0.1.1"); // Root span
using var span = activitySource.CreateActivity("name", ActivityKind.Client); // Span
{ 
    span.SetTag("key", "value"); // Tags
    using var client = new HttpClient();
    using (var nested = activitySource.StartActivity("operation")) // Span
    {
        nested.Start();
        nested.SetTag("BaseAddress", client.BaseAddress);
        await client.GetAsync("");
        nested.Stop();
    }
}


// Metrics
var meter = new Meter("DinApplikation", "1.1.1"); // Meter?
var orderCounter = meter.CreateCounter<int>("Orders");
var checkoutTime = meter.CreateHistogram<int>("CheckoutTime", "seconds", "Time it took to checkout");
var customersLive = meter.CreateUpDownCounter<int>("CurrentCustomers", "Customers", "Current number of customers in webshop");

orderCounter.Add(1);
checkoutTime.Record(30);
checkoutTime.Record(200);
checkoutTime.Record(10);