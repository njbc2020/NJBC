using System;
using System.Collections.Generic;
using System.Text;

namespace NJBC.Models.DTO.Web
{
    public class SetLabelCommentParam
    {
        public long CommentId { get; set; }
        public int UserId { get; set; }
        public string Label { get; set; }
    }
}
