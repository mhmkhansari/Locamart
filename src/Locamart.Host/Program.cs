using Locamart.Dina.Abstracts;
using Locamart.Liam.Adapter.Http;
using Locamart.Liam.Adapter.Postgresql;
using Locamart.Liam.Adapter.Redis;
using Locamart.Nava.Adapter.Elasticsearch;
using Locamart.Nava.Adapter.Http;
using Locamart.Nava.Adapter.ObjectStorage;
using Locamart.Nava.Adapter.Postgresql;
using Locamart.Nava.Adapter.Rabbitmq;
using Locamart.Nava.Application;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Locamart API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token below (prefix with 'Bearer ')"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAdaptersHttpServices();

builder.Services.AddPostgresqleServices(configuration);

builder.Services.AddObjectStorageServices(configuration);

builder.Services.AddAdapterElasticsearchServices(configuration);

builder.Services.AddRabbitmqServices(configuration);

builder.Services.AddLiamPostgresServices(configuration);



builder.Services.AddLiamRedisServices(configuration);

builder.Services.AddLiamAdaptersHttpServices();

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
