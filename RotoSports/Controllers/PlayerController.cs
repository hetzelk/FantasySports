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

namespace RotoSports.Controllers
{
    public class PlayerController : Controller
    {
        string PrimaryKey = "b0c04ca3bc254fa583b272a5ba86977e";
        string SecondaryKey = "7ad46477502a4416be9c19e420656304";

        public ActionResult Index()
        {
            ViewBag.PlayerInfo = GetPlayerList();
            return View();
        }

        public string GetPlayerList()
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
            return jsonContent;
        }
    }
}

/*

Primary key 

Secondary key 
 

    */