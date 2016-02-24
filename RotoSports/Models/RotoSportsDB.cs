using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RotoSports.Models
{
    public class RotoSportsDB : DbContext
    {
        public RotoSportsDB() : base("DefaultConnection")
        {

        }

        public System.Data.Entity.DbSet<RotoSports.Models.CSVFiles> CSVFiles { get; set; }

        public System.Data.Entity.DbSet<RotoSports.Models.NBA> NBAs { get; set; }
    }
}

/*I need tables for 
*/