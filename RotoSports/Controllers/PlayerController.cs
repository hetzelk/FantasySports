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

namespace RotoSports.Controllers
{
    public class PlayerController : Controller
    {
        private OAuthBase YQL;
        public PlayerController()
        {
            YQL = new OAuthBase();
        }

        string consumerKey = "dj0yJmk9aDFNMFpRNVBCVEgwJmQ9WVdrOVkyNU5OekJWTnpZbWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD0zMw--";
        string consumerSecret = "27f3e738f3c59313da428d063e7ce8eb9a25a67b";
        // GET: Player
        public ActionResult Index()
        {
            
            ViewBag.YahooString = try2();
            return View();
        }

        protected void try1()
        {
            string returnUrl = "http://localhost:19785/Player";
            
            string url = "https://api.login.yahoo.com/oauth2/request_auth?client_id=" + consumerKey + "&redirect_uri=" + returnUrl + "&response_type=code&language=en-us";
            Response.Redirect(url);
        }

        protected string try2()
        {
            string returnUrl = "http://localhost:19785/Player";

            /*Sending User To Authorize Access Page*/
            string url = "https://api.login.yahoo.com/oauth2/request_auth?client_id=" + consumerKey + "&redirect_uri=" + returnUrl + "&response_type=code&language=en-us";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(@"" + url);
            string resultString = "";
            using (StreamReader read = new StreamReader(request.GetResponse().GetResponseStream(), true))
            {
                resultString = read.ReadToEnd();
            }
            return resultString;
        }

        public string GetAccessToken()
        {

            string returnUrl = "http://www.yogihosting.com/TutorialCode/YahooOAuth2.0/yahoooauth2.aspx";

            /*Exchange authorization code for Access Token by sending Post Request*/
            Uri address = new Uri("https://api.login.yahoo.com/oauth2/get_token");

            // Create the web request  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Set type to POST  
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] headerByte = System.Text.Encoding.UTF8.GetBytes(consumerKey + ":" + consumerSecret);
            string headerString = System.Convert.ToBase64String(headerByte);
            request.Headers["Authorization"] = "Basic " + headerString;

            // Create the data we want to send  
            StringBuilder data = new StringBuilder();
            data.Append("client_id=" + consumerKey);
            data.Append("&client_secret=" + consumerSecret);
            data.Append("&redirect_uri=" + returnUrl);
            data.Append("&code=" + Request.QueryString["code"]);
            data.Append("&grant_type=authorization_code");

            // Create a byte array of the data we want to send  
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

            // Set the content length in the request headers  
            request.ContentLength = byteData.Length;

            // Write data  
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            // Get response  
            string responseFromServer = "";
            string result = "";
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    // Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    responseFromServer = reader.ReadToEnd();
                    result = ShowReceivedData(responseFromServer);
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public string ShowReceivedData(string responseFromServer)
        {
            responseFromServer = responseFromServer.Substring(1, responseFromServer.Length - 2);
            string accessToken = "", xoauthYahooGuid = "", refreshToken = "", tokenType = "", expiresIn = "";
            string[] splitByComma = responseFromServer.Split(',');
            foreach (string value in splitByComma)
            {
                if (value.Contains("access_token"))
                {
                    string[] accessTokenSplitByColon = value.Split(':');
                    accessToken = accessTokenSplitByColon[1].Replace('"'.ToString(), "");
                }
                else if (value.Contains("xoauth_yahoo_guid"))
                {
                    string[] xoauthYahooGuidSplitByColon = value.Split(':');
                    xoauthYahooGuid = xoauthYahooGuidSplitByColon[1].Replace('"'.ToString(), "");
                }
                else if (value.Contains("refresh_token"))
                {
                    string[] refreshTokenSplitByColon = value.Split(':');
                    refreshToken = refreshTokenSplitByColon[1].Replace('"'.ToString(), "");
                }
                else if (value.Contains("token_type"))
                {
                    string[] tokenTypeSplitByColon = value.Split(':');
                    tokenType = tokenTypeSplitByColon[1].Replace('"'.ToString(), "");
                }
                else if (value.Contains("expires_in"))
                {
                    string[] expiresInSplitByColon = value.Split(':');
                    expiresIn = expiresInSplitByColon[1].Replace('"'.ToString(), "");
                }
            }
            string result = "Access Token:- <b>" + accessToken + "</b><br/><br/> Refresh Token:- <b>" + refreshToken + "</b><br/><br/> XOauth Yahoo Guid:- <b>" + xoauthYahooGuid + "</b><br/><br/> Token Type:- <b>" + tokenType + "</b><br/><br/> Expires In:- <b>" + expiresIn + "</b>";
            return result;
        }
    }
}