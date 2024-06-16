using System.CommandLine;
using Client.CommandHandlers;

// unary
var unaryCommand = new Command("unary")
{
    Handler = new UnaryCommandHandler()
};

// server streaming
var serverStreamingCommand = new Command("server-streaming")
{
    Handler = new ServerStreamingCommandHandler()
};

// client streaming
var clientStreamingCommand = new Command("client-streaming");
var numbersOption = new Option<string>("numbers");
clientStreamingCommand.AddOption(numbersOption);
clientStreamingCommand.SetClientStreamingHandler(numbersOption);

// maximum streaming
var maximumStreamingCommand = new Command("maximum-streaming");
maximumStreamingCommand.SetMaximumStreamingHandler();

// deadline
var deadlineCommand = new Command("deadline");
deadlineCommand.SetDeadlineHandler();

var rootCommand = new RootCommand("This is an application to run gRPC calls");

rootCommand.AddCommand(unaryCommand);
rootCommand.AddCommand(serverStreamingCommand);
rootCommand.AddCommand(clientStreamingCommand);
rootCommand.AddCommand(maximumStreamingCommand);
rootCommand.AddCommand(deadlineCommand);

await rootCommand.InvokeAsync(args);