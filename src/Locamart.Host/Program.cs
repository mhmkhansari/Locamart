using Locamart.Adapter.Http;
using Locamart.Adapter.ObjectStorage;
using Locamart.Adapter.Postgresql;
using Locamart.Application;
using Locamart.Shared.Abstracts;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddAdaptersHttpServices();

builder.Services.AddPostgresqleServices(configuration);

builder.Services.AddObjectStorageServices(configuration);

builder.Services.Scan(scan => scan.FromAssemblies(typeof(IApplicationMarker).Assembly)
    .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);

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
