using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NJBC.DataLayer.Models
{
    public partial class RelComment
    {
        [ForeignKey("RelQuestion")]
        public int RelqId { get; set; }
        public int RelcId { get; set; }
        public string RelcIdName { get; set; }
        public DateTime? RelcDate { get; set; }
        public string RelcUserid { get; set; }
        public string RelcUsername { get; set; }
        public string RelcRelevance2orgq { get; set; }
        public string RelcRelevance2relq { get; set; }
        public string RelCtext { get; set; }
        public string RelCtextClean { get; set; }
        public virtual RelQuestion Relq { get; set; }
    }
}
