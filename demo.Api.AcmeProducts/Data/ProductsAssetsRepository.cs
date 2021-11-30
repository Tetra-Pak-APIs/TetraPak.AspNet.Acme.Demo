using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using demo.Acme.Models;
using demo.Acme.Repositories;
using Microsoft.Extensions.Logging;
using TetraPak;

namespace demo.AcmeProducts.Data
{
    /// <summary>
    ///   This repository is for demonstration purposes only and holds Products->Assets (one-to-many) relationships.
    /// </summary>
    public class ProductsAssetsRepository : Repository<ProductAssetEntry>
    {
        protected override Task<Outcome<ProductAssetEntry>> OnMakeNewItemAsync(ProductAssetEntry source)
            => Task.FromResult(
                Outcome<ProductAssetEntry>.Success(new (source.Id, source.ProductId, source.AssetId)));

        protected override Task OnUpdateItemAsync(ProductAssetEntry target, ProductAssetEntry source)
        {
            throw new InvalidOperationException(
                $"Updating a {nameof(Product)}/{nameof(Asset)} relationship is not possible");
        }
        
        public ProductsAssetsRepository(ILogger logger) : base(logger)
        {
        }
    }
    
    /// <summary>
    ///   Represents a relationship between a <see cref="Product"/> and an <see cref="Asset"/>.
    /// </summary>
    public class ProductAssetEntry : Model
    {
        [Required]
        public string ProductId { get; }

        [Required]
        public string AssetId { get; }

        #region .  Equality  .

        bool equals(ProductAssetEntry other)
        {
            return base.Equals(other) && ProductId == other.ProductId && AssetId == other.AssetId;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && equals((ProductAssetEntry)obj);
        }

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), ProductId, AssetId);

        #endregion

        [JsonConstructor]
        public ProductAssetEntry(string? id, string productId, string assetId) 
        : base(id)
        {
            ProductId = productId;
            AssetId = assetId;
        }
    }
}