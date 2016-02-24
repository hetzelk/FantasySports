using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RotoSports.Models;

namespace RotoSports.Controllers
{
    public class NBAsController : Controller
    {
        private RotoSportsDB db = new RotoSportsDB();

        // GET: NBAs
        public ActionResult Index()
        {
            return View(db.NBAs.ToList());
        }

        // GET: NBAs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NBA nBA = db.NBAs.Find(id);
            if (nBA == null)
            {
                return HttpNotFound();
            }
            return View(nBA);
        }

        // GET: NBAs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NBAs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Position,Name,Salary,GameInfo,AvgPointsPerGame,teamAbbrev")] NBA nBA)
        {
            if (ModelState.IsValid)
            {
                db.NBAs.Add(nBA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nBA);
        }

        // GET: NBAs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NBA nBA = db.NBAs.Find(id);
            if (nBA == null)
            {
                return HttpNotFound();
            }
            return View(nBA);
        }

        // POST: NBAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Position,Name,Salary,GameInfo,AvgPointsPerGame,teamAbbrev")] NBA nBA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nBA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nBA);
        }

        // GET: NBAs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NBA nBA = db.NBAs.Find(id);
            if (nBA == null)
            {
                return HttpNotFound();
            }
            return View(nBA);
        }

        // POST: NBAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NBA nBA = db.NBAs.Find(id);
            db.NBAs.Remove(nBA);
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
