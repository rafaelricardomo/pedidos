using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Scrutor;

namespace Pedidos.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSetup(this IServiceCollection services,IConfiguration configuration)
    {
        AddConfigurations(services, configuration);
        AddRepositories(services, configuration);
        AddPublishers(services, configuration);
        AddUseCases(services, configuration);

        return services;
    }
     private static IServiceCollection AddUseCases(IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<ICriarPedidoUseCase, CriarPedidoUseCase>();
        services.AddScoped<IObterPedidoUseCase, ObterPedidoUseCase>();
        return services;
    }
    
    private static IServiceCollection AddRepositories(IServiceCollection services,IConfiguration configuration)
    {

        services.AddSingleton<IPedidoSqlRepository, PedidoSqlRepository>();
        services.AddSingleton<IItemPedidoMongoRepository, ItemPedidoMongoRepository>();
        services.AddSingleton<IPedidoRepository, PedidoHibridoRepository>();
        services.Decorate<IPedidoRepository, PedidoRepositoryCache>();

        return services;
    }
    
    private static IServiceCollection AddPublishers(IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<IPedidosPublicador, PedidosPublicador>();

        return services;
    }
     private static IServiceCollection AddConfigurations(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqConfiguration>(
            configuration.GetSection("RabbitMq"));

        services.AddSingleton(sp =>
            new SqlConfiguration(configuration.GetValue<string>("ConnectionStrings:SqlServer")));

        services.AddSingleton(sp =>
            new MongoConfiguration(configuration.GetValue<string>("ConnectionStrings:MongoDb")));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        return services;
    }
}
