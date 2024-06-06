using System.CommandLine;
using Client.CommandHandlers;

var unaryCommand = new Command("unary")
{
    Handler = new UnaryCommandHandler()
};

var serverStreamingCommand = new Command("server-streaming")
{
    Handler = new ServerStreamingCommandHandler()
};

var clientStreamingCommand = new Command("client-streaming");
var numbersOption = new Option<string>("numbers");
clientStreamingCommand.AddOption(numbersOption);
clientStreamingCommand.SetClientStreamingHandler(numbersOption);

var rootCommand = new RootCommand("This is an application to run gRPC calls");

rootCommand.AddCommand(unaryCommand);
rootCommand.AddCommand(serverStreamingCommand);
rootCommand.AddCommand(clientStreamingCommand);

await rootCommand.InvokeAsync(args);