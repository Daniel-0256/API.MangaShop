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
    public class FavoritesController : ApiController
    {
        private DBContext db = new DBContext();

        // GET: api/Favorites
        public IQueryable<Favorites> GetFavorites()
        {
            return db.Favorites;
        }

        // GET: api/Favorites/5
        [ResponseType(typeof(Favorites))]
        public IHttpActionResult GetFavorites(int id)
        {
            Favorites favorites = db.Favorites.Find(id);
            if (favorites == null)
            {
                return NotFound();
            }

            return Ok(favorites);
        }

        // GET: api/Favorites/User/5
        [Route("api/Favorites/User/{userId}")]
        [ResponseType(typeof(Favorites))]
        public IHttpActionResult GetFavoritesByUserId(int userId)
        {
            var favorites = db.Favorites.Where(f => f.UserId == userId).ToList();

            return Ok(favorites);
        }


        // PUT: api/Favorites/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFavorites(int id, Favorites favorites)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != favorites.FavoriteId)
            {
                return BadRequest();
            }

            db.Entry(favorites).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavoritesExists(id))
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

        // POST: api/Favorites
        [ResponseType(typeof(Favorites))]
        public IHttpActionResult PostFavorites(Favorites favorites)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Favorites.Add(favorites);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = favorites.FavoriteId }, favorites);
        }

        // DELETE: api/Favorites/5
        [ResponseType(typeof(Favorites))]
        public IHttpActionResult DeleteFavorites(int id)
        {
            Favorites favorites = db.Favorites.Find(id);
            if (favorites == null)
            {
                return NotFound();
            }

            db.Favorites.Remove(favorites);
            db.SaveChanges();

            return Ok(favorites);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FavoritesExists(int id)
        {
            return db.Favorites.Count(e => e.FavoriteId == id) > 0;
        }
    }
}