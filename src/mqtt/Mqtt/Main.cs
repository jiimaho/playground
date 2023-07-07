using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;

var options = new ManagedMqttClientOptionsBuilder()
    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
    .WithClientOptions(new MqttClientOptionsBuilder()
        .WithClientId("Client1")
        .WithTcpServer("localhost", 1883)
        .Build())
    .Build();

IManagedMqttClient mqttClient = new MqttFactory().CreateManagedMqttClient();

await mqttClient.SubscribeAsync(new List<MqttTopicFilter>{ new MqttTopicFilterBuilder().WithTopic("test/topic").Build() });

mqttClient.ConnectedAsync += e =>
{
    Console.WriteLine("### CONNECTED WITH SERVER ###");
    return Task.CompletedTask;
};
mqttClient.ConnectingFailedAsync += e =>
{
    Console.WriteLine("### CONNECTING FAILED WITH ERROR ###");
    Console.WriteLine(e.Exception);
    return Task.CompletedTask;
};
mqttClient.DisconnectedAsync += e =>
{
    Console.WriteLine("### DISCONNECTED FROM SERVER ###");
    return Task.CompletedTask;
};
mqttClient.ApplicationMessageProcessedAsync += e =>
{
    Console.WriteLine("### APPLICATION MESSAGE PROCESSED ###");
    Console.WriteLine("+ Payload = " + Encoding.UTF8.GetString(e.ApplicationMessage.ApplicationMessage.PayloadSegment));
    return Task.CompletedTask;
};
mqttClient.ApplicationMessageReceivedAsync += e =>
{
    Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
    Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
    Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
    Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
    Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
    Console.WriteLine();
    return Task.CompletedTask;
};

await mqttClient.StartAsync(options);

var message = new MqttApplicationMessageBuilder()
    .WithTopic("test/topic")
    .WithPayload("Hello World")
    .Build();

await mqttClient.EnqueueAsync(message);

await Task.Delay(TimeSpan.FromSeconds(10));
