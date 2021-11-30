using System.Collections.Generic;
using System.Collections.Immutable;
using demo.Acme.Models;
using demo.DataModel;
using TetraPak;

namespace demo.Acme
{
    public static class AssetsHelper
    {
        public static Outcome<AssetsComparison> IsEqual(this IEnumerable<Asset> self, IEnumerable<Asset> other)
        {
            var selfDict = self.ToImmutableDictionary(asset => asset.Id!);
            var otherDict = other.ToImmutableDictionary(asset => asset.Id!);

            if (selfDict.Count != otherDict.Count)
                return Outcome<AssetsComparison>.Fail();

            var changed = new List<Asset>();
            var added = new List<Asset>(otherDict.Values);
            var removed = new List<Asset>();
            foreach (var key in selfDict.Keys)
            {
                if (!otherDict.ContainsKey(key))
                {
                    removed.Add(selfDict[key]);
                    continue;
                }

                if (selfDict[key].IsSemanticallyEqual(otherDict[key]))
                {
                    added.Remove(otherDict[key]);
                    continue;
                }
                
                changed.Add(otherDict[key]);
            }
            return Outcome<AssetsComparison>.Success(new AssetsComparison(changed, added, removed));
        }

        public static bool IsSemanticallyEqual(this Asset self, Asset other)
        {
            return self.Id == other.Id
                   && self.Description == other.Description
                   && self.Url == other.Url;
        }

        public static void UpdateFrom(this Asset self, Asset other)
        {
            if (!string.IsNullOrEmpty(other.Description))
            {
                self.Description = other.Description;
            }

            if (!string.IsNullOrEmpty(other.Url))
            {
                self.Url = other.Url;
            }
            
            if (!string.IsNullOrEmpty(other.MimeType))
            {
                self.MimeType = other.MimeType;
            }
        }
    }
}