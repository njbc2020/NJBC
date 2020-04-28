using NJBC.DataLayer.Models.Semeval2015;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBC.Web.App.Label.Models
{
    public class LabelVM
    {
        public int UserId { get; set; }
        public string ErrMsg { get; set; }
        public Question Q { get; set; }
    }
}
