using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NJBC.DataLayer.Models
{
    public partial class OrgQuestion
    {
        public OrgQuestion()
        {
            RelQuestion = new HashSet<RelQuestion>();
        }

        public int OrgqId { get; set; }
        public string OrgqIdName { get; set; }
        public string OrgQsubject { get; set; }
        public string OrgQbody { get; set; }
        //[ForeignKey("User")]
        //public int? UserId { get; set; }

        //public virtual User User { get; set; }
        public virtual ICollection<RelQuestion> RelQuestion { get; set; }
    }
}
