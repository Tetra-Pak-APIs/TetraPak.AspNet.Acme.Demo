using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using demo.Acme.DTO;
using demo.Acme.Models;
using demo.AcmeProducts.Data;
using demo.AcmeProducts.DTO;
using demo.AcmeProducts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using TetraPak;
using TetraPak.AspNet;
using TetraPak.AspNet.Api;
using TetraPak.AspNet.Api.Controllers;
using TetraPak.AspNet.DataTransfers;

namespace demo.AcmeProducts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    // [Authorize]
    public class ProductsController : ControllerBase
    {
        readonly ProductsRepository _productsRepository;
        readonly ProductCategoriesRepository _categoriesRepository;
        readonly AssetsService _assetsService;

        #region .  Products  .

        [HttpGet, Route("{id?}")]
        public async Task<ActionResult> GetProducts(
            string? id = null, 
            [FromQuery] string? cat = null, 
            [FromQuery] string? assets = null)
        {
            var cats = (MultiStringValue)cat;
            var assetsRelLevel = RelationshipLevelHelper.Parse(assets);
            if (string.IsNullOrEmpty(id))
                return await this.RespondAsync(
                    cats.IsEmpty
                        ? await mapDtoAsync(await _productsRepository.ReadAsync(), assetsRelLevel)
                        : await mapDtoAsync(await _productsRepository.ReadWhereCategoriesAsync(cats), assetsRelLevel));
            
            if (!cats.IsEmpty)
                // passing id AND category is bad form ...
                return this.RespondErrorBadRequest("Duh! Either pass id OR category, not both!");
                
            var ids = (MultiStringValue)id;
            return await this.RespondAsync(ids.IsEmpty
                ? await mapDtoAsync(await _productsRepository.ReadAsync(cancellation: HttpContext.RequestAborted), assetsRelLevel)
                : await mapDtoAsync(await _productsRepository.ReadAsync(ids.Items, cancellation: HttpContext.RequestAborted), assetsRelLevel)); 
        }

        [HttpPost]
        public async Task<ActionResult> PostProduct([FromBody] Product? product)
        {
            if (product is null)
                return this.RespondErrorBadRequest("Expected product in body");

            return await this.RespondAsync(await _productsRepository.CreateAsync(product));
        }

        [HttpPut]
        public async Task<ActionResult> PutProduct([FromBody] Product? product)
        {
            if (product is null)
                return this.RespondErrorBadRequest("Expected product in body");

            return await this.RespondAsync(await _productsRepository.CreateOrReplaceAsync(product));
        }

        [HttpPatch]
        public async Task<ActionResult> PatchProduct([FromBody] Product? product)
        {
            if (product is null)
                return this.RespondErrorBadRequest("Expected product in body");

            return await this.RespondAsync(await _productsRepository.UpdateAsync(product));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(string id)
        {
            return await this.RespondAsync(await _productsRepository.DeleteAsync(id));
        }

        #endregion

        #region .  Categories  .

        // NOTE! we do not support PUT and PATCH for product categories

        [HttpGet, Route("Categories")]
        public async Task<ActionResult> GetCategories()
        {
            var outcome = await _categoriesRepository.ReadAsync();
            if (!outcome)
                return await this.RespondAsync(outcome);
            
            EnumOutcome<string> stringOutcome = 
                EnumOutcome<string>.Success(outcome.Value!.Select(i => i.StringValue).ToArray()); 
            return await this.RespondAsync(stringOutcome);
        }

        [HttpPost, Route("Categories")]
        public async Task<ActionResult> PostCategory([FromBody] string? category)
        {
            if (category is null)
                return this.RespondErrorBadRequest("Expected category in body");

            return await this.RespondAsync(await _categoriesRepository.CreateAsync(category));
        }
        
        [HttpDelete, Route("Categories")]
        public async Task<ActionResult> DeleteCategories(string categories)
        {
            // remove category from existing products, then the category itself
            await _productsRepository.RemoveCategoriesFromAllProducts(categories, HttpContext.RequestAborted);
            return await this.RespondAsync(await _categoriesRepository.DeleteAsync(categories));
        }
        #endregion
        
        async Task<EnumOutcome<ProductDTO>> mapDtoAsync(EnumOutcome<Product> productsOutcome, RelationshipLevel assetsRelLevel)
        {
            if (!productsOutcome)
                return EnumOutcome<ProductDTO>.Fail(productsOutcome.Exception);

            List<ProductDTO> list = new();
            foreach (var product in productsOutcome.Value!)
            {
                // get relationships for 'self', 'Categories' and 'Assets' ...  
                var productDto = new ProductDTO(product,
                    new DtoHrefRelationship(
                        "self",
                        this.GetRelLocatorForSelf(HttpMethod.Get)),
                    new DtoHrefRelationship(
                        "categories",
                        this.GetRelLocatorForDefaultAction<CategoriesController>()),
                    assetsRelLevel switch
                    {
                        RelationshipLevel.None => null!,
                        RelationshipLevel.Links => new DtoHrefRelationship(
                            "assets",
                            product.AssetIds.Select(i => _assetsService.Endpoints.Assets.GetRelLocatorFor(i))),
                        RelationshipLevel.Entities => null!,
                        _ => throw new NotSupportedException("this shouldn't happen but keeps compiler happy")
                    });
                if (assetsRelLevel == RelationshipLevel.Entities)
                {
                    var assetsOutcome = await getAssetsDtoAsync(product);
                    if (assetsOutcome)
                    {
                        productDto.SetValue("assets", assetsOutcome.Value!.Data);
                    }
                }
                
                list.Add(productDto);
            }
            
            return EnumOutcome<ProductDTO>.Success(list);
        }

        Task<HttpOutcome<ApiDataResponse<AssetDTO>>> getAssetsDtoAsync(Product product)
        {
            return _assetsService.Endpoints.Assets.GetAsync<AssetDTO>(
                product.AssetIds,
                requestOptions: RequestOptions.Default.WithCancellation(HttpContext.RequestAborted));
        }

        public ProductsController(
            ProductsRepository productsProductsRepository, 
            ProductCategoriesRepository categoriesRepository,
            AssetsService assetsService)
        {
            _productsRepository = productsProductsRepository;
            _categoriesRepository = categoriesRepository;
            _assetsService = assetsService;
        }
    }

    enum RelationshipLevel
    {
        None,
        Links,
        Entities
    }

    static class RelationshipLevelHelper
    {
        public static RelationshipLevel Parse(string? s, RelationshipLevel useDefault = RelationshipLevel.Links)
        {
            s = s?.Trim();
            if (string.IsNullOrEmpty(s))
                return useDefault;

            return Enum.TryParse<RelationshipLevel>(s, true, out var value)
                ? value
                : useDefault;
        }
    }
}