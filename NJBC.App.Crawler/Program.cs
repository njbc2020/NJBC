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

namespace NJBC.App.Crawler
{
    static class Program
    {
        private static long qc = 0;
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
                default:
                    break;
            }
            Console.WriteLine("Hello World!");
        }

        #region 03 Import Json To Database
        public static void ImportJsonToDB()
        {
            int[] forums = new int[] { 115, 116 };
            int[] blockTopic = new int[] { 3298910, 1503527, 1662425, 3666349 };

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

                                Question q = new Question()
                                {
                                    QuestionId = qc++,
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
                                            QuestionId = q.QuestionId,
                                        };
                                        if (!textHelper.RemoveSingleEmoji(cm.CBodyClean))
                                        {
                                            _comments.Add(cm);
                                        }
                                    }
                                }
                                var _cms = _comments.GroupBy(x => x.CommentId).Select(x => x.First()).ToList();
                                var _cms1 = _cms.GroupBy(x => x.CBodyClean).Select(x => x.First()).ToList();
                                comments.AddRange(_cms1);
                                questions.Add(q);

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
                var cms = comments.GroupBy(x => x.CommentId).Select(x => x.First()).ToList();
                rep.AddComments(cms);
            }
            int aaaaa = 55;
        }
        #endregion
    }
}