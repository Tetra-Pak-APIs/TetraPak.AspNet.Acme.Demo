using demo.Acme.Seeding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace demo.AcmeProducts.Data
{
    public static class ServiceCollectionHelper
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton(p =>
            {
                // create and seed the simple products repository ...
                var repo = new ProductsRepository(p.GetService<ILogger<ProductsRepository>>());
                repo.Seed(ProductsSeeder.GetProductsSeed());
                return repo;
            });

            services.AddSingleton(p =>
            {
                // create and seed the simple product categories repository ...
                var repo = new ProductCategoriesRepository(p.GetService<ILogger<ProductCategoriesRepository>>());
                repo.Seed(ProductsSeeder.GetProductCategoriesSeed());
                return repo;
            });
        }
    }
}