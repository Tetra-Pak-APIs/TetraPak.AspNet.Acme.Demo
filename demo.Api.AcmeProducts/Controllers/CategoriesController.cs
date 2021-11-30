using System.Threading.Tasks;
using demo.AcmeProducts.Data;
using Microsoft.AspNetCore.Mvc;
using TetraPak.AspNet.Api.Controllers;

namespace demo.AcmeProducts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        readonly ProductCategoriesRepository _repository;

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return await this.RespondAsync(await _repository.ReadAsync());
        }

        public CategoriesController(ProductCategoriesRepository repository)
        {
            _repository = repository;
        }
    }
    
}