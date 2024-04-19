using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using API.MangaShop.Models;

namespace API.MangaShop.Controllers
{
    public class ProductsController : ApiController
    {
        private DBContext db = new DBContext();

        // GET: api/Products
        public IQueryable<Products> GetProducts()
        {
            return db.Products;
        }

        // GET: api/Products/5
        [ResponseType(typeof(Products))]
        public IHttpActionResult GetProduct(int id)
        {
            Products product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        // GET: api/Products/5
        [ResponseType(typeof(Products))]
        public IHttpActionResult GetProduct(string nameProduct)
        {
            var filteredProducts = db.Products
                .Where(p => !string.IsNullOrEmpty(nameProduct) && p.NameProduct.Contains(nameProduct))
                .ToList();

            return Ok(filteredProducts);
        }

        // GET: api/Products/AdvancedSearch?query=${query}
        [HttpGet]
        [Route("api/Products/AdvancedSearch")]
        [ResponseType(typeof(List<Products>))]
        public IHttpActionResult AdvancedSearch(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            var normalizedQuery = new string(query.Where(c => !char.IsPunctuation(c)).ToArray()).ToLower();
            var terms = Regex.Split(normalizedQuery, @"(?<=\d)(?=\D)|(?<=\D)(?=\d)|\s")
                             .Where(term => !string.IsNullOrEmpty(term))
                             .ToArray();

            var productsQuery = db.Products.AsQueryable();

            foreach (var term in terms)
            {
                productsQuery = productsQuery.Where(p => p.NameProduct.ToLower().Contains(term)
                                                     || p.Category.ToLower().Contains(term));
            }

            var resultList = productsQuery.ToList();
            if (!resultList.Any())
            {
                return NotFound();
            }

            return Ok(resultList);
        }


        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProducts(int id, Products products)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != products.ProductId)
            {
                return BadRequest("The ID in the URL does not match the ID of the product.");
            }

            var existingProduct = db.Products.Find(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            db.Entry(existingProduct).CurrentValues.SetValues(products);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return Conflict();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/Products
        [ResponseType(typeof(Products))]
        public IHttpActionResult PostProducts(Products products)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(products);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = products.ProductId }, products);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Products))]
        public IHttpActionResult DeleteProducts(int id)
        {
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return NotFound();
            }

            db.Products.Remove(products);
            db.SaveChanges();

            return Ok(products);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductsExists(int id)
        {
            return db.Products.Count(e => e.ProductId == id) > 0;
        }
    }
}