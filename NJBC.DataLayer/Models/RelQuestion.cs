using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NJBC.DataLayer.Models
{
    public partial class RelQuestion
    {
        public RelQuestion()
        {
            RelComment = new HashSet<RelComment>();
        }

        [ForeignKey("OrgQuestion")]
        public int OrgqId { get; set; }
        public int RelqId { get; set; }
        public string RelqIdName { get; set; }
        public string RelqRankingOrder { get; set; }
        public string RelqCategory { get; set; }
        public DateTime? RelqDate { get; set; }
        public string RelqUserid { get; set; }
        public string RelqUsername { get; set; }
        public string RelqRelevance2orgq { get; set; }
        public string RelQsubject { get; set; }
        public string RelQbody { get; set; }

        public virtual OrgQuestion Orgq { get; set; }
        public virtual ICollection<RelComment> RelComment { get; set; }
    }
}
