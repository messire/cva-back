DotNetEnv.Env.Load();
const string appName = "cva.app";

var builder = WebApplication.CreateBuilder(args);

builder.RegisterConfig(appName);
builder.RegisterApiServices();
builder.RegisterInnerServices();
builder.RegisterDatabase();
builder.RegisterValidation();

var app = builder.Build();
app.ConfigureDevEnv();
app.ConfigureApi();
app.Run();

