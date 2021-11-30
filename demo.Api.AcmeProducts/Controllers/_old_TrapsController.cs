// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using demo.Api.AcmeProducts.Data;
// using demo.Acme.Common;
// using demo.Acme.Common.Repositories;
// using demo.Acme.Common.Transfer;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using TetraPak;
// using TetraPak.AspNet.Api.Controllers;
//
// namespace demi.AcmeTraps.Controllers
// {
//     [ApiController, Route("[controller]"), Authorize]
//     public class TrapsController : ControllerBase
//     {
//         readonly _old_TrapRepository _oldTrapsRepository;
//
//         [HttpGet]
//         public async Task<ActionResult> Get(string? id = null)
//         {
//             var aborted = HttpContext.RequestAborted;
//             var trapDTOs = await readTrapDTOsAsync(id is { } ? new string[] { id } : null, aborted);
//             return await this.RespondAsync(trapDTOs);
//         }
//
//         [HttpPost]
//         public async Task<ActionResult> PostAsync([FromBody] TrapDTO trap)
//         {
//             var abort = HttpContext.RequestAborted;
//
//             if (!string.IsNullOrEmpty(trap.Id) && await _oldTrapsRepository.ContainsAsync(trap.Id))
//             {
//                 return await this.RespondAsync(Outcome<object>.Fail(
//                     new IdentityConflictException(trap.Id, $"Trap with id {trap.Id} was already registered")));
//             }
//             
//             // note: There's no transactional protection. Some assets might be created/updated even when the transaction fails
//             var assetsList = new List<string>();
//             foreach (var asset in trap.Assets)
//             {
//                 var outcome = await _assetsRepository.CreateOrUpdateAsync(asset, abort);
//                 if (!outcome)
//                     return await this.RespondAsync(outcome);
//                 
//                 assetsList.Add(outcome.Value!);
//             }
//             
//             var postOutcome = await _oldTrapsRepository.CreateAsync(new Product(trap.Id ?? new RandomString())
//             {
//                 ProductType = _old_TrapRepository.TrapProductType,
//                 Name = trap.Name,
//                 Description = trap.Description,
//                 Price = trap.Price,
//                 AssetIds = assetsList
//             }, abort);
//
//             return this.RespondOkCreated(postOutcome.Value!);
//         }
//
//         [HttpPut]
//         public async Task<ActionResult> PutAsync([FromBody] TrapDTO trap)
//         {
//             var abort = HttpContext.RequestAborted;
//
//             if (string.IsNullOrEmpty(trap.Id) || !await _oldTrapsRepository.ContainsAsync(trap.Id))
//                 return await PostAsync(trap);
//
//             var assetsList = new List<string>();
//             foreach (var asset in trap.Assets)
//             {
//                 var outcome = await _assetsRepository.CreateOrUpdateAsync(asset, abort);
//                 if (!outcome)
//                     return await this.RespondAsync(outcome);
//                 
//                 assetsList.Add(outcome.Value!);
//             }
//          
//             var putOutcome = await _oldTrapsRepository.CreateOrUpdateAsync(new Product(trap.Id ?? new RandomString())
//             {
//                 ProductType = _old_TrapRepository.TrapProductType,
//                 Name = trap.Name,
//                 Description = trap.Description,
//                 Price = trap.Price,
//                 AssetIds = assetsList
//             }, abort);
//             
//             return this.RespondOkCreated(putOutcome.Value!);
//         }
//
//         public async Task<ActionResult> PatchAsync([FromBody] TrapDTO trap)
//         {
//             var abort = HttpContext.RequestAborted;
//             if (string.IsNullOrEmpty(trap.Id))
//                 return this.RespondErrorBadRequest(new Exception("Submitted trap must have an id"));
//
//             var readOutcome = await readTrapDTOsAsync(new[] { trap.Id }, abort);
//             if (readOutcome)
//                 return await this.RespondAsync(readOutcome);
//
//             var existingTrap = readOutcome.Value!.First();
//             var assetsList = new List<string>();
//             foreach (var asset in trap.Assets)
//             {
//                 var outcome = await _assetsRepository.CreateOrUpdateAsync(asset, abort);
//                 if (!outcome)
//                     return await this.RespondAsync(outcome);
//                 
//                 assetsList.Add(outcome.Value!);
//             }
//             
//             trap.AssetIds = assetsList;
//             trap.Assets = null;
//             var updateOutcome = await _oldTrapsRepository.UpdateAsync(trap);
//             return updateOutcome
//                 ? this.RespondOk()
//                 : await this.RespondAsync(updateOutcome);
//         }
//
//         async Task<EnumOutcome<TrapDTO>> readTrapDTOsAsync(string[]? ids, CancellationToken? aborted)
//         {
//             var readOutcome = await _oldTrapsRepository.ReadAsync(ids, aborted);
//             if (!readOutcome)
//                 return EnumOutcome<TrapDTO>.Fail(readOutcome.Exception);
//
//             var traps = readOutcome.Value!.ToArray();
//             var dtoList = new List<TrapDTO>();
//             for (var i = 0; i < traps.Length; i++)
//             {
//                 var trap = traps[i];
//                 var assetsOutcome = await _assetsRepository.ReadAsync(trap.AssetIds.ToArray(), false, aborted);
//                 if (!assetsOutcome)
//                     return EnumOutcome<TrapDTO>.Fail(assetsOutcome.Exception);
//
//                 dtoList.Add(new TrapDTO(trap.Id)
//                 {
//                     ProductType = trap.ProductType,
//                     Name = trap.Name,
//                     Description = trap.Description,
//                     Assets = assetsOutcome.Value,
//                     Price = trap.Price
//                 });
//             }
//             return EnumOutcome<TrapDTO>.Success(dtoList.ToArray());
//         }
//
//         public TrapsController(_old_TrapRepository oldTrapsRepository, AssetsRepository assetsRepository)
//         {
//             _oldTrapsRepository = oldTrapsRepository;
//             _assetsRepository = assetsRepository;
//         }
//     }
// }