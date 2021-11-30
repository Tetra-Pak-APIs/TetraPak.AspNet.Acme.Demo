using System.Threading.Tasks;
using demo.Acme.Models;
using demo.AcmeAssets.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TetraPak;
using TetraPak.AspNet.Api.Controllers;
using TetraPak.AspNet.DataTransfers;
using TetraPak.DynamicEntities;

namespace demo.AcmeAssets.Controllers
{
    [ApiController]
    [Route("Media/[controller]")]
    //[Authorize]
    public class AssetsController : ControllerBase
    {
        readonly AssetsRepository _repository;
        readonly FilesRepository _filesRepository;

        [HttpGet, Route("{id?}")]
        public async Task<ActionResult> Get(string? id = null)
        {
            var ids = (MultiStringValue)id;
            var outcome = ids.IsEmpty
                ? await _repository.ReadAsync(cancellation: HttpContext.RequestAborted)
                : await _repository.ReadAsync(ids.Items, cancellation: HttpContext.RequestAborted);

            return await this.RespondAsync(outcome);
        }
        
        [HttpPost]
        public async Task<ActionResult> Post([FromForm] string? description, [FromForm] IFormFile? file)
        {
            if (file is null)
                return this.RespondErrorBadRequest("Expected 'file' in form");
            
            var cancel = HttpContext.RequestAborted;
            var fileModel = new FileModel(new RandomString(16), file);
            var createFileOutcome = await _filesRepository.CreateAsync(fileModel, cancel);
            if (!createFileOutcome)
                return this.RespondErrorInternalServer(createFileOutcome.Exception);

            var id = createFileOutcome.Value!;
            var locator = this.GetRelLocatorForAction(nameof(Get)).WithKeys(id);
            var asset = new Asset(fileModel.Id!)
            {
                Description = description,
                Url = locator.Uri.EnsurePrefix(FilePath.UnixSeparator),
                MimeType = file.ContentType
            };
            return await this.RespondAsync(await _repository.CreateAsync(asset));
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Asset? asset)
        {
            if (asset is null)
                return this.RespondErrorBadRequest("Expected asset in body");

            return await this.RespondAsync(await _repository.CreateOrReplaceAsync(asset));
        }
        
        [HttpPatch]
        public async Task<ActionResult> Patch([FromBody] Asset? asset)
        {
            if (asset is null)
                return this.RespondErrorBadRequest("Expected asset in body");

            return await this.RespondAsync(await _repository.UpdateAsync(asset));
        }
        
        [HttpDelete, Route("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            return await this.RespondAsync(await _repository.DeleteAsync(id));
        }
        
        public AssetsController(AssetsRepository repository, FilesRepository filesRepository)
        {
            _repository = repository;
            _filesRepository = filesRepository;
        }
    }
}