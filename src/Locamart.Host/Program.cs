using Locamart.Dina.Abstracts;
using Locamart.Liam.Adapter.Http;
using Locamart.Liam.Adapter.Postgresql;
using Locamart.Liam.Adapter.Redis;
using Locamart.Liam.Application;
using Locamart.Nava.Adapter.Elasticsearch;
using Locamart.Nava.Adapter.Http;
using Locamart.Nava.Adapter.ObjectStorage;
using Locamart.Nava.Adapter.Postgresql;
using Locamart.Nava.Application;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

builder.Services.AddAdaptersHttpServices();

builder.Services.AddPostgresqleServices(configuration);

builder.Services.AddObjectStorageServices(configuration);

builder.Services.AddAdapterElasticsearchServices(configuration);


builder.Services.AddLiamPostgresServices(configuration);



builder.Services.AddLiamRedisServices(configuration);

builder.Services.AddLiamAdaptersHttpServices();

builder.Services.AddLiamApplicationServices(configuration);

builder.Services.Scan(scan => scan.FromAssemblies(typeof(IApplicationMarker).Assembly)
    .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await AdapterElasticServiceCollectionExtensions.InitializeAdapterElasticsearchAsync(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
