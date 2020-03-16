using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.SqlServer.Design;

namespace NJBC.DataLayer.Models
{
    public partial class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public DateTime LastDatetime { get; set; }

        public ICollection<OrgQuestion> OrgQuestions { get; set; }
    }
}
