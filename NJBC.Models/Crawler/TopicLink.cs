using System;
using System.Collections.Generic;
using System.Text;

namespace NJBC.Models.Crawler
{
    public class TopicLink
    {
        public int ForumId { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string URL { get; set; }
    }
}
