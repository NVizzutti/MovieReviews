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
                return View(reviews.ToList());
            }
        }
        
        public ActionResult All()
        {
            return View(db.Movies.ToList());
        } 

        public ActionResult Details(int? id)
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


        public ActionResult Create()
        {
            ViewBag.Movies = new SelectList(db.Movies, "Id", "Title");
            return View();
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
           // ViewBag.ApplicationUserId = new SelectList(db.ApplicationUsers, "Id", "Username", review.ApplicationUserId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Reviews/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Reviews/Delete/5
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
