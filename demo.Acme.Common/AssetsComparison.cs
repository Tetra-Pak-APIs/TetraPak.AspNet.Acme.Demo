using System.Collections.Generic;
using System.Linq;
using demo.Acme;
using demo.Acme.Models;

namespace demo.DataModel
{
    public class AssetsComparison
    {
        public Asset[] Changed { get; }
        public Asset[] Added { get; } 
        public Asset[] Removed { get; }

        public AssetsComparison(IEnumerable<Asset> changed, IEnumerable<Asset> added, IEnumerable<Asset> removed)
        {
            Changed = changed.ToArray();
            Added = added.ToArray();
            Removed = removed.ToArray();
        }
    }
}