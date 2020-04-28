using NJBC.DataLayer.Models.Semeval2015;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBC.Web.App.Label.Models
{
    public class QuestionsVM
    {
        public List<Question> Questions { get; set; }
        public int Count { get; set; }
        public int Page { get; set; }
        public int Max { get; set; }
        public string Token { get; set; }
    }
}
