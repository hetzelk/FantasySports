using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RotoSports.Models;
using Microsoft.AspNet.Identity;

namespace RotoSports.Controllers
{
    public class CSVFilesController : Controller
    {
        private RotoSportsDB db = new RotoSportsDB();
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: CSVFiles
        public ActionResult Index()
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.UserId = UserID;
            List<CSVFiles> MyFiles = new List<CSVFiles>();
            foreach (CSVFiles eachFIle in db.CSVFiles)
            {
                if (eachFIle.UserId != UserID)
                {
                    //do nothing
                }
                else
                {
                    MyFiles.Add(eachFIle);
                }
            }
            return View(MyFiles);
        }

        // GET: CSVFiles/Details/5
        public ActionResult Details(int? id)
        {
            CSVFiles thisCSVfile = db.CSVFiles.Find(id);

            string[] allLines = thisCSVfile.File.Split(new string[] { "*/*" }, StringSplitOptions.None);

            ViewBag.AllLines = allLines;
            if (id == null)
            {
                return RedirectToAction("InvalidRequest", "Home");
            }
            if (thisCSVfile == null)
            {
                return RedirectToAction("InvalidRequest", "Home");
            }
            return View(thisCSVfile);
        }

        // GET: CSVFiles/Create
        public ActionResult Create(string sport)
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.UserId = UserID;
            ViewBag.SportName = sport;
            return View();
        }

        // POST: CSVFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserId,Title,Sport,Details,File")] CSVFiles cSVFiles)
        {
            string[] lines = cSVFiles.File.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string endFile = "";
            foreach (string line in lines)
            {
                string newLine = line + "*/*";
                endFile += newLine;
            }
            cSVFiles.File = endFile;
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
        public ActionResult Edit([Bind(Include = "ID,UserId,Title,Sport,Details,File")] CSVFiles cSVFiles)
        {
            string[] lines = cSVFiles.File.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string endFile = "";
            if(lines.Length == 1)
            {
                endFile = lines[0];
            }
            else
            {
                foreach (string line in lines)
                {
                    string newLine = line + "*/*";
                    endFile += newLine;
                }
            }
            cSVFiles.File = endFile;
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
