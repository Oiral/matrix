using Matrix.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddStandardApiFeatures();

var app = builder.Build();

app.AddStandardAppFeatures();

app.UseHttpsRedirection();

app.Run();