using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace demo.Acme.Models
{
    public class Asset : Model
    {
        /// <summary>
        ///   Gets or sets a description og the assets.
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        ///   (required)<br/>
        ///   Gets or sets the asset universal resource locator.  
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        ///   (required)<br/>
        ///   Gets or sets the asset MIME type.  
        /// </summary>
        [Required]
        public string? MimeType { get; set; }

        public override string ToString()
        {
            return $"[{Description}] {Url}";
        }

        [JsonConstructor]
        public Asset(string id) : base(id)
        {
        }
    }
}