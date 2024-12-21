using LudoSnakeAndLadder.Databases.Models;
using LudoSnakeAndLadder.Domain.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connection = builder.Configuration.GetConnectionString("DbConnection");
//builder.Services.AddDbContext<SnakeDbContext>(option=>{ option.UseSqlServer(connection); },ServiceLifetime.Transient,ServiceLifetime.Transient);
builder.Services.AddDbContext<SnakeDbContext>(option => { option.UseSqlServer(connection); },ServiceLifetime.Scoped,ServiceLifetime.Singleton);
builder.Services.AddScoped<PlayGameServices>();
builder.Services.AddScoped<StartGameServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
