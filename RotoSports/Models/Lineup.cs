using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RotoSports.Models
{
    public class Lineup
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Sport { get; set; }
        public string Details { get; set; }
        public string SingleLineup { get; set; }
        public string PlayerList { get; set; }
    }
}