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
    public class PredictionController : Controller
    {
        private RotoSportsDB db;
        List<string[]> allPlayers;
        List<string[]> predictedLineup;
        List<string> titlesList;
        string sport;
        Random random;

        public PredictionController()
        {
            db = new RotoSportsDB();
            allPlayers = new List<string[]>();
            predictedLineup = new List<string[]>();
            titlesList = new List<string>();
            sport = "";
            random = new Random();
        }

        // GET: Prediction
        public ActionResult Index()
        {
            var UserID = User.Identity.GetUserId();
            List<CSVFiles> UserFiles = db.CSVFiles.Where(x => x.UserId == UserID).ToList();
            Dictionary<string, string> UserFileDictionary = new Dictionary<string, string>();
            foreach (CSVFiles userFile in UserFiles)
            {
                UserFileDictionary.Add(userFile.ID.ToString(), userFile.Title);
            }
            ViewBag.UserFiles = UserFileDictionary;
            return View();
        }

        public ActionResult Team(string id)
        {
            var UserID = User.Identity.GetUserId();
            int fileid;
            bool result = Int32.TryParse(id, out fileid);
            if (result)
            {
                CSVFiles thisfile = db.CSVFiles.Find(fileid);
                sport = thisfile.Sport;
                if(thisfile == null)
                {
                    return RedirectToAction("Index", "Prediction", null);
                }
                if(thisfile.UserId != UserID)
                {
                    return RedirectToAction("Index", "Prediction", null);
                }
                ViewBag.FileTitle = thisfile.Title;
                ViewBag.Sport = thisfile.Sport;
                GetLineup(thisfile);
                ViewBag.Titles = titlesList;
                ViewBag.Prediction = predictedLineup;
                int countlines = predictedLineup.Count;
                int titletotal = titlesList.Count;
                ViewBag.Total = countlines;
                ViewBag.TitleTotal = titletotal;
                ViewBag.SalaryCap = GetTotalSalary();

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Prediction", null);
            }
        }

        public void GetLineup(CSVFiles file)
        {
            string allplayers = file.File;
            List<string> allLines = allplayers.Split(new string[] { "*/*" }, StringSplitOptions.None).ToList();
            string basetitles = allLines[0];
            string[] titles = allLines[0].Split(',');
            foreach (string title in titles)
            {
                string newtitle = title.Replace("\"", "");
                titlesList.Add(newtitle);
            }
            allLines.Remove(allLines[0]);
            List<string[]> allPlayersArrays = new List<string[]>();
            foreach (string player in allLines)
            {
                if (player == "" || player == null || player == " ")
                {
                    //do nothing
                }
                else
                {
                    string[] details = player.Split(',');
                    List<string> formatted = new List<string>();
                    foreach (string item in details)
                    {
                        string newitem = item.Replace("\"", "");
                        formatted.Add(newitem);
                    }
                    string[] endformat = formatted.ToArray();
                    allPlayersArrays.Add(endformat);
                }
                
            }
            allPlayers = allPlayersArrays;
            string[] highest = GetHighestAvgPoint();
            string[] lowest = GetLowPayHighAvg();
            predictedLineup.Add(highest);
            predictedLineup.Add(lowest);
            int reqpos = 0;
            switch (sport)
            {
                case "NBA": reqpos = 8; break;
                case "MMA": reqpos = 5; break;
                case "CBB": reqpos = 8; break;
                case "NHL": reqpos = 9; break;
                case "NAS": reqpos = 5; break;
                case "LOL": reqpos = 6; break;
                case "SOC": reqpos = 11; break;
                case "NFL": reqpos = 9; break;
                case "MLB": reqpos = 9; break;
                case "PGA": reqpos = 6; break;
                case "CFB": reqpos = 9; break;
            }
            for (int i = predictedLineup.Count(); i < reqpos; i++)
            {
                string[] random = GetRandom();
                predictedLineup.Add(random);
            }
        }

        public string[] GetHighestAvgPoint()
        {
            string[] highestplayer = { };
            decimal highestAvg = 0;
            foreach (string[] player in allPlayers)
            {
                decimal avg = Convert.ToDecimal(player[4]);
                if (avg > highestAvg)
                {
                    highestAvg = avg;
                    highestplayer = player;
                }
                else
                {
                    //onto the next one
                }
            }
            return highestplayer;
        }

        public string[] GetLowPayHighAvg()
        {
            string[] lowestplayer = { };
            int lowestSalary = GetLowestPay();
            decimal highestavg = 0;
            foreach (string[] player in allPlayers)
            {
                int salary = Convert.ToInt32(player[2]);
                decimal avg = Convert.ToDecimal(player[4]);
                if (avg > highestavg && lowestSalary == salary)
                {
                    highestavg = avg;
                    lowestplayer = player;
                }
                else
                {
                    //onto the next one
                }
            }
            return lowestplayer;
        }

        public int GetLowestPay()
        {
            int lowest = 100000;
            foreach (string[] player in allPlayers)
            {
                int salary = Convert.ToInt32(player[2]);
                if (salary < lowest)
                {
                    lowest = salary;
                }
            }
            return lowest;
        }

        public string[] GetRandom()
        {
            string[] randomplayer = { };
            Random randint = new Random();
            int randomnumber = random.Next(0, allPlayers.Count());
            int i = 0;
            foreach (string[] player in allPlayers)
            {
                if (randomnumber == i)
                {
                    randomplayer = player;
                }
                else
                {
                    //onto the next one
                }
                i++;
            }
            return randomplayer;
        }

        public int GetTotalSalary()
        {
            int totalSalary = 0;
            for (int i = 0; i < predictedLineup.Count(); i++)
            {
                string[] current = predictedLineup[i];
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
            return totalSalary;
        }
    }
}
