using DatabaseControl;
using ExceptionHandling;
using FluentValidation;
using FluentValidation.AspNetCore;
using Marten;
using ProductApi;
using ProductApi.Core.Entities;
using ProductApi.Documentation;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;
using Weasel.Core;
using ApplicationAssembly = ProductApi.Application.AssemblyReference;

var builder = WebApplication.CreateBuilder(args);

#region Infrastructure 

builder.Services.AddDbContextFactory<ProductDbContext>(
    builder.Configuration.GetConnectionString(ConfigurationKeys.DATABASE_CONNECTION_STRING)!,
    "ProductApi"
);
builder.Services.AddRepository<ProductDbContext>();

#endregion

#region Cors

var allowedOriginsString = builder.Configuration[ConfigurationKeys.ALLOWED_CORS_ORIGINS] ?? string.Empty;
var allowedOrigins = allowedOriginsString.Split(",", StringSplitOptions.RemoveEmptyEntries);

var corsPolicy = "AllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy, policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowCredentials()
            .AllowAnyMethod();

        if (builder.Environment.IsDevelopment())
        {
            policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
        }
    });
});

#endregion

#region Marten

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString(ConfigurationKeys.DATABASE_CONNECTION_STRING)!);

    options.UseSystemTextJsonForSerialization();

    options.Schema.For<Product>().Identity(x => x.Code)/*.SoftDeleted()*/;

    if (builder.Environment.IsDevelopment())
    {
        options.AutoCreateSchemaObjects = AutoCreate.All;
    }
});

#endregion

#region Fluent Validator

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(ApplicationAssembly));

ValidatorOptions.Global.LanguageManager.Enabled = false;

#endregion

#region Project Services

builder.Services.AddSingleton<IProductRepository, ProductRepository>();

#endregion

builder.Services.AddAutoMapper(ApplicationAssembly.Assembly);

builder.Services.ConfigureCustomInvalidModelStateResponseControllers();

builder.Services.AddControllers();

builder.AddProductApiDocumentation();

var app = builder.Build();

app.UseCors(corsPolicy);

if (app.Configuration[ConfigurationKeys.EF_CREATE_DATABASE] == "true")
{
    await app.ConfigureDatabaseAsync<ProductDbContext>(CancellationToken.None);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();