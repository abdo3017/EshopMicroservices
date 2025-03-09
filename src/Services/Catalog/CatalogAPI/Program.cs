var builder = WebApplication.CreateBuilder(args);

//Add DI - AddServices to the contianer
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

var app = builder.Build();

//Configure the HTTP request pilpeline - UseMethod
app.MapCarter();
app.Run();
