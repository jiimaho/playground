using Grpc.Core;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BlogServer.Services;

public class BlogServiceImpl : BlogService.BlogServiceBase
{
    private static MongoClient _mongoClient = new MongoClient("mongodb://admin:password@localhost:27017");
    private static IMongoDatabase _mongoDatabase = _mongoClient.GetDatabase("mydb");
    private static IMongoCollection<BsonDocument> _mongoCollection = _mongoDatabase.GetCollection<BsonDocument>("blog");
    
    public override Task<CreateBlogResponse> CreateBlog(CreateBlogRequest request, ServerCallContext context)
    {
        var blog = request.Blog;
        BsonDocument doc = new BsonDocument("author_id", blog.Author)
            .Add("title", blog.Title)
            .Add("content", blog.Content);

        _mongoCollection.InsertOne(doc);

        var id = doc.GetValue("_id").ToString();

        blog.Id = id;

        return Task.FromResult(new CreateBlogResponse { Blog = blog });
    }

    public override async Task<ReadBlogResponse> ReadBlog(ReadBlogRequest request, ServerCallContext context)
    {
        var id = request.Id;

        var filter = new FilterDefinitionBuilder<BsonDocument>().Eq("_id", new ObjectId(id));

        var result = await _mongoCollection.FindAsync(filter);
        var single = result.SingleOrDefault();
        
        if (single is null)
            throw new RpcException(new Status(StatusCode.NotFound, "Blog not found"));

        var blog = new Blog
        {
            Author = single.GetValue("author_id").ToString(),
            Title = single.GetValue("title").ToString(),
            Content = single.GetValue("content").ToString()
        };
        
        return new ReadBlogResponse { Blog = blog };
    }

    public override async Task<UpdateBlogResponse> UpdateBlog(UpdateBlogRequest request, ServerCallContext context)
    {
        var id = request.Blog.Id;
        
        var filter = new FilterDefinitionBuilder<BsonDocument>().Eq("_id", new ObjectId(id));

        var matches = await _mongoCollection.FindAsync(filter);

        var blog = matches.SingleOrDefault();
        
        if (blog is null)
            throw new RpcException(new Status(StatusCode.NotFound, "Blog not found"));
        
        blog.Set("author_id", request.Blog.Author);
        blog.Set("title", request.Blog.Title);
        blog.Set("content", request.Blog.Content);
        
        await _mongoCollection.ReplaceOneAsync(filter, blog);
        
        return new UpdateBlogResponse
        {
            Blog = new Blog
            {
                Author = request.Blog.Author,
                Title = request.Blog.Title,
                Content = request.Blog.Content,
                Id = request.Blog.Id
            }
        };
    }

    public override async Task<DeleteBlogResponse> DeleteBlog(DeleteBlogRequest request, ServerCallContext context)
    {
        var id = request.Id;
        
        var filter = new FilterDefinitionBuilder<BsonDocument>().Eq("_id", new ObjectId(id));
        
        var result = await _mongoCollection.DeleteOneAsync(filter);

        if (result.DeletedCount == 0)
            throw new RpcException(new Status(StatusCode.NotFound, "Blog not found"));
        return new DeleteBlogResponse { Id = id };
    }

    public override async Task<ListBlogResponse> ListBlog(Empty request, ServerCallContext context)
    {
        var filter = new FilterDefinitionBuilder<BsonDocument>().Empty;
        var result = await _mongoCollection.FindAsync(filter);

        var blogs = result.ToEnumerable().Select(b => new Blog
        {
            Author = b.GetValue("author_id").ToString(),
            Title = b.GetValue("title").ToString(),
            Content = b.GetValue("content").ToString(),
            Id = b.GetValue("_id").ToString()
        });

        return new ListBlogResponse
        {
            Blog = { blogs }
        };
    }
}