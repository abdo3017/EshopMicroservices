var builder = WebApplication.CreateBuilder(args);
//Add DI - AddServices to the contianer

var app = builder.Build();
//Configure the HTTP request pilpeline - UseMethod

app.Run();
