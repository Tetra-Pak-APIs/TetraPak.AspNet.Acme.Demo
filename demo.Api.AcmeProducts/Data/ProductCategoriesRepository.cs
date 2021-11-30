using System.Threading.Tasks;
using demo.Acme.Models;
using demo.Acme.Repositories;
using Microsoft.Extensions.Logging;
using TetraPak;

namespace demo.AcmeProducts.Data
{
    public class ProductCategoriesRepository : Repository<Category>
    {
        protected override Task<Outcome<Category>> OnMakeNewItemAsync(Category source) 
            => Task.FromResult(Outcome<Category>.Success(new (source.Id!)));

        protected override Task OnUpdateItemAsync(Category target, Category source)
        {
            // no need to "update" a string ...
            return Task.CompletedTask; 
        }
        
        public ProductCategoriesRepository(ILogger? logger) 
        : base(logger!)
        {
        }
    }
}