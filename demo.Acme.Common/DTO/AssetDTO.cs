using System;
using System.Text.Json.Serialization;
using demo.Acme.Models;
using TetraPak.Serialization;

namespace demo.Acme.DTO
{
    [Serializable, JsonConverter(typeof(DynamicEntityJsonConverter<AssetDTO>))]
    public class AssetDTO : DataTransferObject
    {
        public string? Description
        {
            get => Get<string?>();
            set => Set(value);
        }
        
        public string? Url 
        {
            get => Get<string?>();
            set => Set(value);
        }

        public string? MimeType
        {
            get => Get<string?>();
            set => Set(value);
        }

        [JsonConstructor]
        public AssetDTO()
        {
        }

        public AssetDTO(Asset asset)
        {
            Description = asset.Description;
            Url = asset.Url;
            MimeType = asset.MimeType;
        }
    }
}