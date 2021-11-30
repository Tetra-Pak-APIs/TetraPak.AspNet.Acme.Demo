using System;
using System.Text.Json.Serialization;
using TetraPak.AspNet;
using TetraPak.DynamicEntities;
using TetraPak.Serialization;

namespace demo.Acme.DTO
{
    [Serializable, JsonConverter(typeof(DynamicEntityJsonConverter<DataTransferObject>))]
    public class DataTransferObject : DynamicEntity
    {
        public string Id
        {
            get => Get<string>();
            set => Set(value);
        }
    }
}