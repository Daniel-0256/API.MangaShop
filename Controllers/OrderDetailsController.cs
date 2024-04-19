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
    public class OrderDetailsController : ApiController
    {
        private DBContext db = new DBContext();

        // GET: api/OrderDetails
        public IQueryable<OrderDetails> GetOrderDetails()
        {
            return db.OrderDetails;
        }

        // GET: api/OrderDetails/5
        [ResponseType(typeof(OrderDetails))]
        public IHttpActionResult GetOrderDetails(int id)
        {
            OrderDetails orderDetails = db.OrderDetails.Find(id);
            if (orderDetails == null)
            {
                return NotFound();
            }

            return Ok(orderDetails);
        }

        // GET: api/OrderDetails/Order/5
        [Route("api/OrderDetails/Order/{orderId}")]
        [ResponseType(typeof(List<OrderDetails>))]
        public IHttpActionResult GetOrderDetailsByOrderId(int orderId)
        {
            var orderDetails = db.OrderDetails.Where(od => od.OrderId == orderId).Include(od => od.Products).ToList();
            if (orderDetails == null || orderDetails.Count == 0)
            {
                return NotFound();
            }

            return Ok(orderDetails);
        }

        // PUT: api/OrderDetails/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrderDetails(int id, OrderDetails orderDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderDetails.OrderDetailId)
            {
                return BadRequest();
            }

            db.Entry(orderDetails).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderDetailsExists(id))
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

        // POST: api/OrderDetails
        [ResponseType(typeof(OrderDetails))]
        public IHttpActionResult PostOrderDetails(OrderDetails orderDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OrderDetails.Add(orderDetails);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = orderDetails.OrderDetailId }, orderDetails);
        }

        // DELETE: api/OrderDetails/5
        [ResponseType(typeof(OrderDetails))]
        public IHttpActionResult DeleteOrderDetails(int id)
        {
            OrderDetails orderDetails = db.OrderDetails.Find(id);
            if (orderDetails == null)
            {
                return NotFound();
            }

            db.OrderDetails.Remove(orderDetails);
            db.SaveChanges();

            return Ok(orderDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderDetailsExists(int id)
        {
            return db.OrderDetails.Count(e => e.OrderDetailId == id) > 0;
        }
    }
}