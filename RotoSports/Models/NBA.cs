using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RotoSports.Models
{
    public class NBA
    {
        public int ID { get; set; }
        public string Position { get; set; }
        public string Name { get; set; }
        public string Salary { get; set; }
        public string GameInfo { get; set; }
        public string AvgPointsPerGame { get; set; }
        public string teamAbbrev { get; set; }
    }
}