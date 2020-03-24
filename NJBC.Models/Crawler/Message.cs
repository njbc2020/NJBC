using System;
using System.Collections.Generic;
using System.Text;

namespace NJBC.Models.Crawler
{
    public class Message
    {
        public int MessageId { get; set; }
        public int? ReplayId { get; set; }
        public string Text { get; set; }
        public int Like { get; set; }
        public string Name { get; set; }
        public int PostCount { get; set; }
        public string Date { get; set; }
    }
}
