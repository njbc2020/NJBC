using System;
using System.Collections.Generic;
using System.Text;
using NJBC.Models.DTO.Comment;

namespace NJBC.Models.DTO.Question
{
    public class QuestionDTO
    {
        public string QID { get; set; }
        public string QCATEGORY { get; set; }
        public DateTime QDATE { get; set; }
        public long QUSERID { get; set; }
        public string QTYPE { get; set; }
        public string QGOLD_YN { get; set; }
        public string QUsername { get; set; }
        public string QBody { get; set; }
        public string QSubject { get; set; }
        public virtual List<CommentDTO> Comments { get; set; }
    }

}
