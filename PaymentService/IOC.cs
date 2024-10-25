using Microsoft.EntityFrameworkCore;
using PaymentService.Abstracts;
using PaymentService.Context;
using PaymentService.Factories;
using PaymentService.Services;
using PaymentService.Strategies;
using StackExchange.Redis;
namespace PaymentService
{
    public static class IOC
    {
        public static IServiceCollection ServiceRegistation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICacheService, CacheService>();
            services.AddScoped<SaleStrategy>();
            services.AddScoped<CancelStrategy>();
            services.AddScoped<RefundStrategy>();
            services.AddScoped<IFactory, Factory>();
            services.AddScoped<IRetryService, RetryService>();

            services.AddDbContext<PaymentServiceDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

            var redisConfig = configuration.GetSection("RedisConfig");

            var configOptions = new ConfigurationOptions
            {
                EndPoints = { redisConfig["ConnectionString"] },
                Password = null,
                AbortOnConnectFail = false 
            };

            // StackExchange.RedisCache ayarlarını ekliyoruz
            services.AddStackExchangeRedisCache(config =>
            {
                config.Configuration = configOptions.ToString();
            });


            return services;
        }
    }
}
