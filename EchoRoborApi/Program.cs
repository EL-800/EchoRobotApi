using EchoRoborApi.Models;
using EchoRoborApi.Services;
using EchoRoborApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

const string _MyCors = "MyCors";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: _MyCors, builder =>
    {
        //permite todos los origenes
        //builder.AllowAnyOrigin();

        builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
        .AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddDbContext<EchoRobotContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IComunityService, ComunityService>();
builder.Services.AddScoped<IMultimediaService, MultimediaService>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(_MyCors);


app.UseAuthorization();

app.MapControllers();

app.Run();
