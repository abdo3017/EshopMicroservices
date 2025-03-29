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
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();
//Configure the HTTP request pilpeline - UseMethod
app.UseExceptionHandler(options => { });
app.MapCarter();
app.Run();
