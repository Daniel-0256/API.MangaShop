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
    public class OrdersController : ApiController
    {
        private DBContext db = new DBContext();

        // GET: api/Orders
        public IQueryable<Orders> GetOrders()
        {
            return db.Orders;
        }

        // GET: api/Orders/5
        [ResponseType(typeof(List<Orders>))]
        public IHttpActionResult GetOrders(int id)
        {
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }


        // GET: api/Orders/User/5
        [Route("api/Orders/User/{userId}")]
        [ResponseType(typeof(List<Orders>))]
        public IHttpActionResult GetOrdersByUser(int userId)
        {
            var orders = db.Orders.Where(o => o.UserId == userId).ToList();
            if (!orders.Any())
            {
                return NotFound();
            }

            return Ok(orders);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrders(int id, Orders orderUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderUpdateDto.OrderId)
            {
                return BadRequest("The ID in the URL does not match the OrderId in the body.");
            }

            var order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = orderUpdateDto.Status;

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST: api/Orders
        [ResponseType(typeof(Orders))]
        public IHttpActionResult PostOrders(Orders orders)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orders.Add(orders);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = orders.OrderId }, orders);
        }


        // DELETE: api/Orders/5
        public IHttpActionResult DeleteOrders(int id)
        {
            Orders orders = db.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.OrderId == id);
            if (orders == null)
            {
                return NotFound();
            }

            db.OrderDetails.RemoveRange(orders.OrderDetails);

            db.Orders.Remove(orders);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                return InternalServerError(ex);
            }

            return Ok(orders);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrdersExists(int id)
        {
            return db.Orders.Count(e => e.OrderId == id) > 0;
        }
    }
}