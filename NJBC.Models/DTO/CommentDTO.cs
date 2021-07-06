using System;
using System.Collections.Generic;
using System.Text;

namespace NJBC.Models.DTO.Comment
{
    public class CommentDTO
    {
        public string CID { get; set; }
        public long CUSERID { get; set; }
        public string CGOLD { get; set; }
        public string CGOLD_YN { get; set; }
        public string CSubject { get; set; }
        public string CBody { get; set; }
        public string CBodyClean { get; set; }
        public string CUsername { get; set; }
        public DateTime? CDate { get; set; }
        public DateTime? LabelDate { get; set; }
        public long? ReplayCommentId { get; set; }
    }
}
