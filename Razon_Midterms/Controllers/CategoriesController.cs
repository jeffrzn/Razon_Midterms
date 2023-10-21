using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Razon_Midterms.Models;

namespace Razon_Midterms.Controllers
{
    public class CategoriesController : ApiController
    {
        private midtermsdbEntities db = new midtermsdbEntities();

        // GET: api/Categories/5
        [HttpGet]
        [Route("api/products/bycategory/{categoryCode}")]
        public IHttpActionResult GetProductsByCategoryCode(string categoryCode)
        {
            try
            {
                var category = db.Category.SingleOrDefault(c => c.CategoryCode == categoryCode);
                if (category != null)
                {
                    return NotFound();
                }

                var products = db.Product.Where(p => p.CategoryId == category.CategoryId).ToList();
                return Ok(products);
            } 
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Category.Count(e => e.CategoryId == id) > 0;
        }
    }
}