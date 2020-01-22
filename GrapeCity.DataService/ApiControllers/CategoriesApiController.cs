using GrapeCity.DataService.DTO;
using GrapeCity.DataService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GrapeCity.DataService.ApiControllers
{
    [Route("api/Categories")]
    [ApiController]
    public class CategoriesApiController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public CategoriesApiController(NorthwindContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IQueryable<CategoryDto> GetCategories()
        {
            return _context.Categories.Select(DtoConverter.AsCategoryDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return DtoConverter.ConvertToCategoryDto(category);
        }

        [HttpGet("{id}/Details")]
        public async Task<ActionResult<CategoryDetailDto>> GetCategoryDetail(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return DtoConverter.ConvertToCategoryDetailDto(category);
        }

        [HttpGet("{id}/Products")]
        public IQueryable<ProductDto> GetProductsInCategory(int id)
        {
            return _context.Categories.Where(c => c.CategoryId == id)
                .SelectMany(c => c.Products)
                .Select(DtoConverter.AsProductDto);
        }
    }
}
