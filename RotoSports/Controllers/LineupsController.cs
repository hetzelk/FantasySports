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
using System.Net.Http;

namespace RotoSports.Controllers
{
    public class LineupsController : Controller
    {
        string PrimaryKey = "b0c04ca3bc254fa583b272a5ba86977e";
        string PlayerList = "";
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
            int countlines = allPlayersArrays.Count;
            int titletotal = titleList.Count;
            ViewBag.Titles = titleList;
            ViewBag.Total = countlines;
            ViewBag.AllPlayers = allPlayersArrays;

            string detailsOne = "";
            string detailsTwo = "";
            string sport = lineup.Sport;
            switch (sport)
            {
                case "NBA": detailsOne = "The Positions required for NBA is PG, SG, SF, PF, C, G, F, UTIL. "; detailsTwo = "PG/SG can be in the G position. SF/PF can be in the F position. Any player can be in the UTIL Position. Make sure to put the players in the correct positions."; break;
                case "MMA": detailsOne = "There are 5 positions required for a MMA lineup. "; detailsTwo = "You can pick fighters from the same fight.\nThey can be in any position."; break;
                case "CBB": detailsOne = "The Positions required for CBB is G, G, G, F, F, F, UTIL, UTIL. "; detailsTwo = "Any players can be in the UTIL positions.\nMake sure to put the players in the correct positions."; break;
                case "NHL": detailsOne = "The Positions required for NHL is C, C, W, W, W, D, D, G, UTIL. "; detailsTwo = "The W positions can be RW or LW players.\nMake sure to put the players in the correct positions."; break;
                case "NAS": detailsOne = "There are 5 positions required for NAS. "; detailsTwo = "They can be in any position."; break;
                case "LOL": detailsOne = "The Positions required for LOL is TOP, JNG, MID, ADC, SUP, FLEX, FLEX, TEAM. "; detailsTwo = "The players must be in the correct positions.\nAny players can be used in the FLEX positions.\nPick 1 team.\nMake sure to put the players in the correct positions."; break;
                case "SOC": detailsOne = "The Positions required for SOC is GK, D, D, D, M, M, M, F, F, UTIL, UTIL. "; detailsTwo = "GK can not be used in the UTIL positions.\nD, M, & F can be used in the UTIL positions.\nMake sure to put the players in the correct positions."; break;
                case "NFL": detailsOne = "The Positions required for NFL is QB, RB, RB, WR, WR, WR, TE, FLEX, DST. "; detailsTwo = "The QB cannot be used in the FLEX.\nEvery other position can be used in the FLEX.\nPick one DST.\nMake sure to put the players in the correct positions."; break;
                case "MLB": detailsOne = "The Positions required for MLB is P, P, C, 1B, 2B, 3B, OF, OF, OF. "; detailsTwo = "The W positions can be RW or LW players."; break;
                case "PGA": detailsOne = "There are 6 positions required for a PGA lineup. "; detailsTwo = "They can be in any position"; break;
                case "CFB": detailsOne = "The Positions required for NFL is QB, QB, RB, RB, WR, WR, WR, FLEX, FLEX. "; detailsTwo = "The QB cannot be used in the FLEX.\nThe other 2 positions can be used in the FLEX.\nMake sure to put the players in the correct positions."; break;
                default: detailsOne = "Go back and make sure you did your file and lineup properly"; detailsTwo = "Sorry about that."; break;
            }
            ViewBag.Details1 = detailsOne;
            ViewBag.Details2 = detailsTwo;
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
                if (currentlineup[i].Length <= 3)
                {
                    //do not add to string
                }
                else if (i == position)
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
case "NBA": lineup.SingleLineup = "PG:1 Empty Player Data~~~~~~*/*SG:2 Empty Player Data~~~~~~*/*SF:3 Empty Player Data~~~~~~*/*PF:4 Empty Player Data~~~~~~*/*C:5 Empty Player Data~~~~~~*/*F:6 Empty Player Data~~~~~~*/*F:7 Empty Player Data~~~~~~*/*UTIL:8 Empty Player Data~~~~~~"; break;
case "MMA": lineup.SingleLineup = "Empty Fighter Data~~~~~~*/*Empty Fighter Data~~~~~~*/*Empty Fighter Data~~~~~~*/*Empty Fighter Data~~~~~~*/*Empty Fighter Data~~~~~~"; break;
case "CBB": lineup.SingleLineup = "G:Empty Player Data~~~~~~*/*G:Empty Player Data~~~~~~*/*G:Empty Player Data~~~~~~*/*F:Empty Player Data~~~~~~*/*F:Empty Player Data~~~~~~*/*F:Empty Player Data~~~~~~*/*UTIL:Empty Player Data~~~~~~*/*UTIL:Empty Player Data~~~~~~"; break;
case "NHL": lineup.SingleLineup = "C:Empty Player Data~~~~~~*/*C:Empty Player Data~~~~~~*/*W:Empty Player Data~~~~~~*/*W:Empty Player Data~~~~~~*/*W:Empty Player Data~~~~~~*/*D:Empty Player Data~~~~~~*/*D:Empty Player Data~~~~~~*/*G:Empty Player Data~~~~~~*/*UTIL:Empty Player Data~~~~~~"; break;
case "NAS": lineup.SingleLineup = "Empty Driver Data~~~~~~*/*Empty Driver Data~~~~~~*/*Empty Driver Data~~~~~~*/*Empty Driver Data~~~~~~*/*Empty Driver Data~~~~~~*/*Empty Driver Data~~~~~~*/*"; break;
case "LOL": lineup.SingleLineup = "Empty Player Data~~~~~~*/*Empty Player Data~~~~~~*/*Empty Player Data~~~~~~*/*Empty Player Data~~~~~~*/*Empty Player Data~~~~~~*/*Empty Player Data~~~~~~*/*"; break;
case "SOC": lineup.SingleLineup = "GK:Empty Player Data~~~~~~*/*D:Empty Player Data~~~~~~*/*D:Empty Player Data~~~~~~*/*D:Empty Player Data~~~~~~*/*M:Empty Player Data~~~~~~*/*M:Empty Player Data~~~~~~*/*M:Empty Player Data~~~~~~*/*F:Empty Player Data~~~~~~*/*F:Empty Player Data~~~~~~*/*UTIL:Empty Player Data~~~~~~*/*UTIL:Empty Player Data~~~~~~"; break;
case "NFL": lineup.SingleLineup = "QB:Empty Player Data~~~~~~*/*RB:Empty Player Data~~~~~~*/*RB:Empty Player Data~~~~~~*/*WR:Empty Player Data~~~~~~*/*WR:Empty Player Data~~~~~~*/*WR:Empty Player Data~~~~~~*/*TE:Empty Player Data~~~~~~*/*FLEX:Empty Player Data~~~~~~*/*DST:Empty Player Data~~~~~~*/*"; break;
case "MLB": lineup.SingleLineup = "P:Empty Player Data~~~~~~*/*P:Empty Player Data~~~~~~*/*C:Empty Player Data~~~~~~*/*1B:Empty Player Data~~~~~~*/*2B:Empty Player Data~~~~~~*/*3B:Empty Player Data~~~~~~*/*OF:Empty Player Data~~~~~~*/*OF:Empty Player Data~~~~~~*/*OF:Empty Player Data~~~~~~*/*"; break;
case "PGA": lineup.SingleLineup = "Empty Golfer Data~~~~~~*/*Empty Golfer Data~~~~~~*/*Empty Golfer Data~~~~~~*/*Empty Golfer Data~~~~~~*/*Empty Golfer Data~~~~~~*/*Empty Golfer Data~~~~~~"; break;
case "CFB": lineup.SingleLineup = "QB:Empty Player Data~~~~~~*/*QB:Empty Player Data~~~~~~*/*RB:Empty Player Data~~~~~~*/*RB:Empty Player Data~~~~~~*/*WR:Empty Player Data~~~~~~*/*WR:Empty Player Data~~~~~~*/*WR:Empty Player Data~~~~~~*/*FLEX:Empty Player Data~~~~~~*/*FLEX:Empty Player Data~~~~~~*/*"; break;
default   : lineup.SingleLineup = "Empty Player Data~~~~~~~~~~~*/*Empty Player Data~~~~~~*/*Empty Player Data~~~~~~*/*Empty Player Data~~~~~~*/*Empty Player Data~~~~~~*/*Empty Player Data~~~~~~*/*Empty Player Data~~~~~~*/*Empty Player Data~~~~~~*/*"; break;
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
            List<string> SingleLineupLines = lineup.SingleLineup.Split(new string[] { "*/*" }, StringSplitOptions.None).ToList();
            string[] titles = stringtitlelist.Split(',');

