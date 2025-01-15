using DatabaseControl.Repositories;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace DatabaseControl
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository<Context>(this IServiceCollection services) where Context : DbContext
        {
            services.AddSingleton<IDatabaseRepository<Context>, DatabaseRepository<Context>>();
            return services;
        }

        public static IServiceCollection AddDbContextFactory<Context>(
            this IServiceCollection services,
            string connectionString,
            string? migrationAssembly = null,
            Action<NpgsqlDbContextOptionsBuilder>? dbAdditionalConfig = null,
            Action<DbContextOptionsBuilder>? additionalConfig = null
        ) where Context : DbContext
        {
            services.AddDbContextFactory<Context>(options =>
            {
                var npgsqlOptions = options.UseNpgsql(connectionString, b =>
                {
                    if (!string.IsNullOrEmpty(migrationAssembly))
                    {
                        b.MigrationsAssembly(migrationAssembly);
                    }
                    dbAdditionalConfig?.Invoke(b);
                });

                options.UseSnakeCaseNamingConvention();
                npgsqlOptions.UseExceptionProcessor();

                additionalConfig?.Invoke(options);
            });

            return services;
        }
    }
}
