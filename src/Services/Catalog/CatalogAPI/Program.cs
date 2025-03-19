using BuildingBlocks.Bahviors;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;
//Add DI - AddServices to the contianer
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(opt =>
{
    opt.Connection(builder.Configuration.GetConnectionString("Database")!);
    opt.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.CreateOrUpdate;
})
    .UseLightweightSessions();

var app = builder.Build();

//Configure the HTTP request pilpeline - UseMethod
app.MapCarter();
app.Run();
