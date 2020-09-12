namespace NJBC.Models.Crawler
{
    public class Message
    {
        public int MessageId { get; set; }
        public int? ReplayId { get; set; }
        public string Text { get; set; }
        public string TextClean { get; set; }
        public int Like { get; set; }
        public string Name { get; set; }
        public int PostCount { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
