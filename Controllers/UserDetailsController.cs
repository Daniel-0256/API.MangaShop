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
    public class UserDetailsController : ApiController
    {
        private DBContext db = new DBContext();

        // GET: api/UserDetails
        public IQueryable<UserDetails> GetUserDetails()
        {
            return db.UserDetails.Where(ud => ud.IsActive);
        }

        // GET: api/UserDetails/5
        public IHttpActionResult GetUserDetails(int id)
        {
            UserDetails userDetails = db.UserDetails.FirstOrDefault(ud => ud.UserDetailsId == id && ud.IsActive);
            if (userDetails == null)
            {
                return NotFound();
            }

            return Ok(userDetails);
        }

        // GET: api/UserDetails/User/{userId}
        [Route("api/UserDetails/User/{userId}")]
        [ResponseType(typeof(List<UserDetails>))]
        public IHttpActionResult GetUserDetailsByUserId(int userId)
        {
            var userDetails = db.UserDetails.Where(ud => ud.UserId == userId && ud.IsActive).ToList();
            if (userDetails == null || !userDetails.Any())
            {
                return Ok(new List<UserDetails>()); // Restituisci un array vuoto anziché un errore 404
            }
            return Ok(userDetails);
        }


        // PUT: api/UserDetails/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserDetails(int id, UserDetailsIsActiveDto userDetailsDto)
        {
            var dbUserDetails = db.UserDetails.Find(id);
            if (dbUserDetails == null)
            {
                return NotFound();
            }

            dbUserDetails.IsActive = userDetailsDto.IsActive;
            db.Entry(dbUserDetails).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDetailsExists(id))
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



        // POST: api/UserDetails
        [ResponseType(typeof(UserDetails))]
        public IHttpActionResult PostUserDetails(UserDetails userDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserDetails.Add(userDetails);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userDetails.UserDetailsId }, userDetails);
        }

        // DELETE: api/UserDetails/5
        [ResponseType(typeof(UserDetails))]
        public IHttpActionResult DeleteUserDetails(int id)
        {
            UserDetails userDetails = db.UserDetails.Find(id);
            if (userDetails == null)
            {
                return NotFound();
            }

            db.UserDetails.Remove(userDetails);
            db.SaveChanges();

            return Ok(userDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserDetailsExists(int id)
        {
            return db.UserDetails.Count(e => e.UserDetailsId == id) > 0;
        }
    }
}