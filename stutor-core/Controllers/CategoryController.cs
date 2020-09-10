using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using stutor_core.Database;
using stutor_core.Models.Sql;
using stutor_core.Services;

namespace stutor_core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(ApplicationDbContext db)
        {
            _categoryService = new CategoryService(db);
        }

        [HttpGet]
        public IEnumerable<Category> GetAll()
        {
            return _categoryService.GetAll();
        }
    }
}
