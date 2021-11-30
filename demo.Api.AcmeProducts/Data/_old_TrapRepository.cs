// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using demo.Acme.Common;
// using demo.Acme.Common.Repositories;
// using Microsoft.Extensions.Logging;
// using TetraPak;
// using TetraPak.AspNet;
//
// namespace demo.Api.AcmeProducts.Data
// {
//     public class _old_TrapRepository : SimpleRepository<Product>
//     {
//         internal const string TrapProductType = "trap";
//         
//         readonly Dictionary<string, Product> _traps;
//         readonly AssetsRepository _assetsRepository;
//
//         public async Task<Outcome<string>> CreateAsync(Product trap, CancellationToken? cancellation = null)
//         {
//             await SimulateSlowRepositoryAsync(cancellation);
//
//             if (string.IsNullOrEmpty(trap.Id) && _traps.ContainsKey(trap.Id))
//                 return Outcome<string>.Fail(
//                     new IdentityConflictException(trap.Id, $"Asset with identity '{trap.Id}' was already posted"));
//
//             var newId = new RandomString();
//             _traps.Add(newId, new Product(newId)
//             {
//                 ProductType = TrapProductType,
//                 Description = trap.Description,
//                 Name = trap.Name,
//                 Price = trap.Price,
//                 AssetIds = trap.AssetIds,
//             });
//             return Outcome<string>.Success(newId);
//         }
//         
//         public async Task<EnumOutcome<Product>> ReadAsync(string[]? ids = null, CancellationToken? cancellation = null)
//         {
//             await SimulateSlowRepositoryAsync(cancellation);
//
//             if (!ids?.Any() ?? true)
//                 return EnumOutcome<Product>.Success(_traps.Values.ToArray());
//
//             var result = new List<Product>();
//             foreach (var id in ids)
//             {
//                 if (!_traps.TryGetValue(id, out var product))
//                     return EnumOutcome<Product>.Fail(
//                         new ResourceNotFoundException($"Trap with id '{id}' was not found"));
//                 
//                 result.Add(product);
//             }
//             
//             return EnumOutcome<Product>.Success(result.ToArray());
//         }
//         
//         public async Task<bool> ContainsAsync(string id, CancellationToken? cancellation = null)
//         {
//             await SimulateSlowRepositoryAsync(cancellation);
//             return _traps.ContainsKey(id);
//         }
//
//         public async Task<Outcome<string>> CreateOrUpdateAsync(Product? product, CancellationToken? cancellation = null)
//         {
//             await SimulateSlowRepositoryAsync(cancellation);
//
//             if (product is null)
//                 return Outcome<string>.Fail(new Exception("Expected a value"));
//
//             if (_traps.TryGetValue(product.Id, out _))
//                 return await UpdateAsync(product); 
//                 
//             _traps.Add(product.Id, product);
//             return Outcome<string>.Success(product.Id);
//         }
//
//         public async Task<Outcome<string>> UpdateAsync(Product? trap, CancellationToken? cancellation = null)
//         {
//             await SimulateSlowRepositoryAsync(cancellation);
//             
//             if (trap is null)
//                 return Outcome<string>.Fail(new Exception("Expected a value"));
//
//             if (!_traps.TryGetValue(trap.Id, out var existing))
//                 return Outcome<string>.Fail(
//                     new ResourceNotFoundException($"Trap with id '{trap.Id}' was not found"));
//
//             existing.UpdateFrom(trap);
//             return Outcome<string>.Success(existing.Id);
//         }
//
//         public _old_TrapRepository(ILogger logger, AssetsRepository assetsRepository) 
//         : base(logger)
//         {
//             _traps = new Dictionary<string, Product>();
//             _assetsRepository = assetsRepository;
//         }
//     }
// }