using LudoSnakeAndLadder.Client.Api;
using LudoSnakeAndLadder.Client.Api.Rest;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient(provider =>
{
    var client = new RestClient();
    return new SnakeGameClient(client, "https://localhost:7056/api/SnakeAndLadder");
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.StartSnakeGameEndpoints();

app.Run();


