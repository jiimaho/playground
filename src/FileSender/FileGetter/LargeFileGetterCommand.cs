using System.CommandLine;

namespace FileGetter;

public static class LargeFileGetterCommand
{
    public static void MapLargeFileGetterHandler(this Command command)
    {
        command.SetHandler(async ctx =>
        {
            try
            {
                using var client = new HttpClient();

                var response = await client.GetAsync("http://localhost:5234/largefile");

                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync(ctx.GetCancellationToken());

                await using var fileStream =
                    new FileStream($"{Random.Shared.Next(0, 1000)}largefile", FileMode.Create, FileAccess.Write);
                await stream.CopyToAsync(fileStream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }
}