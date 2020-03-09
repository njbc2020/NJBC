using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace NJBC.DataLayer.Context
{
    public class AppilcationDbContext : DbContext
    {

        public AppilcationDbContext()
        {

        }
        //private const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=EFCore;Trusted_Connection=True;";
        //public virtual DbSet<OrgQuestion> OrgQuestions { get; set; }
        //public virtual DbSet<RelComment> RelComments { get; set; }
        //public virtual DbSet<RelQuestion> RelQuestions { get; set; }
        //public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
