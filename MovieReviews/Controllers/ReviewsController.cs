using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieReviews.Models;
using Microsoft.AspNet.Identity;

namespace MovieReviews.Controllers
{
    public class ReviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentId = User.Identity.GetUserId();
                var userReviews = from r in db.Review where r.ApplicationUserId == currentId select r;
                var withMovie = userReviews.Include(r => r.Movie);
                return View(withMovie.ToList());
            } else
            {
                var reviews = from r in db.Review where r.ApplicationUserId == null select r;
                var withMovie = reviews.Include(r => r.Movie);
                return View(withMovie.ToList());
            }
        }
        
        public ActionResult All()
        {
            var reviews = from r in db.Review select r;
            var withMovie = reviews.Include(r => r.Movie);
            return View(withMovie.ToList());
        } 


        public ActionResult Create()
        {
            ViewBag.Movies = new SelectList(db.Movies, "Id", "Title");
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var reviews = from r in db.Review where r.Id == id select r;
            var withMovie = reviews.Include(r => r.Movie).First();
            ViewBag.Title = withMovie.Title;
            if (withMovie.User != null)
            {
                ViewBag.Author = withMovie.User.UserName;
            }
            ViewBag.Movie = withMovie.Movie.Title;
            if (withMovie == null)
            {
                return HttpNotFound();
            }
            return View(withMovie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ApplicationUserId,Title,Body,MovieId")] Review review)
        {
            review.ApplicationUserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Review.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(review);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Review.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ApplicationUserId,Title,Body")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "Email", review.ApplicationUserId);
            return View(review);
        }

        public ActionResult Delete(int? id)
        {
            var currentId = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Review.Find(id);
            if (review == null)
            {
                Response.Redirect("/reviews");
            }
            if (review.ApplicationUserId != currentId && review.ApplicationUserId != null)
            {
                Response.Redirect("/reviews");
            }
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review review = db.Review.Find(id);
            db.Review.Remove(review);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
