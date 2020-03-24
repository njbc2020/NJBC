using System;
using System.Collections.Generic;

namespace NJBC.Models.Crawler
{
    public class Topic
    {
        public string Question { get; set; }
        public string Description { get; set; }
        public string DescriptionClean { get; set; }
        public int TopicId { get; set; }
        public string CreateDate { get; set; }
        public DateTime CreateDatetime { get; set; }
        public List<Message> Messages { get; set; }
    }
}
