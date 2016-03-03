using OAuth;
using RotoSports.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using RotoSports.Models;

namespace RotoSports.Controllers
{
    public class PlayerController : Controller
    {
        private RotoSportsDB db = new RotoSportsDB();
        private ApplicationDbContext context = new ApplicationDbContext();
        string PassedPlayer = "";
        string PrimaryKey = "b0c04ca3bc254fa583b272a5ba86977e";
        //string SecondaryKey = "7ad46477502a4416be9c19e420656304";
        string PlayerList = "empty";
        string PlayerProjections = "empty";
        string CurrentPlayer = "empty";
        string playername = "";
        List<string> splitList = new List<string>();

        public ActionResult Index(string player, string filepath)
        {
            CSVFiles thisCSVfile = db.CSVFiles.Find(Convert.ToInt32(filepath));
            string GameDate = thisCSVfile.GameDate;
            if (player == null || player == "")
            {
                return RedirectToAction("Index", "Home", null);
            }
            PassedPlayer = player;
            GetAllPlayerData();
            ViewBag.PlayerInfo = CurrentPlayer;
            List<string> splitPlayerInfo = CurrentPlayer.Split(',').ToList();
            List<string> finallist = new List<string>();
            List<string> injuryList = new List<string>();
            string playerid = splitPlayerInfo[0];
            foreach (string detail in splitPlayerInfo)
            {
                if (detail.Contains("Status") || detail.Contains("\"Team\"") || detail.Contains("Position") || detail.Contains("Height") || detail.Contains("Weight") || detail.Contains("Experience"))
                {
                    string formatted = detail.Replace("\"", "");
                    finallist.Add(formatted);
                }

            }
            foreach (string detail in splitPlayerInfo)
            {
                if (detail.Contains("InjuryStatus") || detail.Contains("InjuryBodyPart") || detail.Contains("InjuryStartDate") || detail.Contains("InjuryNotes"))
                {
                    string formatted = detail.Replace("\"", "");
                    if (formatted.Contains("Date"))
                    {
                        string[] date = formatted.Split(new string[] { "T00" }, StringSplitOptions.None);
                        injuryList.Add(date[0]);
                    }
                    else
                    {
                        formatted = detail.Replace("}", "");
                        injuryList.Add(formatted);
                    }
                }
            }
            
            ViewBag.Injury = "true";
            if (injuryList[0].Contains("null"))
            {
                ViewBag.Injury = "false";
            }
            ViewBag.PlayerInfoList = finallist;
            ViewBag.InjuryList = injuryList;
            ViewBag.PlayerName = playername;
            ViewBag.PlayerID = playerid;
            PlayerProjection(GameDate, playerid);
            List<string> ProjectionList = PlayerProjections.Split(',').ToList();
            ViewBag.GameProjection = ProjectionList;
            return View();
        }

        public void GetAllPlayerData()
        {
            GetPlayerList();
            List<string> thisplayer = PassedPlayer.Split('~').ToList();
            playername = thisplayer[1];
            string[] fullname = thisplayer[1].Split(' ');
            SearchPlayer(fullname[0], fullname[1]);
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

        public void SearchPlayer(string firstname, string lastname)
        {
            splitList = PlayerList.Split('{').ToList();
            foreach(string player in splitList)
            {
                if (player.Contains(firstname) && player.Contains(lastname))
                {
                    CurrentPlayer = player;
                }
            }

        }

        public void PlayerProjection(string date, string playerid)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", PrimaryKey);
            string[] newId = playerid.Split(':');
            string baseurl = "https://api.fantasydata.net/nba/v2/JSON/PlayerGameProjectionStatsByPlayer/" + date + "/" + newId[1];
            var uri = baseurl;

            var response = client.GetAsync(uri);

            var word = response.Result;

            HttpContent requestContent = word.Content;
            string jsonContent = requestContent.ReadAsStringAsync().Result;
            PlayerProjections = jsonContent;
        }
    }
}

/*

Primary key 

Secondary key 
 

    */