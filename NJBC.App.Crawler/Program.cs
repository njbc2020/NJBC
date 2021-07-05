using NJBC.Services.Crawler;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NJBC.DataLayer.Repository;
using NJBC.DataLayer.IRepository;
using System.Collections.Generic;
using Newtonsoft.Json;
using NJBC.Models.Crawler;
using System.IO;
using System.Linq;
using NJBC.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading;
using NJBC.DataLayer.Models.Semeval2015;
using NJBC.Common;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NJBC.App.Crawler
{
    static class Program
    {
        private static long qc = 0;
        private static long qc1 = 100000;
        private static long cc = 0;
        private static long uc = 0;
        static List<Question> questions = new List<Question>();
        static List<Comment> comments = new List<Comment>();
        static Dictionary<string, long> usersDic = new Dictionary<string, long>();
        static Dictionary<long, string> usersIdDic = new Dictionary<long, string>();
        //private static NJBC_DBContext _appDbContext;
        static void Main(string[] args)
        {
            CrawlerHelper crawler = new CrawlerHelper();
            string message = $"Hi\nStart {DateTime.Now}\nMethod List: \n";
            message += "1_ Get Messages From Ninisite\n";
            message += "2_ Convert Json to CSV\n";
            message += "3_ Import Compress File To SQL Database\n";
            message += "Enter Method Number:";
            Console.Write(message);
            int method = int.Parse(Console.ReadLine());
            switch (method)
            {
                case 1:
                    crawler.StartCrawling();  // متد اصلی
                    break;
                case 2:
                    crawler.convertToCSV();
                    break;
                case 3:
                    ImportJsonToDB();  // متد ایمپورت
                    break;
                case 4:
                    txt();
                    break;
                case 5:
                    m5RemoveDuplicate();
                    break;
                case 6:
                    m6test();
                    break;
                default:
                    break;
            }
            Console.WriteLine("Hello World!");
        }

        #region 03 Import Json To Database
        public static void ImportJsonToDB()
        {
            int[] forums = new int[] { 115, 116, 21 };
            int[] blockTopic = new int[] { 3298910, 1503527, 1662425, 3666349, 4432469, 2225778, 1449197 };

            List<long> commentIds = new List<long>();
            List<string> questionIds = new List<string>();

            ISemEvalRepository rep = new SemEvalRepository();
            List<string> filePaths = new List<string>();
            foreach (var forum in forums)
            {
                string path1 = Path.Combine(@"E:\", $"{forum}.zip");
                Stopwatch stopWatch = new Stopwatch();
                using (FileStream fs = new FileStream(path1, FileMode.Open))
                using (ZipArchive zip = new ZipArchive(fs))
                {
                    if (zip.Entries.Count > 0)
                    {
                        for (int i = 0; i < zip.Entries.Count; i++)
                        {
                            stopWatch.Reset();
                            stopWatch.Start();
                            using (StreamReader sr = new StreamReader(zip.Entries[i].Open()))
                            {
                                string readText = sr.ReadToEnd();
                                Topic topic = new Topic();
                                if (string.IsNullOrEmpty(readText))
                                    continue;
                                try
                                {
                                    topic = JsonConvert.DeserializeObject<Topic>(readText);
                                }
                                catch (Exception)
                                {
                                    continue;
                                }

                                if (blockTopic.Contains(topic.TopicId))
                                    continue;
                                if (!usersDic.ContainsKey(topic.NickName))
                                {
                                    long userid = uc++;
                                    usersDic.Add(topic.NickName, userid);
                                    usersIdDic.Add(userid, topic.NickName);
                                }

                                if (forum != 21)
                                {
                                    questionIds.Add(topic.TopicId.ToString());
                                }

                                Question q = new Question()
                                {
                                    QuestionId = forum == 21 ? (qc1++) : qc++,
                                    QID = topic.TopicId.ToString(),
                                    QSubject = topic.Question,
                                    QBody = topic.Description,
                                    QCATEGORY = forum.ToString(),
                                    QDATE = topic.CreateDatetime,
                                    QGOLD_YN = "Not Applicable",
                                    QTYPE = "General",
                                    QUsername = topic.NickName,
                                    QUSERID = usersDic[topic.NickName]
                                };

                                List<Comment> _comments = new List<Comment>();
                                using (var textHelper = new TextHelper())
                                {
                                    foreach (var message in topic.Messages)
                                    {
                                        if (message.MessageId == 46837536)
                                        {

                                        }
                                        if (string.IsNullOrEmpty(message.Name) || string.IsNullOrEmpty(message.Text))
                                            continue;
                                        if (message.MessageId == null)
                                            continue;
                                        if (!usersDic.ContainsKey(message.Name))
                                        {
                                            long userid = uc++;
                                            usersDic.Add(message.Name, userid);
                                            usersIdDic.Add(userid, message.Name);
                                        }
                                        if (forum == 21 && !questionIds.Contains(topic.TopicId.ToString()))
                                        {
                                            string cleanText = textHelper.CleanReview(message.TextClean);

                                            Comment cm = new Comment()
                                            {
                                                CommentId = message.MessageId,
                                                CID = message.MessageId.ToString(),
                                                CBody = message.Text,
                                                CBodyClean = cleanText,
                                                CUsername = message.Name,
                                                CUSERID = usersDic[message.Name],
                                                CGOLD = "",
                                                CGOLD_YN = "",
                                                CSubject = "",
                                                ReplayCommentId = message.ReplayId,
                                                QuestionId = q.QuestionId
                                            };
                                            int[] _persianDate = message.Date.Split('/').Select(x => Convert.ToInt32(x)).ToArray();

                                            if (_persianDate.Length == 3)
                                            {
                                                PersianCalendar pc = new PersianCalendar();
                                                DateTime dt = new DateTime();
                                                if (!string.IsNullOrEmpty(message.Time))
                                                {
                                                    int[] _persianTime = message.Time.Split(':').Select(x => Convert.ToInt32(x)).ToArray();
                                                    dt = new DateTime(_persianDate[0], _persianDate[1], _persianDate[2], _persianTime[0], _persianTime[1], 0, pc);
                                                }
                                                else
                                                    dt = new DateTime(_persianDate[0], _persianDate[1], _persianDate[2], 0, 0, 0, pc);
                                                cm.CDate = dt;
                                            }
                                            if (!textHelper.RemoveSingleEmoji(cm.CBodyClean))
                                            {
                                                _comments.Add(cm);
                                            }
                                        }
                                        else
                                        {
                                            commentIds.Add(message.MessageId);
                                        }
                                    }
                                }


                                if (forum == 21)
                                {
                                    var _cms = _comments.GroupBy(x => x.CommentId).Select(x => x.First()).ToList();
                                    List<Comment> _cms1 = _cms.GroupBy(x => x.CBodyClean).Select(x => x.First()).ToList();
                                    comments.AddRange(_cms1);
                                    questions.Add(q);
                                }

                                //if (comments.Count() > 10000)
                                //{
                                //    rep.AddQuestions(questions);
                                //    Thread.Sleep(2 * 1000);
                                //    var cms_1 = comments.GroupBy(x => x.CommentId).Select(x => x.First()).ToList();
                                //    rep.AddComments(cms_1);
                                //    questions.Clear();
                                //    comments.Clear();
                                //    _comments.Clear();
                                //    _cms.Clear();
                                //    Console.Write("|");
                                //}
                            }
                            stopWatch.Stop();

                            TimeSpan ts = stopWatch.Elapsed;
                            string elapsedTime = ts.Milliseconds.ToString();
                            //Thread.Sleep(500);
                            //Console.Write($"\rRunTime {elapsedTime}   ");
                        }
                    }
                }
            }
            if (questions.Count > 1)
                rep.AddQuestions(questions);
            //Thread.Sleep(2 * 1000);
            if (comments.Count > 1)
            {
                var cms =
                    comments.GroupBy(x => x.CommentId).Select(x => x.First()).ToList();//Where(x => x.CommentId != 46836997).ToList();
                //var cms1 = cms.Where(x => !commentIds.Contains(x.CommentId)).ToList();
                //var cms2 = cms.Where(x => commentIds.Contains(x.CommentId)).ToList();
                //var k = cms.Select(x => x.CommentId).ToArray();
                //var s = questions.Where(x => x.QuestionId == 46837536).FirstOrDefault();
                //var kk = k.Distinct().ToArray();
                rep.AddComments(cms);
            }
            int aaaaa = 55;
        }
        #endregion

        #region 04
        static void txt()
        {
            List<string> _txt = new List<string>();
            using (var textHelper = new TextHelper())
            {
                //var txts = System.IO.File.ReadAllLines(@"e:\cqa\Question_Body.txt");
                var txts = System.IO.File.ReadAllLines(@"e:\cqa\answer.csv");
                
                foreach (var txt1 in txts)
                {
                    string t = textHelper.CleanReview(txt1);
                    if (!string.IsNullOrEmpty(t))
                    {
                        _txt.Add(t);
                    }
                }
                //File.WriteAllLines(@"e:\cqa\Question_Body_clean.txt", _txt);
                File.WriteAllLines(@"e:\cqa\answer_clean.txt", _txt);
            }
            var ss = "";
        }
        #endregion

        #region 05 - Remove Duplicate Vocab
        public static void m5RemoveDuplicate()
        {
            var _text = File.ReadAllLines(@"c:\vocab_13.txt");
            var _vocab = _text.Distinct().ToArray();
            var _vocab1 = _vocab.OrderBy(x => x).ToArray();
            File.WriteAllLines(@"c:\sem\vocab13.txt",_vocab1);
        }
        #endregion

        public static void m6test()
        {
            List<QuestionExport> questionExports1 = new List<QuestionExport>();
            var jsonTxt = File.ReadAllText(@"c:\sem\1.json");
            var jsonData = JsonConvert.DeserializeObject<List<QuestionExport>>(jsonTxt);
            var countQuestion = jsonData.Select(c => c.QID).Distinct().ToList();
        }
    }

    class Answer
    {
        public string CID { get; set; }
        public long CommentId { get; set; }
        public string AnswerText = "";
        public string Label { get; set; }
    }

    class QuestionExport
    {
        public int QID { get; set; }
        public string QuestionText { get; set; }
        public List<Answer> GoodAnswers { get; set; }
        public List<Answer> BadAnswers { get; set; }
        public List<Answer> PotentialAnswers { get; set; }
    }
}