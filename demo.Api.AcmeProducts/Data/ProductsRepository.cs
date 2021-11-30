using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using demo.Acme.Models;
using demo.Acme.Repositories;
using Microsoft.Extensions.Logging;
using TetraPak;
using TetraPak.AspNet;

namespace demo.AcmeProducts.Data
{
    public class ProductsRepository : Repository<Product>
    {
        internal async Task<EnumOutcome<Product>> ReadWhereCategoriesAsync(
            IEnumerable<string> categories, 
            CancellationToken? cancel = null)
        {
            var products = await ReadWhereAsync(p 
                => p.ProductCategories.ContainsAny(categories, StringComparison.InvariantCultureIgnoreCase), cancel);
            return products.Any()
                ? EnumOutcome<Product>.Success(products)
                : EnumOutcome<Product>.Fail(ServerException.NotFound("No products found for specified categories"));
        }

        protected override Task<Outcome<Product>> OnMakeNewItemAsync(Product source)
        {
            var newId = string.IsNullOrEmpty(source.Id) ? new RandomString() : source.Id;
            return Task.FromResult(Outcome<Product>.Success(
                new (newId)
                {
                    ProductCategories = source.ProductCategories,
                    Name = source.Name,
                    Description = source.Description,
                    Price = source.Price,
                    AssetIds = source.AssetIds
                }));
        }

        protected override Task OnUpdateItemAsync(Product target, Product source)
        {
            if (target.Name != source.Name && !string.IsNullOrEmpty(source.Name))
            {
                target.Name = source.Name;
            }
            if (target.Description != source.Description && !string.IsNullOrEmpty(source.Description))
            {
                target.Description = source.Description;
            }
            if (Math.Abs(target.Price - source.Price) > float.Epsilon && source.Price >= 0f)
            {
                target.Price = source.Price;
            }

            var targetCategories = new MultiStringValue(target.ProductCategories);
            var sourceCategories = new MultiStringValue(source.ProductCategories);
            if (!targetCategories.EqualsSemantically(sourceCategories))
            {
                target.ProductCategories = source.ProductCategories;
            }

            var targetAssets = new MultiStringValue(target.AssetIds.ToArray());
            var sourceAssets = new MultiStringValue(source.AssetIds.ToArray());
            if (!targetAssets.EqualsSemantically(sourceAssets))
            {
                target.AssetIds = source.AssetIds;
            }

            return Task.CompletedTask;
        }
        
        public async Task RemoveCategoriesFromAllProducts(string categories, CancellationToken cancel)
        {
            var cats = (MultiStringValue)categories;
            var outcome = await ReadWhereCategoriesAsync((MultiStringValue)categories, cancel);
            if (!outcome)
                return;
            
            foreach (var product in outcome.Value!)
            {
                var newCategories = product.ProductCategories.Where(pc =>
                    !cats.Any(s => s.Equals(pc, StringComparison.InvariantCultureIgnoreCase)
                ));
                product.ProductCategories = newCategories.ToArray();
            }
        }
        
        public ProductsRepository(ILogger? logger) 
        : base(logger!)
        {
        }
    }
}