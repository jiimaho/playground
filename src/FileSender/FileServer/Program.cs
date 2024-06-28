var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/file.png", async ctx =>
{
    await using var file = File.OpenRead("Images/file.png");
    var result = Results.File(file, "image/png");
    await result.ExecuteAsync(ctx);
});
app.MapGet("/largefile", async ctx =>
{
    if (File.Exists("Files/1GB_file"))
    {
        var notFound =
            Results.NotFound(
                "In order to run this endpoint you must create a large file. This is not done because I did " +
                "not want to commit a very large file into git. I could of course generate it on the fly but " +
                "that's for the future");
        await notFound.ExecuteAsync(ctx);
    }

    await using var file = File.OpenRead("Files/1GB_file");
    var result = Results.File(file, "application/octet-stream");
    await result.ExecuteAsync(ctx);
});

app.Run();