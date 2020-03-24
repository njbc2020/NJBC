using System;
using System.Collections.Generic;
using System.Text;

namespace NJBC.Models.Crawler
{
    public class Forum
    {
        public int ForumId { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public List<TopicLink> TopicLinks { get; set; }
    }
}
