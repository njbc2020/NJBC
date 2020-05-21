using System;
using System.Collections.Generic;
using System.Text;

namespace NJBC.Models.DTO.Web
{
    public class QuestionEditParam
    {
        public long QuestionId { get; set; }
        public string QBody { get; set; }
        public string QSubject { get; set; }
        public string QBodyClean { get; set; }
        public string QSubjectClean { get; set; }
    }
}
