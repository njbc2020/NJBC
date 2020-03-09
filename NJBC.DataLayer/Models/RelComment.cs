using System;
using System.Collections.Generic;

namespace NJBC.DataLayer.Models
{
    public partial class RelComment
    {
        public int? RelqId { get; set; }
        public int RelcId { get; set; }
        public string RelcIdName { get; set; }
        public DateTime? RelcDate { get; set; }
        public string RelcUserid { get; set; }
        public string RelcUsername { get; set; }
        public string RelcRelevance2orgq { get; set; }
        public string RelcRelevance2relq { get; set; }
        public string RelCtext { get; set; }

        public RelQuestion Relq { get; set; }
    }
}
