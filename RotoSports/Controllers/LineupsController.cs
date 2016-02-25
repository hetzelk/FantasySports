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
    public class LineupsController : Controller
    {
        private RotoSportsDB db = new RotoSportsDB();

        // GET: Lineups
        public ActionResult Index()
        {
            string userID = User.Identity.GetUserId();
            return View(db.Lineups.Where(x => x.UserId == userID));
        }

        // GET: Lineups
        public ActionResult ChangeSport(int? ID)
        {
            if (ID == null)
            {
                return RedirectToAction("Index");
            }
            Lineup lineup = db.Lineups.Find(ID);
            if (lineup.Sport == "NBA")
            {
                return RedirectToAction("NBAEditor", new { id = ID });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Lineups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lineup lineup = db.Lineups.Find(id);
            if (lineup == null)
            {
                return HttpNotFound();
            }
            return View(lineup);
        }

        // GET: Lineups/Create
        public ActionResult Create()
        {
            var UserID = User.Identity.GetUserId();
            ViewBag.UserId = UserID;
            Dictionary<int, string> allUserFiles = new Dictionary<int, string>();
            foreach (CSVFiles file in db.CSVFiles)
            {
                allUserFiles.Add(file.ID, file.Title);
            }

            ViewBag.AllFileDict = allUserFiles;
            return View();
        }

        // POST: Lineups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserId,Title,FileConnection,Details")] Lineup lineup)
        {
            int filekey = Convert.ToInt32(lineup.FileConnection);
            CSVFiles file = db.CSVFiles.Find(filekey);
            lineup.Sport = file.Sport;
            lineup.SingleLineup = "*/PG:**/SG:**/SF:**/PF:**/C:**/G:**/F:**/UTIL:*";
            if (ModelState.IsValid)
            {
                db.Lineups.Add(lineup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lineup);
        }

        // GET: Lineups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lineup lineup = db.Lineups.Find(id);
            if (lineup == null)
            {
                return HttpNotFound();
            }
            return View(lineup);
        }

        // POST: Lineups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserId,Title,Sport,Details,SingleLineup,PlayerList")] Lineup lineup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lineup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lineup);
        }
        // GET: Lineups/Edit/5
        public ActionResult NBAEditor(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Lineup lineup = db.Lineups.Find(id);
            List<string> fullLineup = lineup.SingleLineup.Split(new string[] { "*/" }, StringSplitOptions.None).ToList();
            fullLineup.Remove(fullLineup[0]);
            List<string> names = new List<string>();
            foreach (string item in fullLineup)
            {
                string[] newname = item.Split(new string[] { ":*" }, StringSplitOptions.None);
                names.Add(newname[1]);
            }
            ViewBag.Position1 = names[0];
            ViewBag.Position2 = names[1];
            ViewBag.Position3 = names[2];
            ViewBag.Position4 = names[3];
            ViewBag.Position5 = names[4];
            ViewBag.Position6 = names[5];
            ViewBag.Position7 = names[6];
            ViewBag.Position8 = names[7];
            
            List<string> starredPlayers = new List<string>();
            starredPlayers.Add("Keith Hetzel");
            starredPlayers.Add("Ray CisNerdos");
            starredPlayers.Add("Drew Otteson");
            starredPlayers.Add("Steph Curry");
            if (lineup == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.StarPlayers = starredPlayers;
            return View(lineup);
        }

        // POST: Lineups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NBAEditor(string spotPG, string spotSG, string spotSF, string spotPF, string spotC, string spotG, string spotF, string spotUTIL, [Bind(Include = "ID")] Lineup lineup)
        {
            string singleLineup = "*/PG:*" + spotPG + "*/SG:*" + spotSG + "*/SF:*" + spotSF + "*/PF:*" + spotPF + "*/C:*" + spotC + "*/G:*" + spotG + "*/F:*" + spotF + "*/UTIL:*" + spotUTIL;
            Lineup newlineup = db.Lineups.Find(lineup.ID);
            newlineup.SingleLineup = singleLineup;
            if (ModelState.IsValid)
            {
                db.Entry(newlineup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newlineup);
        }

        // GET: Lineups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lineup lineup = db.Lineups.Find(id);
            if (lineup == null)
            {
                return HttpNotFound();
            }
            return View(lineup);
        }

        // POST: Lineups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lineup lineup = db.Lineups.Find(id);
            db.Lineups.Remove(lineup);
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
