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
    public class CSVFilesController : Controller
    {
        private RotoSportsDB db = new RotoSportsDB();

        // GET: CSVFiles
        public ActionResult Index()
        {
            return View(db.CSVFiles.ToList());
        }

        // GET: CSVFiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSVFiles cSVFiles = db.CSVFiles.Find(id);
            if (cSVFiles == null)
            {
                return HttpNotFound();
            }
            return View(cSVFiles);
        }

        // GET: CSVFiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CSVFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserId,Title,Sport,File")] CSVFiles cSVFiles)
        {
            if (ModelState.IsValid)
            {
                db.CSVFiles.Add(cSVFiles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cSVFiles);
        }

        // GET: CSVFiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSVFiles cSVFiles = db.CSVFiles.Find(id);
            if (cSVFiles == null)
            {
                return HttpNotFound();
            }
            return View(cSVFiles);
        }

        // POST: CSVFiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserId,Title,Sport,File")] CSVFiles cSVFiles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cSVFiles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cSVFiles);
        }

        // GET: CSVFiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CSVFiles cSVFiles = db.CSVFiles.Find(id);
            if (cSVFiles == null)
            {
                return HttpNotFound();
            }
            return View(cSVFiles);
        }

        // POST: CSVFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CSVFiles cSVFiles = db.CSVFiles.Find(id);
            db.CSVFiles.Remove(cSVFiles);
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
