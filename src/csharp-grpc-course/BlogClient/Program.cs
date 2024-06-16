// See https://aka.ms/new-console-template for more information


using Grpc.Core;
using Grpc.Net.Client;

var channel = GrpcChannel.ForAddress(new Uri("http://localhost:5285"), new GrpcChannelOptions
{
    Credentials = ChannelCredentials.Insecure
});

var client = new BlogService.BlogServiceClient(channel);

// await client.CreateBlogAsync(new CreateBlogRequest
//     { Blog = new Blog { Author = "1", Title = "My First Blog", Content = "Content of the blog" } });

// var result = await client.ReadBlogAsync(new ReadBlogRequest { Id = "666f23984172094b7d551103" });
//
// Console.WriteLine(result.Blog.Title);
// Console.WriteLine(result.Blog.Content);
// Console.WriteLine(result.Blog.Author);
// Console.WriteLine(result.Blog.Id);
// Console.WriteLine("Hello, World!");

// await client.UpdateBlogAsync(new UpdateBlogRequest
// {
//     Blog = new Blog
//     {
//         Author = "jim",
//         Content = "new content",
//         Title = "new title",
//         Id = "666f23984172094b7d551103"
//     }
// });

await client.DeleteBlogAsync(new DeleteBlogRequest { Id = "666f23984172094b7d551103" });
