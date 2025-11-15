using Locamart.Liam.Adapter.Http;
using Locamart.Liam.Adapter.Masstransit;
using Locamart.Liam.Adapter.Postgresql;
using Locamart.Liam.Adapter.Redis;
using Locamart.Nava.Adapter.Elasticsearch;
using Locamart.Nava.Adapter.Http;
using Locamart.Nava.Adapter.Http.Middlewares;
using Locamart.Nava.Adapter.Masstransit;
using Locamart.Nava.Adapter.ObjectStorage;
using Locamart.Nava.Adapter.Postgresql;
using MassTransit;
using Microsoft.OpenApi.Models;
using Serilog;


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
        Description = "Enter your JWT token below (Already prefixed with 'Bearer ')"
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
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddSingleton(Log.Logger);


builder.Services.AddMassTransit(cfg =>
{
    cfg.AddLiamMasstransit();
    cfg.AddNavaMasstransit();

    cfg.UsingInMemory((context, bus) =>
    {
        bus.ConfigureEndpoints(context);
        bus.ConcurrentMessageLimit = Environment.ProcessorCount;
    });
});

//Nava services

builder.Services.AddAdaptersHttpServices(configuration);

builder.Services.AddPostgresqleServices(configuration);

builder.Services.AddObjectStorageServices(configuration);

builder.Services.AddAdapterElasticsearchServices(configuration);

builder.Services.AddAdapterMasstransitServices(configuration);

//Liam services

builder.Services.AddLiamPostgresServices(configuration);

builder.Services.AddLiamRedisServices(configuration);

builder.Services.AddLiamAdaptersHttpServices();


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

app.UseMiddleware<SetCurrentUserMiddleware>();

app.Run();
