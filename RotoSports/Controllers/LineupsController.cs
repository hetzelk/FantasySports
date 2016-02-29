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
                return RedirectToAction("Editor", new { id = ID });
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
                return RedirectToAction("Index");
            }
            Lineup lineup = db.Lineups.Find(id);
            if (lineup == null)
            {
                return RedirectToAction("Index");
            }
            return View(lineup);
        }

        // GET: Lineups/Star/5$playerdetails
        public ActionResult Star(int? lineupid, string player, int? fileId)
        {
            if (lineupid == null)
            {
                return RedirectToAction("Index");
            }
            Lineup lineup = db.Lineups.Find(lineupid);
            if (lineup == null)
            {
                return RedirectToAction("Index");
            }
            string currentstars = lineup.PlayerList;
            if(currentstars == "noplayers")
            {
                currentstars = player + "*/*";
            }
            else
            {
                currentstars += player + "*/*";
                lineup.PlayerList = currentstars;
            }
            lineup.PlayerList = currentstars;
            if (ModelState.IsValid)
            {
                db.Entry(lineup).State = EntityState.Modified;
                db.SaveChanges();
                int? id = fileId;
                return RedirectToAction("Details", "CSVFiles", new { id = id });
            }
            return View(lineup);
        }

        // GET: Lineups/Star/5$playerdetails
        public ActionResult Add(int? id, string inputplayer)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            string[] displayplayer = inputplayer.Split('~');
            string stringplayer = "";
            foreach(string detail in displayplayer)
            {
                stringplayer += detail + " ";
            }
            ViewBag.DisplayPlayer = stringplayer;
            ViewBag.NewPlayer = inputplayer;
            Lineup lineup = db.Lineups.Find(id);
            string stringtitlelist = lineup.BaseTitleList;
            List<string> allLines = lineup.SingleLineup.Split(new string[] { "*/*" }, StringSplitOptions.None).ToList();
            string[] titles = stringtitlelist.Split(',');

            List<string> titleList = new List<string>();
            foreach (string title in titles)
            {
                string newtitle = title.Replace("\"", "");
                titleList.Add(newtitle);
            }
            List<string[]> allPlayersArrays = new List<string[]>();
            foreach (string player in allLines)
            {
                string[] details = player.Split('~');
                List<string> formatted = new List<string>();
                foreach (string item in details)
                {
                    string newitem = item.Replace("\"", "");
                    formatted.Add(newitem);
                }
                formatted.Remove(formatted[formatted.Count() - 1]);
                string[] endformat = formatted.ToArray();
                allPlayersArrays.Add(endformat);
            }
            int countlines = allLines.Count;
            int titletotal = titleList.Count;
            ViewBag.Titles = titleList;
            ViewBag.Total = countlines;
            ViewBag.AllPlayers = allPlayersArrays;
            return View(lineup);
        }
        // GET: Lineups/Star/5$playerdetails
        public ActionResult AddPlayer(int? id, string playerinput, int position)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Lineup lineup = db.Lineups.Find(id);
            if (lineup == null || playerinput == "")
            {
                return RedirectToAction("Index");
            }

            List<string> currentlineup = lineup.SingleLineup.Split(new string[] { "*/*" }, StringSplitOptions.None).ToList();
            string singlelineup = "";
            for (int i = 0; i < currentlineup.Count(); i++)
            {
                if (i == position)
                {
                    singlelineup += playerinput + "*/*";
                }
                else
                {
                    singlelineup += currentlineup[i] + "*/*";
                }
                
            }
            lineup.SingleLineup = singlelineup;
            if (ModelState.IsValid)
            {
                db.Entry(lineup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Editor", "Lineups", new { id = id });
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserId,Title,FileConnection,Details")] Lineup lineup)
        {
            int filekey = Convert.ToInt32(lineup.FileConnection);
            CSVFiles file = db.CSVFiles.Find(filekey);
            lineup.Sport = file.Sport;
            string sport = file.Sport;
            switch (sport)
            {
                case "NBA": lineup.SingleLineup = "PG:1 Empty Player Data~*/*SG:2 Empty Player Data~*/*SF:3 Empty Player Data~*/*PF:4 Empty Player Data~*/*C:5 Empty Player Data~*/*F:6 Empty Player Data~*/*F:7 Empty Player Data~*/*UTIL:8 Empty Player Data~"; break;
                case "MMA": lineup.SingleLineup = "Empty Fighter Data~*/*Empty Fighter Data~*/*Empty Fighter Data~*/*Empty Fighter Data~*/*Empty Fighter Data~"; break;
                case "CBB": lineup.SingleLineup = "G:Empty Player Data~*/*G:Empty Player Data~*/*G:Empty Player Data~*/*F:Empty Player Data~*/*F:Empty Player Data~*/*F:Empty Player Data~*/*UTIL:Empty Player Data~*/*UTIL:Empty Player Data~"; break;
                case "NHL": lineup.SingleLineup = "C:Empty Player Data~*/*C:Empty Player Data~*/*W:Empty Player Data~*/*W:Empty Player Data~*/*W:Empty Player Data~*/*D:Empty Player Data~*/*D:Empty Player Data~*/*G:Empty Player Data~*/*UTIL:Empty Player Data~"; break;
                case "NAS": lineup.SingleLineup = "Empty Driver Data~*/*Empty Driver Data~*/*Empty Driver Data~*/*Empty Driver Data~*/*Empty Driver Data~*/*Empty Driver Data~*/*"; break;
                case "LOL": lineup.SingleLineup = "Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*"; break;
                case "SOC": lineup.SingleLineup = "GK:Empty Player Data~*/*D:Empty Player Data~*/*D:Empty Player Data~*/*D:Empty Player Data~*/*M:Empty Player Data~*/*M:Empty Player Data~*/*M:Empty Player Data~*/*F:Empty Player Data~*/*F:Empty Player Data~*/*UTIL:Empty Player Data~*/*UTIL:Empty Player Data~"; break;
                case "NFL": lineup.SingleLineup = "Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*"; break;
                case "MLB": lineup.SingleLineup = "Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*"; break;
                case "PGA": lineup.SingleLineup = "Empty Golfer Data~*/*Empty Golfer Data~*/*Empty Golfer Data~*/*Empty Golfer Data~*/*Empty Golfer Data~*/*Empty Golfer Data~"; break;
                case "CFB": lineup.SingleLineup = "Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*"; break;
                default   : lineup.SingleLineup = "Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*Empty Player Data~*/*"; break;
            }
            lineup.PlayerList = "noplayers";
            lineup.BaseTitleList = file.BaseTitleList;
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
        public ActionResult Editor(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Lineup lineup = db.Lineups.Find(id);
            string stringtitlelist = lineup.BaseTitleList;
            List<string> allLines = lineup.SingleLineup.Split(new string[] { "*/*" }, StringSplitOptions.None).ToList();
            string[] titles = stringtitlelist.Split(',');

            List<string> titleList = new List<string>();
            foreach (string title in titles)
            {
                string newtitle = title.Replace("\"", "");
                titleList.Add(newtitle);
            }
            List<string[]> allPlayersArrays = new List<string[]>();
            foreach (string player in allLines)
            {
                string[] details = player.Split('~');
                List<string> formatted = new List<string>();
                foreach (string item in details)
                {
                    string newitem = item.Replace("\"", "");
                    formatted.Add(newitem);
                }
                formatted.Remove(formatted[formatted.Count() - 1]);
                string[] endformat = formatted.ToArray();
                allPlayersArrays.Add(endformat);
            }
            int countlines = allLines.Count;
            int titletotal = titleList.Count;
            ViewBag.Titles = titleList;
            ViewBag.Total = countlines;
            ViewBag.TitleTotal = titletotal;
            ViewBag.AllLines = allLines;
            ViewBag.AllPlayers = allPlayersArrays;

            List<string[]> starredPlayers = new List<string[]>();
            if (lineup.PlayerList != "noplayers")
            {
                ViewBag.NoPlayers = "false";
                List<string> allstars = lineup.PlayerList.Split(new string[] { "*/*" }, StringSplitOptions.None).ToList();
                allstars.Remove(allstars[allstars.Count - 1]);
                foreach (string player in allstars)
                {
                    List<string> thisplayer = player.Split('~').ToList();
                    thisplayer.Remove(thisplayer[thisplayer.Count - 1]);
                    string[] thisarray = thisplayer.ToArray();
                    starredPlayers.Add(thisarray);
                }
            }
            else
            {
                ViewBag.NoPlayers = "true";
            }
            if (lineup == null)
            {
                return RedirectToAction("Index");
            }
            List<string> countthestars = new List<string>();
            int countstars = 0;
            if (lineup.PlayerList != "empty")
            {
                countthestars = lineup.PlayerList.Split(new string[] { "*/*" }, StringSplitOptions.None).ToList();
                countthestars.Remove(countthestars[countthestars.Count - 1]);
                countstars = countthestars.Count;
            }
            else
            {
                countstars = 0;
                ViewBag.NoPlayers = "true";
            }
            ViewBag.TotalStars = countstars;
            ViewBag.StarPlayers = starredPlayers;
            return View(lineup);
        }

        // POST: Lineups/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editor([Bind(Include = "ID")] Lineup lineup)
        {
            Lineup newlineup = db.Lineups.Find(lineup.ID);
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
