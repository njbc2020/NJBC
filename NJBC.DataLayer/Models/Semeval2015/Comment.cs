using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NJBC.DataLayer.Models.Semeval2015
{
    [Serializable]
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long CommentId { get; set; }
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

        //[ForeignKey("ReplayComment")]
        public long? ReplayCommentId { get; set; }
        //public virtual Comment ReplayComment { get; set; }

        [ForeignKey("Question")]
        public long QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
