using TetraPak.AspNet.Api;

namespace demo.AcmeProducts.Services
{
    public class AssetsService : BackendService<AssetsService.AssetsEndpoints>
    {
        public class AssetsEndpoints : ServiceEndpoints
        {
            public ServiceEndpoint Assets => GetEndpoint();
        }

        public AssetsService(AssetsEndpoints endpoints) : base(endpoints)
        {
        }
    }
}