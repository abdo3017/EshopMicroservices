using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);
//Add DI - AddServices to the contianer
var assembly = typeof(Program).Assembly;
//Add DI - AddServices to the contianer
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();
builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Database")!);
    opt.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.CreateOrUpdate;
    opt.Schema.For<ShoppingCart>().Identity(x => x.UserName);
})
    .UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>(); // auto register for decorator
//builder.Services.AddScoped<IBasketRepository>(provider => // manual register for decorator
//{
//    var basketRepository = provider.GetService<BasketRepository>();
//    var cache = provider.GetService<IDistributedCache>();
//    return new CachedBasketRepository(basketRepository!, cache!);
//});

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();
//Configure the HTTP request pilpeline - UseMethod
app.UseExceptionHandler(options => { });
app.MapCarter();
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
app.Run();