            List<string> titleList = new List<string>();
            foreach (string title in titles)
            {
                string newtitle = title.Replace("\"", "");
                titleList.Add(newtitle);
            }
            List<string[]> allPlayersArrays = new List<string[]>();
            foreach (string player in SingleLineupLines)
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
                if (endformat.Count() <=2)
                {
                    //do not add
                }
                else
                {
                    allPlayersArrays.Add(endformat);
                }
            }
            int countlines = allPlayersArrays.Count;
            int titletotal = titleList.Count;
            ViewBag.Titles = titleList;
            ViewBag.Total = countlines;
            ViewBag.TitleTotal = titletotal;
            ViewBag.AllLines = SingleLineupLines;
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
            int totalSalary = 0;
            for (int i = 0; i < allPlayersArrays.Count(); i++)
            {
                string[] current = allPlayersArrays[i];
                int nextsalary;
                bool result = Int32.TryParse(current[2], out nextsalary);
                if (result)
                {
                    totalSalary += nextsalary;
                }
                else
                {
                    //do nothing
                }
            }
            string YouAreBroke = "false";
            string stringmoney = totalSalary.ToString("C0");
            int remainder = 50000 - totalSalary;
            string remainingSalary = (remainder).ToString("C0");
            ViewBag.TotalSalary = stringmoney;
            ViewBag.RemainingSalary = remainingSalary;
            if (remainder <= -1)
            {
                YouAreBroke = "true";
            }
            else
            {
                YouAreBroke = "false";
            }
            ViewBag.YouAreBrokeRed = YouAreBroke;
            decimal totalfantasypoints = GetAllFantasyPoints(id);
            ViewBag.TotalFantasyPoints = totalfantasypoints;
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

