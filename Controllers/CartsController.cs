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
using API.MangaShop.Models;

namespace API.MangaShop.Controllers
{
    public class CartsController : ApiController
    {
        private DBContext db = new DBContext();

        // GET: api/Carts
        public IQueryable<Cart> GetCart()
        {
            return db.Cart;
        }

        // GET: api/Carts/5
        [ResponseType(typeof(Cart))]
        public IHttpActionResult GetCart(int id)
        {
            Cart cart = db.Cart.Find(id);
            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        // GET: api/Carts/User/5
        [Route("api/Carts/User/{userId}")]
        public IQueryable<Cart> GetCartByUserId(int userId)
        {
            var cart = db.Cart.Where(c => c.UserId == userId);
            return cart;
        }

        // PUT: api/Carts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCart(int id, Cart cartUpdate)
        {
            var cart = db.Cart.Find(id);
            if (cart == null)
            {
                return NotFound();
            }

            cart.Quantity = cartUpdate.Quantity; 

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/Carts
        [ResponseType(typeof(Cart))]
        public IHttpActionResult PostCart(Cart cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCartItem = db.Cart.FirstOrDefault(c => c.ProductId == cart.ProductId && c.UserId == cart.UserId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cart.Quantity;
            }
            else
            {
                db.Cart.Add(cart);
            }

            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = cart.CartId }, cart);
        }


        // DELETE: api/Carts/5
        [ResponseType(typeof(Cart))]
        public IHttpActionResult DeleteCart(int id)
        {
            Cart cart = db.Cart.Find(id);
            if (cart == null)
            {
                return NotFound();
            }

            db.Cart.Remove(cart);
            db.SaveChanges();

            return Ok(cart);
        }

        // DELETE: api/Carts/User/5
        [Route("api/Carts/User/{userId}")]
        public IHttpActionResult DeleteAllCartsByUserId(int userId)
        {
            var carts = db.Cart.Where(c => c.UserId == userId).ToList();
            if (!carts.Any())
            {
                return NotFound();
            }

            foreach (var cart in carts)
            {
                db.Cart.Remove(cart);
            }

            db.SaveChanges();
            return Ok("All cart items have been successfully deleted.");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CartExists(int id)
        {
            return db.Cart.Count(e => e.CartId == id) > 0;
        }
    }
}