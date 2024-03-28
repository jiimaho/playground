using System.Diagnostics;
using System.Diagnostics.Metrics;

// Microsoft now follows OpenTelemetry
// Below is .NET BCL classes and in comments the equivalent in OpenTelemetry

 // Tracing
var activitySource = new ActivitySource("Diagnostics.Program", "0.1.1"); // Root span, ONE singleton per app is recommended.
var _ = new ActivitySource("Diagnostics.Program", "0.1.1"); // Grouped by NAME, so this is essentially the same as the one above
using var activity = activitySource.CreateActivity("name", ActivityKind.Client); // Span, add as many as you like
{ 
    activity?.SetTag("key", "value"); // Tags, decorate with application specific information
    using var client = new HttpClient();
    using var operation = activitySource.StartActivity("ExternalCall", ActivityKind.Client); // Span, add as many as you like
    operation?.Start();
    operation?.SetTag("BaseAddress", client.BaseAddress);
    await client.GetAsync("");
    operation?.Stop();
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