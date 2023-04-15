using Microsoft.EntityFrameworkCore;
using Todo.Domain;
using Todo.Infrastructure;
using Todo.Infrastructure.EntityFramework;
using Todo.Infrastructure.EventStore;
using Todo.MinimalApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoDbYolo"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, func) =>
{
    Console.WriteLine($"Request for {context.Request.Path}");

    await func.Invoke();
});

app.MapGet("/todos", async () =>
{
    var query = new GetAllTodosQuery();
    var handler = new GetAllTodosQueryHandler(new TodoService(new EventStoreRepository()));

    return await handler.Handle(query);
});

app.MapPost("/todos", async (TodoRequest request) =>
{
    var command = new AddTodoCommand(request.Name);
    var handler = new AddTodoCommandHandler(new TodoService(new EventStoreRepository()));

    await handler.Handle(command);
});

app.Run();