        public ActionResult RemovePlayer(int? id, string playerinput, int position)
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
                if (currentlineup[i].Length <= 3)
                {
                    //do not add to string
                }
                else if (i == position)
                {
                    singlelineup += "Empty Player Data~~~~~~*/*";
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

        public decimal GetAllFantasyPoints(int? lineupid)
        {
            GetPlayerList();
            decimal total = 0;
            Lineup lineup = db.Lineups.Find(lineupid);
            List<string> currentlineup = lineup.SingleLineup.Split(new string[] { "*/*" }, StringSplitOptions.None).ToList();
            CSVFiles thisfile = db.CSVFiles.Find(Convert.ToInt32(lineup.FileConnection));
            string date = thisfile.GameDate;
            foreach (string player in currentlineup)
            {
                if (player.Contains("empty") || player.Contains("Empty") || player.Length <= 3)
                {
                    //don't do stuff
                }
                else
                {
                    List<string> thisplayer = player.Split('~').ToList();
                    string playername = thisplayer[1];
                    string[] fullname = thisplayer[1].Split(' ');
                    string playerid = SearchPlayer(fullname[0], fullname[1]);
                    total += PlayerProjection(date, playerid);
                }
            }
            return total;
        }

        public decimal PlayerProjection(string date, string playerid)
        {
            var client = new HttpClient();
            decimal fantasypoints = 0;
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", PrimaryKey);
            string baseurl = "https://api.fantasydata.net/nba/v2/JSON/PlayerGameProjectionStatsByPlayer/" + date + "/" + playerid;
            var uri = baseurl;

            var response = client.GetAsync(uri);

            var word = response.Result;

            HttpContent requestContent = word.Content;
            string jsonContent = requestContent.ReadAsStringAsync().Result;
            string currentJson = jsonContent;
            
            List<string> playerDetailList = currentJson.Split(',').ToList();
            foreach (string detail in playerDetailList)
            {
                if (detail.Contains("FantasyPointsDraftKings"))
                {
                    string[] fantasy = detail.Split(':');
                    fantasypoints = Convert.ToDecimal(fantasy[1]);
                }
            }
            return fantasypoints;
        }

        public void GetPlayerList()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", PrimaryKey);

            var uri = "https://api.fantasydata.net/nba/v2/JSON/Players";

            var response = client.GetAsync(uri);

            var word = response.Result;

            HttpContent requestContent = word.Content;
            string jsonContent = requestContent.ReadAsStringAsync().Result;
            PlayerList = jsonContent;
        }

        public string SearchPlayer(string firstname, string lastname)
        {
            List<string> splitList = PlayerList.Split('{').ToList();
            string playerid = "";
            string CurrentPlayer = "";
            foreach (string player in splitList)
            {
                if (player.Contains(firstname) && player.Contains(lastname))
                {
                    CurrentPlayer = player;
                }
            }

            List<string> playerDetailList = CurrentPlayer.Split(',').ToList();
            foreach(string detail in playerDetailList)
            {
                if (detail.Contains("\"PlayerID\""))
                {
                    string[] playeridstring = detail.Split(':');
                    playerid = playeridstring[1];
                }
            }
            return playerid;
        }
    }
}
