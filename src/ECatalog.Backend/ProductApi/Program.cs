using DatabaseControl;
using ExceptionHandling;
using FluentValidation;
using FluentValidation.AspNetCore;
using ProductApi;
using ProductApi.Application;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;

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
        policy.WithOrigins(allowedOrigins)
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

#region Fluent Validator

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(AssemblyReference));

ValidatorOptions.Global.LanguageManager.Enabled = false;

#endregion

#region Project Services

builder.Services.AddSingleton<IProductRepository, ProductRepository>();

#endregion

builder.Services.AddAutoMapper(AssemblyReference.Assembly);

builder.Services.ConfigureCustomInvalidModelStateResponseControllers();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.DescribeAllParametersInCamelCase();
    });
}

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
