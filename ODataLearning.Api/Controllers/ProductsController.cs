using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODataLearning.Api.DataAccess;
using ODataLearning.Api.Entities;
using ODataLearning.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODataLearning.Api.Controllers
{

    public class ProductsController : ODataController
    {
        private readonly AppDbContext _context;
        public ProductsController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_context.Products.AsQueryable());
        }

        [ODataRoute("products({stock})")]
        public IActionResult GetWithFilter(int stock)
        {
            return Ok(_context.Products.Where(p => p.Stock == stock));
        }
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(product);
        }
        [HttpPut]
        public IActionResult Put([FromODataUri] int key, [FromBody] Product product)
        {
            product.Id = key;
            _context.Update(product);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete]
        public IActionResult Delete([FromODataUri]int key)
        {
            var product = _context.Products.Find(key);
            _context.Products.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpPost]
        public IActionResult Login(ODataActionParameters parameters)
        {
            Login login = parameters["UserLogin"] as Login;
            return Ok(login.Email);
        }
        [HttpGet]
        public IActionResult MultiplyFunction([FromODataUri]int a1, [FromODataUri] int a2, [FromODataUri] int a3)
        {
            return Ok(a1 * a2 * a3);
        }
        [HttpGet]
        public IActionResult KdvHesapla(int key,double kdv)
        {
            var product = _context.Products.Find(key);
            var price =(double) product.Price + ((double)product.Price * kdv);
            return Ok(price);
        }
    }
}
