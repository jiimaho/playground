using Todo.Domain;
using Todo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, func) =>
{
    Console.WriteLine("yoooolo");

    await func.Invoke();
});

app.MapGet("/todos", async () =>
{
    var query = new GetAllTodosQuery();
    var handler = new GetAllTodosQueryHandler(new TodoService(new Repository()));

    return await handler.Handle(query);
});

app.MapPost("/todos", async (TodoRequest request) =>
{
    var command = new AddTodoCommand(request.Name);
    var handler = new AddTodoCommandHandler(new TodoService(new Repository()));

    await handler.Handle(command);
});

app.Run();