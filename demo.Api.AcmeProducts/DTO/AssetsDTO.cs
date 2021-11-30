using System.Collections.Generic;
using demo.Acme.DTO;
using TetraPak.AspNet.DataTransfers;

namespace demo.AcmeProducts.DTO
{
    public class AssetsDTO : DtoRelationshipBase
    {
        public IEnumerable<AssetDTO> Type { get; set; }
    }
}