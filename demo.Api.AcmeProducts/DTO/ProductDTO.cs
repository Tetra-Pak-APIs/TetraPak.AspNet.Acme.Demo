using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using demo.Acme.Models;
using Newtonsoft.Json;
using TetraPak.AspNet.DataTransfers;

namespace demo.AcmeProducts.DTO
{
    public class ProductDTO : DataTransferObject
    {
        /// <summary>
        ///   (required)<br/>
        ///  Gets or sets a product name.   
        /// </summary>
        [Required]
        [JsonPropertyName("name"), JsonProperty("name")]
        public string? Name
        {
            get => Get<string>();
            set => Set(value);
        }
        
        /// <summary>
        ///   Gets or sets a product description.
        /// </summary>
        [JsonPropertyName("description"), JsonProperty("description")]
        public string? Description 
        {
            get => Get<string>();
            set => Set(value);
        }
        
        /// <summary>
        ///   Gets or sets the product price.
        /// </summary>
        [JsonPropertyName("price"), JsonProperty("price")]
        public float Price 
        {
            get => Get<float>();
            set => Set(value);
        }
        
        public ProductDTO(Product product, params DtoRelationshipBase[] relationships)
        {
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            Relationships = relationships;
        }
    }
}