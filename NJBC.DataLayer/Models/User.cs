using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.SqlServer.Design;
using NJBC.DataLayer.Models.Semeval2015;

namespace NJBC.DataLayer.Models
{
    public partial class User
    {
        public User()
        {
            OrgQuestions = new HashSet<OrgQuestion>();
            Questions = new HashSet<Question>();
        }

        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public DateTime LastDatetime { get; set; }

        public virtual ICollection<OrgQuestion> OrgQuestions { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
