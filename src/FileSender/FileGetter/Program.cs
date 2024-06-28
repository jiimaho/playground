// See https://aka.ms/new-console-template for more information

using System.CommandLine;
using FileGetter;

var rootCommand = new RootCommand("default");

var fileGetter = new Command("file-getter", "Get file from server");
fileGetter.MapFileGetterHandler();

var largeFileGetter = new Command("large-file-getter", "Get large file from server");
largeFileGetter.MapLargeFileGetterHandler();

rootCommand.AddCommand(fileGetter);
rootCommand.AddCommand(largeFileGetter);

await rootCommand.InvokeAsync(args);