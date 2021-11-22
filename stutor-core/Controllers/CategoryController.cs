using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using stutor_core.Models.Sql;
using stutor_core.Services.Interfaces;

namespace stutor_core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IEnumerable<Category> GetAll()
        {
            return _categoryService.GetAll();
        }

        [HttpGet]
        public IEnumerable<PopularCategory> GetAllPopular()
        {
            return _categoryService.GetAllPopular();
        }
    }
}
