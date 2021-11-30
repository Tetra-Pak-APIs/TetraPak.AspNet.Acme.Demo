using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace demo.Acme.Models
{
    /// <summary>
    ///   Represents a generic ACME product, for demonstration purposes only. 
    /// </summary>
    public class Product : Model
    {
        /// <summary>
        ///   (required)<br/>
        ///   Gets or sets a collection of categories that applies for this product. 
        /// </summary>
        [Required]
        public string[] ProductCategories { get; set; }
        
        /// <summary>
        ///   (required)<br/>
        ///  Gets or sets a product name.   
        /// </summary>
        [Required]
        public string? Name { get; set; }
        
        /// <summary>
        ///   Gets or sets a product description.
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        ///   Gets or sets the product price.
        /// </summary>
        public float Price { get; set; }

        /// <summary>
        ///   Gets or sets a collection of assets (identities). 
        /// </summary>
        public IEnumerable<string> AssetIds { get; set; }

        public Product(string id) : base(id)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            AssetIds = new HashSet<string>();
            ProductCategories = Array.Empty<string>();
        }
    }
}