DotNetEnv.Env.Load();
const string appName = "cva.app";

var builder = WebApplication.CreateBuilder(args);

builder.RegisterConfig(appName);
builder.RegisterApiServices();
builder.RegisterCors();
builder.RegisterInnerServices();
builder.RegisterDatabase();
builder.RegisterValidation();
builder.RegisterAuth();

var app = builder.Build();
app.ConfigureDevEnv();
app.ConfigureApi();
app.Run();

