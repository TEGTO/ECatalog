using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace ProductApi.Documentation
{
    public static class Extensions
    {
        public static IHostApplicationBuilder AddProductApiDocumentation(this IHostApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.DescribeAllParametersInCamelCase();

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Product API",
                    Description = "An ASP.NET Core Web API for managing products",
                });

                options.EnableAnnotations();
                options.ExampleFilters();
            });

            builder.Services.AddFluentValidationRulesToSwagger();
            builder.Services.AddSwaggerExamplesFromAssemblyOf(typeof(AssemblyReference));

            return builder;
        }
    }
}
