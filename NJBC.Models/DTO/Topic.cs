using System;
using System.Collections.Generic;
using System.Text;

namespace NJBC.Models.DTO
{
    public class Topic
    {
        public string Question { get; set; }
        public string Description { get; set; }
        public int TopicId { get; set; }
        public List<Message> Messages { get; set; }
    }
}
