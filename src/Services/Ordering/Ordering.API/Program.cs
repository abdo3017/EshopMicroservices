using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
// Configure the HTTP request pipeline.

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();
var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseApiServices();

app.Run();
