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
    public class ProductsController : ApiController
    {
        private midtermsdbEntities db = new midtermsdbEntities();

        // GET: api/Products/5
        [HttpGet]
        [Route("api/Products/Find/{productCode}")]
        public IHttpActionResult GetProductByCode(string productCode)
        {
            // Find the product by its product code
            var product = db.Product.SingleOrDefault(p => p.ProductCode == productCode);

            if (product == null)
            {
                return NotFound(); // Product not found
            }

            // Map the product entity to a DTO
            var productDTO = new ProductDTO
            {
                ProductId = product.ProductId,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                CategoryId = product.CategoryId,
                Color = product.Color,
                Size = product.Size,
                Price = (int)product.Price,
            };

            return Ok(productDTO);
        }

        // PUT: api/Products/5
        [HttpPut]
        [Route("api/products/{productCode}")]
        public IHttpActionResult UpdateProductByCode(string productCode, [FromBody] ProductDTO updatedProductDTO)
        {
            try
            {
                // Find the product by its product code
                var product = db.Product.SingleOrDefault(p => p.ProductCode == productCode);

                if (product == null)
                {
                    return NotFound(); // Product not found
                }

                // Update the product properties based on the updatedProductDTO
                product.ProductName = updatedProductDTO.ProductName;
                product.ProductDescription = updatedProductDTO.ProductDescription;
                product.CategoryId = updatedProductDTO.CategoryId;
                product.Color = updatedProductDTO.Color;
                product.Size = updatedProductDTO.Size;
                product.Price = updatedProductDTO.Price;

                // Save changes to the database
                db.SaveChanges();

                return Ok("Product updated successfully.");
            }
            catch (DbUpdateException ex)
            {
                // Handle validation errors if any
                var validationErrors = db.GetValidationErrors();

                foreach (var entityValidationErrors in validationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Validation Error: {validationError.ErrorMessage}");
                    }
                }

                return BadRequest("Validation error. Please check the data.");
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                return InternalServerError(ex);
            }
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Product.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        [Route("api/Products/Find/{productCode}")]
        public IHttpActionResult DeleteByProductCode(string productCode)
        {
            var product = db.Product.SingleOrDefault(p => p.ProductCode == productCode);

            if (product == null)
            {
                return NotFound(); // Product not found
            }

            db.Product.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Product.Count(e => e.ProductId == id) > 0;
        }
    }
}