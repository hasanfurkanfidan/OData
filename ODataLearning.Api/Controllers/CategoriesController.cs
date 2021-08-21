using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODataLearning.Api.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODataLearning.Api.Controllers
{

    public class CategoriesController : ODataController
    {
        private readonly AppDbContext _context;
        public CategoriesController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            return Ok(_context.Categories.AsQueryable());
        }
        [HttpGet]
        [EnableQuery]

        [ODataRoute("categories({id})/products({item})")]
        public IActionResult ProductById([FromODataUri] int id, [FromODataUri] int item)
        {
            return Ok(_context.Products.Where(p => p.Id == item && p.CategoryId == id).AsQueryable());
        }
        [HttpPost]
        public IActionResult TotalProductPrice([FromODataUri] int key)
        {
            var productsPrice = _context.Products.Where(p => p.CategoryId == key).Sum(p => p.Price);



            return Ok(productsPrice);
        }
        [HttpPost]
        public IActionResult TotalProductPrice2()
        {
            var productsPrice = _context.Products.Sum(p => p.Price);
            return Ok(productsPrice);
        }
        [HttpPost]
        public IActionResult TotalProductPriceWithParameter(ODataActionParameters parameters)
        {
            var categoryId = (int)parameters["categoryId"];
            var productPrice = _context.Products.Where(p => p.CategoryId == categoryId).Sum(p => p.Price);
            return Ok(productPrice);
        }
        [HttpPost]
        public IActionResult Total(ODataActionParameters parameters)
        {
            int a = (int)parameters["a"];
            int b = (int)parameters["b"];
            int c = (int)parameters["c"];
            return Ok(a+b+c);
        }
        [HttpGet]
        public IActionResult GetCategoryCount()
        {
            return Ok(_context.Categories.Count());
        }
    }

}
