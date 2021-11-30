using System;
using System.Collections.Generic;
using TetraPak;
using TetraPak.AspNet;

namespace demo.Acme.Models
{
    /// <summary>
    ///   Convenient <see cref="Product"/> helper methods.
    /// </summary>
    public static class ProductHelper
    {
        /// <summary>
        ///   Adds one or more assets (ids) to the product.    
        /// </summary>
        /// <returns>
        ///   An <see cref="EnumOutcome{T}"/> to indicate success/failure and, on success, also carry
        ///   the identities of all added assets (a <see cref="string"/> collection)
        ///   or, on failure, an <see cref="Exception"/>.
        /// </returns>
        public static EnumOutcome<string> AddAssets(this Product self, params string[] assetIds)
        {
            var hashSet = (HashSet<string>) self.AssetIds;
            foreach (var assetId in assetIds)
            {
                if (hashSet.Contains(assetId))
                    return EnumOutcome<string>.Fail(new IdentityConflictException($"Asset {assetId} is already added"));
            }

            foreach (var assetId in assetIds)
            {
                hashSet.Add(assetId);
            }

            return EnumOutcome<string>.Success(assetIds);
        }

        /// <summary>
        ///   Removes one or more assets (ids) from the product.    
        /// </summary>
        /// <returns>
        ///   An <see cref="EnumOutcome{T}"/> to indicate success/failure and, on success, also carry
        ///   the identities of all removed assets (a <see cref="string"/> collection)
        ///   or, on failure, an <see cref="Exception"/>.
        /// </returns>
        public static EnumOutcome<string> RemoveAssets(this Product self, params string[] assetIds)
        {
            var hashSet = (HashSet<string>) self.AssetIds;
            foreach (var assetId in assetIds)
            {
                if (!hashSet.Contains(assetId))
                    return EnumOutcome<string>.Fail(ServerException.NotFound($"Asset {assetId} not found"));
            }

            foreach (var assetId in assetIds)
            {
                hashSet.Remove(assetId);
            }
            
            return EnumOutcome<string>.Success(assetIds);
        }    }
}