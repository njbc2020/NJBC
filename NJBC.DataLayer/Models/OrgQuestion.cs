using System;
using System.Collections.Generic;

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
        public int? UserId { get; set; }

        public User User { get; set; }
        public ICollection<RelQuestion> RelQuestion { get; set; }
    }
}
