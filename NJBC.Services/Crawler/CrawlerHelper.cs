using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using NJBC.Models.Crawler;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Globalization;

namespace NJBC.Services.Crawler
{
    public class CrawlerHelper
    {
        string csvHeader = "MessageId,TopicId,Message,Description,Replay,Username,Like,PostCount\n";

        #region 01 Start Method - Ninisite Crawling
        public void StartCrawling()
        {
            List<TopicLink> links = new List<TopicLink>();
            links = GetTopics(new List<int> { 116 }); // گرفتن لیست تاپیک ها

            Console.WriteLine("Topic Number: " + links.Count);
            foreach (var link in links)
            {
                try
                {
                    Console.Write($"| {link.TopicId} | ");
                    Topic topic = new Topic();
                    List<Message> msgs = new List<Message>();
                    var url = $"https://www.ninisite.com/discussion/topic/{link.TopicId}";
                    string description = "";

                    var web = new HtmlWeb(); // گرفتن اطلاعات از صفحه وب
                    var doc = web.Load(url);

                    int maxId = 1;
                    // بعضی از تاپیک ها در حین کرال تویط ادمین پاک میشوند
                    bool _html = doc.DocumentNode.InnerText.Contains("این تاپیک تعطیل شده است");
                    if (_html)
                    {
                        continue; // خروج از حلقه
                    }
                    _html = doc.DocumentNode.InnerText.Contains("صفحه ای پیدا نشد");
                    if (_html)
                    {
                        continue;
                    }

                    //string queryQuestion = "//div[@class='post-message topic-post__message col-xs-12 fr-view m-b-1 p-x-1']";
                    //var questionDiv = doc.DocumentNode.SelectNodes(queryQuestion);
                    //if (questionDiv != null && questionDiv.Any())
                    //{
                    //    var parags = questionDiv.FirstOrDefault().SelectNodes("//p");
                    //}

                    if (doc.GetElementbyId("topic").HasChildNodes) // شرط این است که آیا المان فوق دارای المان های زیر شاخه است؟
                    {
                        var s = doc.GetElementbyId("topic").SelectSingleNode(
                            "//div[@class='col-xs-12 col-sm-12 col-md-8 col-lg-8 col-xl-9 p-x-0  topic-post__body p-t-0 direction-rtl nini-medium']" +
                            "//div[@class='col-xs-12 p-x-0 postbody']" +
                            "//div[@class='post-toggle']" +
                            "//div[@class='post-message topic-post__message col-xs-12 fr-view m-b-1 p-x-1']"
                           ).ChildNodes;
                        if (s != null)
                        {
                            // در موقع کرال مشاهده شد که بعضی از توضیحات بیش از یک پاراگراف است، برای همین تمامی آنها را در یک خط قرار میدهیم
                            // بعضی از کاراکترها را هم جایگزین میکنیم که در موقع ذخیره سازی مشکل نداشته باشیم
                            // البته این عملیات در موقع توکنایز کردن هم کاربرد دارد و نوعی پیش پردازش بحساب می آید
                            string[] _tempStrArray = s.Select(x => x.InnerText.Trim().Replace("\r\n", "").Replace("\t", " . ").Replace(",", " . ").Replace("&nbsp;", " ").Replace("\n", " . ")).ToList().Where(x => !string.IsNullOrEmpty(x)).ToArray();
                            description = string.Join(". ", _tempStrArray).Replace("  ", " ").Replace(".. ", ". ").Replace(".. ", ". ").Replace(".. ", ". ");
                        }

                        var createDate = doc.GetElementbyId("topic").SelectSingleNode(
                            "//div[@class='col-xs-12 col-sm-12 col-md-8 col-lg-8 col-xl-9 p-x-0  topic-post__body p-t-0 direction-rtl nini-medium']" +
                            "//div[@class='col-xs-12 pull-xs-right p-x-0']" +
                            "//div[@class='col-xs-6 created-post text-xs-left p-x-1 m-t-0']" +
                            "//div[@class='d-inline-block']"+
                            "//span[@class='date']"
                           );
                        var createTime = doc.GetElementbyId("topic").SelectSingleNode(
                            "//div[@class='col-xs-12 col-sm-12 col-md-8 col-lg-8 col-xl-9 p-x-0  topic-post__body p-t-0 direction-rtl nini-medium']" +
                            "//div[@class='col-xs-12 pull-xs-right p-x-0']" +
                            "//div[@class='col-xs-6 created-post text-xs-left p-x-1 m-t-0']" +
                            "//div[@class='d-inline-block']" +
                            "//span[@class='time']"
                           );
                        if (createDate != null)
                        {
                            topic.CreateDate = createDate.InnerText;
                            try
                            {
                                if (!string.IsNullOrEmpty(topic.CreateDate) && topic.CreateDate.Contains("/"))
                                {
                                    int[] _persianDate = topic.CreateDate.Split('/').Select(x => Convert.ToInt32(x)).ToArray();
                                    int[] _persianTime = createTime.InnerText.Trim().Split(':').Select(x => Convert.ToInt32(x)).ToArray();
                                    if (_persianDate.Length == 3 && _persianTime.Length ==2)
                                    {
                                        PersianCalendar pc = new PersianCalendar();
                                        DateTime dt = new DateTime(_persianDate[0], _persianDate[1], _persianDate[2], _persianTime[0], _persianTime[1], 0, pc);
                                        topic.CreateDatetime = dt;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    // بدست آمدن عنوان سوال
                    var _tempQuestion = doc.DocumentNode.Descendants("h1").Where(x => x.Attributes.Contains("itemprop")).FirstOrDefault();
                    string question = _tempQuestion.Descendants("a").FirstOrDefault().InnerText.Trim();

                    // گرفتن المان های مربوط به صفحه بندی
                    var pagingExists = doc.GetElementbyId("post-grid").SelectNodes(
                        "//div[@class='row m-b-1']" +
                        "//div[@class='text-xs-center text-sm-left']" +
                        "//ul//li");

                    if (pagingExists == null) /// آیا صفحه بندی خالی است؟
                    {
                        // اگر خالی باشد، یعنی تاپیک یک صفحه بیشتر ندارد
                        // و ما با شناسه تاپیک عدد یک را ارسال میکنیم برای کرال کردن پاسخ ها
                        msgs.AddRange(getAnswersFromTopicId(link.TopicId, 1));
                    }
                    else
                    {
                        // اگر صفحه بندی وجود داشت مقدار عنصر نهایی را بدست میاوریم و آنرا به همراه شناسه تاپیک برای کرال ارسال میکنیم
                        // که تمامی صفحات تاپیک که بهمراه پاسخ های زیادی است دریافت شوند

                        var paging = doc.GetElementbyId("post-grid").SelectNodes("//div[@class='row m-b-1']" +
                                                        "//div[@class='text-xs-center text-sm-left']" +
                                                        "//ul//li").Select(x => x.InnerText).ToList();
                        foreach (var item in paging.Where(x => !string.IsNullOrEmpty(x)))
                        {
                            if (int.TryParse(item, out int ss))
                            {
                                maxId = ss > maxId ? ss : maxId;
                            }
                        }
                        for (int i = 1; i < maxId + 1; i++)
                        {
                            msgs.AddRange(getAnswersFromTopicId(link.TopicId, i));
                        }
                    }

                    topic.TopicId = link.TopicId;
                    topic.Messages = msgs;
                    topic.Question = question;
                    topic.Description = description;

                    
                    var jsonFile = JsonConvert.SerializeObject(topic);
                    MakeJson(jsonFile, link.ForumId, topic.TopicId);
                    // به دلیل وجود مقادیر تمیز نشده در برخی از فیلدها، امکان دخیره سازی با فرمت فوق ناممکن است

                    //string csv = "";
                    //string outCsvAll = "";
                    //csv = JsonToCSV(topic, out outCsvAll);
                    //string path = $@"C:\Nini\Forum\{link.ForumId}\";

                    //MakeCsv(csv, path, $"Topic_{topic.TopicId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"*******\nError -> ForumId: {link.ForumId}   Topicid: {link.TopicId}\n{ex.Message}\n");
                }
            }
        }
        #endregion

        #region 02 Convert Methods - در کرالر کاربردی ندارد
        // این دو متد ارتباطی با کرالر ندارد
        public void convertToCSV()
        {
            string pathDir = @"D:\nini\Forum\115\";
            string[] filePaths = Directory.GetFiles(pathDir, "*.json");

            string CsvAll = csvHeader;
            foreach (var path in filePaths)
            {
                int s = path.LastIndexOf(@"\") + 1;
                int e = path.Length - s;
                string fileName = path.Substring(s, e);
                string content = File.ReadAllText(path, Encoding.UTF8);
                var topic = JsonConvert.DeserializeObject<Topic>(content);

                string csv = "";
                var jsonFile = JsonConvert.SerializeObject(topic);
                csv = csvHeader;
                csv += ($"{topic.TopicId},{topic.TopicId},{topic.Question},-1,null,null,null" + "\n");
                foreach (var item in topic.Messages)
                {
                    string record = $"{item.MessageId}," +
                        $"{topic.TopicId}," +
                        $"{ item.Text.Replace("\n", ". ").Replace("\r", " ")}," +
                        (item.ReplayId.HasValue ? item.ReplayId.Value.ToString() : "null") + "," +
                        $"{ item.Name}," +
                        $"{ item.Like}," +
                        $"{ item.PostCount}\n";
                    csv += record;
                    CsvAll += record;
                }
                MakeCsv(csv, pathDir, fileName.Replace(".json", ""));
                Console.Write("8");
            }
        }
        public string JsonToCSV(Topic topic, out string csvAll)
        {
            string csv = "";
            csvAll = "";
            csv = csvHeader;
            csv += ($"{topic.TopicId},{topic.TopicId},{topic.Question},{topic.Description},-1,null,null,null" + "\n");
            foreach (var item in topic.Messages)
            {
                string record =
                    $"{item.MessageId}," +
                    $"{topic.TopicId}," +
                    $"{ item.Text.Replace("\n", ". ").Replace("\r", " ")}," +
                    $"null," +
                    (item.ReplayId.HasValue ? item.ReplayId.Value.ToString() : "null") + "," +
                    $"{ item.Name}," +
                    $"{ item.Like}," +
                    $"{ item.PostCount}\n";
                csv += record;
                //csvAll += record;
            }
            return csv;
        }
        #endregion

        #region Talar - Forums
        public List<ForumLink> getTalars()
        {
            List<ForumLink> forumLinks = new List<ForumLink>();
            string path = @"c:\Ninisite\ForumLinks.json";

            if (!File.Exists(path)) // اگر فایل لیست تالارها موجو نبود برو از طریق تایع زیر بگیر و ذخیره اش کن
            {
                forumLinks = getTalarLink(); // صدا زدن متد کرالر دریافت لینک های تالارها
                string createText = JsonConvert.SerializeObject(forumLinks); // تبدیل آبجکت لیست لینک تالار ها به فرمت جیسون
                File.WriteAllText(path, createText); // متد ذخیره کردن یک فایل متنی که در اینجا همان مقادیر جیسون خط بالا است
            }
            forumLinks = JsonConvert.DeserializeObject<List<ForumLink>>(File.ReadAllText(path)); // فایل جیسون مربوط به لینک های فروم و یا همان تالار را لود میکند و تبدیل به آبجکت میکند
            Console.WriteLine("\n" + forumLinks.Count + "\n");
            return forumLinks;
        }

        public List<ForumLink> getTalarLink()
        {
            var url = $"https://www.ninisite.com/discussion";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            string q1 =
                "//body//div[@class='discussion-section']//div[@class='adtube__content-wrapper']//div[@class='container forum-container text-xl-right']" +
                "//div[@class='row']" +
                "//div[@class='col-xl-9 col-lg-8 pull-xs-right m-b-1 ']";

            var section = doc.DocumentNode.SelectSingleNode(q1).InnerHtml;
            var doc1 = new HtmlDocument();
            doc1.LoadHtml(section);
            //var divs= doc1.DocumentNode.SelectNodes(q2);
            List<ForumLink> forumLinks = new List<ForumLink>();

            var childDivs = doc1.DocumentNode.SelectNodes("//a[@class='category--title']");
            for (int i = 0; i < childDivs.Count; i++)
            {
                ForumLink forumLink = new ForumLink();

                //talar.id = i;
                forumLink.Name = childDivs[i].InnerText.Replace("\r\n", "").Trim();
                forumLink.URL = childDivs[i].Attributes["href"].Value;

                forumLink.URL = forumLink.URL.Replace("/discussion/forum/", "");
                int iSlash = forumLink.URL.IndexOf("/");
                string fid = forumLink.URL.Substring(0, iSlash);
                forumLink.URL = $"https://www.ninisite.com/discussion/forum/{fid}/a";
                forumLink.ForumId = int.Parse(fid);
                forumLink.Count = 0;

                forumLinks.Add(forumLink);
            }
            return forumLinks;
        }
        #endregion

        #region Topics Methods
        public List<TopicLink> GetTopicLinks(string url)
        {
            url = url.Replace("https://www.ninisite.com/discussion/forum/", "");
            int iSlash = url.IndexOf("/");
            string fid = url.Substring(0, iSlash);
            int forumId = int.Parse(fid);
            url = $"https://www.ninisite.com/discussion/forum/{fid}/a";
            List<TopicLink> links = new List<TopicLink>();
            var web = new HtmlWeb();
            var doc = web.Load(url);

            // بدست آوردن عنصر انتهایی در صفحه بندی لیست تاپیک ها
            // برای شروع مقدار یک را قرار میدهیم که در صورت وجود نداشتن
            // صفحه بندی حداقل مقدار یک به صورت پییشفرض در نظر گرفته شود
            int max = 1;
            if (doc.DocumentNode.SelectNodes("//ul[@class='pagination']//li[@class='page-item']//a[@class='page-link']").Any())
            {
                var pagging = doc.DocumentNode.SelectNodes("//ul[@class='pagination']//li[@class='page-item']//a[@class='page-link']").ToList().Select(x => x.InnerText).ToArray();
                foreach (var item in pagging.Where(x => !string.IsNullOrEmpty(x)))
                {
                    if (int.TryParse(item, out int ss))
                    {
                        max = ss > max ? ss : max;
                    }
                }
            }

            // حلقه ای که در آن از ابتدا به تا عنصر نهایی بدست آمده سر میزنیم
            // و تمامی لینک های تاپیک ها را ذخیره میکنیم
            for (int i = 1; i < max + 1; i++)
            {
                string page = url + "?page=" + i;
                var web1 = new HtmlWeb();
                var doc1 = web1.Load(page);
                var topics = doc1.GetElementbyId("cat").SelectNodes(
                    "//div[@class='col-xs-12 category--header p-x-0']" +
                    "//div[@class='col-xs-12  pull-xs-right category--item']" +
                    "//div[@class='col-xs-12 col-md-6 pull-xs-right p-x-0']").Select(x => x.InnerHtml).ToList();

                foreach (var item in topics)
                {
                    try
                    {
                        var docTopic = new HtmlDocument();
                        docTopic.LoadHtml(item);
                        var lnk = docTopic.DocumentNode.SelectSingleNode("//a").Attributes["href"].Value;
                        var name = docTopic.DocumentNode.SelectSingleNode("//a//h2").InnerText.Trim();
                        TopicLink topicLink = new TopicLink();
                        lnk = lnk.Replace("/discussion/topic/", "");
                        int s = lnk.IndexOf("/");
                        if (s == -1 && int.TryParse(lnk, out int lnkId))
                        {
                            topicLink.TopicId = lnkId;
                        }
                        else
                        {
                            string topicId = lnk.Substring(0, s);
                            topicLink.TopicId = int.Parse(topicId);
                        }

                        topicLink.URL = "https://www.ninisite.com/discussion/topic/" + topicLink.TopicId + "/a";
                        topicLink.TopicName = name;
                        topicLink.ForumId = forumId;
                        links.Add(topicLink);
                    }
                    catch (Exception)
                    {
                        Console.Write("\n •  Error: " + i + "  •\n");
                    }
                }
                if (i % 20 == 0)
                {
                    Console.Write("." + i);
                }
                else
                {
                    Console.Write(".");
                }
                if (i == max)
                {
                    Console.Write("." + i + "\t end. \n");
                }
            }

            return links;
        }

        public List<TopicLink> GetTopics(List<int> talars) // ورودی لیستی از شماره های تالارهایی که میخواهیم تاپیک های آنها را دریافت کنیم
        {
            List<ForumLink> forumLinks = new List<ForumLink>();
            forumLinks = getTalars(); // تایعی جهت گرفتن لیست تالارها یا همان فروم ها
            if (talars != null && talars.Any()) // در صورت پر بودن پارمتر ورودی تابع فیلتر را انجام میدهیم ، در غیر این صورت تمامی تاییک ها از تمامی تالار ها را گرفته خواهد شد
            {
                forumLinks = forumLinks.Where(x => talars.Contains(x.ForumId)).ToList(); // فیلتر شدن تالار ها براساس ورودی تایع که دریافت کردیم
            }
            List<TopicLink> listTopics = new List<TopicLink>();

            foreach (var item in forumLinks) // حلقه در فروم ها جهت دریافت جمع آوری لینک تاپیک ها
            {
                string path = $@"c:\Ninisite\Topics\Forum_Topics_{item.ForumId}.json"; // ساخت لینک تاپیک
                List<TopicLink> _listTopics = new List<TopicLink>();
                if (!File.Exists(path)) // اگر فایل لیست تاپیک های یک تالار وجود نداشت لینک تمام تاپیک هارو بدست بیار و در یک فایل جیسون ذخیره کن
                {
                    Forum forum = new Forum();
                    _listTopics.Clear();
                    var links = GetTopicLinks(item.URL);
                    if (links.Count > 0)
                    {
                        _listTopics.AddRange(links);
                        forum.ForumId = item.ForumId;
                        forum.Count = _listTopics.Count();
                        forum.Name = item.Name;
                        forum.TopicLinks = _listTopics;
                        Console.Write('|');
                        string createText = JsonConvert.SerializeObject(forum, Formatting.Indented);
                        File.WriteAllText(path, createText);
                    }
                    else
                    {
                        Console.WriteLine("\nError: " + item.URL + " \n");
                        continue;
                    }
                }
                _listTopics = JsonConvert.DeserializeObject<Forum>(File.ReadAllText(path)).TopicLinks;
                listTopics.AddRange(_listTopics);
            }
            return listTopics;
        }

        public List<Message> getAnswersFromTopicId(int topicId, int id)
        {
            List<Message> msgs = new List<Message>();
            // ساختن لینک و بدست آوردن مقدار صفحه
            var url = "https://www.ninisite.com/discussion/topic/" + topicId + "/a?page=" + id;
            var web = new HtmlWeb();
            var doc = web.Load(url);


            var data = doc.GetElementbyId("posts").Descendants("article");

            foreach (var item in data)
            {
                Message msg1 = new Message();
                string _article = item.InnerHtml
                    .Replace(@"class=""post-message", @"id=""msgDiv"" class=""post-message")
                    .Replace(@"class=""reply-message""", @"id=""reply-message""")
                    .Replace(@"class=""col-xs-12 p-x-0 postfooter""", @"id=""postfooter""")
                    .Replace(@"class=""date""", @"id=""date""")
                    .Replace(@"class=""like-count fancy__text""", @"id=""likeCount""")
                    .Replace(@"class=""col-xs-9 col-md-12 text-md-center text-xs-right nickname""", @"id=""nickname""")
                    .Replace(@"class=""text-xs-right pull-sm-right pull-md-none text-md-center post-count""", @"id=""postCount""")
                    .Replace(@"تعداد پست:", " ");
                var doc1 = new HtmlDocument();
                doc1.LoadHtml(_article);
                msg1.MessageId = int.Parse(item.Attributes["id"].Value.Replace("post-", ""));
                var parags = doc1.GetElementbyId("msgDiv").SelectNodes("//p");
                var postDate = doc1.GetElementbyId("date").InnerHtml.Replace("\r\n", "").Replace("  ", " ").Trim();
                if (!string.IsNullOrEmpty(postDate))
                {
                    msg1.Date = postDate;
                }
                var postCount = doc1.GetElementbyId("postCount")
                                        .InnerHtml.Replace("\r\n", "")
                                        .Replace("<span>", "")
                                        .Replace("</span>", "")
                                        .Replace("  ", " ")
                                        .Trim();
                if (!string.IsNullOrEmpty(postCount) && int.TryParse(postCount, out int intNumber))
                {
                    msg1.PostCount = intNumber;
                }
                var isNickName = doc1.GetElementbyId("nickname");
                string nickName = string.Empty;
                if (isNickName != null)
                {
                    nickName = doc1.GetElementbyId("nickname").InnerHtml.Replace("\r\n", "").Replace("  ", " ").Trim();
                }

                if (!string.IsNullOrEmpty(nickName))
                {
                    msg1.Name = nickName;
                }
                var likeCount = doc1.GetElementbyId("likeCount").InnerHtml.Replace("\r\n", "")
                                                .Replace("نفر لایک کرده اند ...", "")
                                                .Replace("<span>", "")
                                                .Replace("</span>", "")
                                                .Replace("  ", " ").Trim();
                if (!string.IsNullOrEmpty(likeCount) && int.TryParse(likeCount, out int intNumberLike))
                {
                    msg1.Like = intNumberLike;
                }
                string _txt = "";
                if (parags != null)
                {
                    _txt = string.Join(". ", doc1.GetElementbyId("msgDiv").SelectNodes("//p").Select(x => x.InnerText));

                }
                else
                {
                    _txt = doc1.GetElementbyId("msgDiv").InnerText.ToString();
                }
                msg1.Text = _txt;
                _txt = _txt.Replace(@"	", ". ").Replace("&nbsp;", " ").Replace("  ", " ")
                    .Replace("  ", " ").Replace("\n\r", ". ").Replace("\r\n", ". ").Replace(". . ", ". ").Trim();
                if (string.IsNullOrEmpty(_txt))
                {
                    continue;
                }
                if (_txt.Substring(0, 1) == ".")
                {
                    _txt = _txt.Substring(1, _txt.Length - 1);
                }
                _txt = _txt.Trim();
                msg1.TextClean = _txt;
                if (doc1.GetElementbyId("reply-message") != null)
                {
                    if (int.TryParse(doc1.GetElementbyId("reply-message").Attributes["data-id"].Value, out int _replayId))
                    {
                        msg1.ReplayId = _replayId;
                    }
                }
                msgs.Add(msg1);
            }
            return msgs;
        }
        #endregion

        #region Save to File Methods
        public void MakeCsv(string text, string path, string fileName)
        {
            //string path = /*Environment.GetFolderPath(Environment.SpecialFolder.Desktop) +*/ @"C:\Nini\Forum\" + forumId + "\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string logFile = fileName + ".csv";
            if (!File.Exists(path + logFile))
            {
                FileStream f = File.Create(path + logFile);
                f.Close();
            }

            File.WriteAllText(path + logFile, text, Encoding.UTF8);

        }
        public void MakeJson(string text, int forumId, int topicId)
        {
            string path = @"C:\Nini\Forum\" + forumId + "\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string logFile = topicId + ".json";
            if (!File.Exists(path + logFile))
            {
                FileStream f = File.Create(path + logFile);
                f.Close();
            }

            using (StreamWriter sw = new StreamWriter(path + logFile, true))
            {
                sw.WriteLine(text);
                sw.Close();
            }
        }
        #endregion
    }
}
