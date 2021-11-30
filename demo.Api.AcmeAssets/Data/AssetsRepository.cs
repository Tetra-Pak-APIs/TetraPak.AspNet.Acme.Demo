using System.Threading.Tasks;
using demo.Acme;
using demo.Acme.Models;
using demo.Acme.Repositories;
using Microsoft.Extensions.Logging;
using TetraPak;

namespace demo.AcmeAssets.Data
{
    public class AssetsRepository : Repository<Asset>
    {
        protected override Task<Outcome<Asset>> OnMakeNewItemAsync(Asset source)
        {
            var id = string.IsNullOrWhiteSpace(source.Id) ? new RandomString(16) : source.Id;
            return Task.FromResult(Outcome<Asset>.Success(
                new Asset(id)
                {
                    Description = source.Description,
                    Url = source.Url,
                    MimeType = source.MimeType
                }));
        }

        protected override Task OnUpdateItemAsync(Asset target, Asset source)
        {
            target.UpdateFrom(source);
            return Task.CompletedTask;
        }

        public AssetsRepository(ILogger? logger) 
        : base(logger)
        {
        }
    }
}