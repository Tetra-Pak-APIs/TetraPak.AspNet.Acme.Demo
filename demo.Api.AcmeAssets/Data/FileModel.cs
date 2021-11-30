using demo.Acme.Models;
using Microsoft.AspNetCore.Http;

namespace demo.AcmeAssets.Data
{
    public class FileModel : Model
    {
        public IFormFile File { get; set; }
        
        public FileModel(string? id, IFormFile file) 
        : base(id)
        {
            File = file;
        }
    }
}