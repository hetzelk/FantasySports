using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RotoSports.Models
{
    public class CSVFiles
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Sport { get; set; }
        public string Details { get; set; }
        public string File { get; set; }
        public string BaseTitleList { get; set; }
        public string GameDate { get; set; }
    }
}