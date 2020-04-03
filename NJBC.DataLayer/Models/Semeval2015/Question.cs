using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NJBC.DataLayer.Models.Semeval2015
{
    public class Question
    {
        public Question()
        {
            Comments = new HashSet<Comment>();
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long QuestionId { get; set; }
        public string QID { get; set; }
        public string QCATEGORY { get; set; }
        public DateTime QDATE { get; set; }
        public long QUSERID { get; set; }
        public string QTYPE { get; set; }
        public string QGOLD_YN { get; set; }
        public string QUsername { get; set; }
        public string QBody { get; set; }
        public string QSubject { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
