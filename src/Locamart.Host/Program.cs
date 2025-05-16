using Locamart.Adapter.Http;
using Locamart.Adapter.Postgresql;
using Locamart.Adapter.ObjectStorage;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddAdaptersHttpServices();

builder.Services.AddPostgresqleServices(configuration);

builder.Services.AddObjectStorageServices(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
