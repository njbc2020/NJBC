using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NJBC.Common;
using NJBC.DataLayer.IRepository;
using NJBC.DataLayer.Models.Semeval2015;
using NJBC.Models.Crawler;

namespace NJBC.Web.App.Label.Controllers
{
    public class ExportController : Controller
    {
        private readonly ISemEvalRepository SemEvalRepository;
        public ExportController(ISemEvalRepository SemEvalRepository)
        {
            this.SemEvalRepository = SemEvalRepository;
        }

        public IActionResult Index()
        {
            ImportJsonToDB();
            bool cleanData = true;
            var data = SemEvalRepository.GetLabeledQuestions().Result;
            string xmlHeader = System.IO.File.ReadAllText(@"E:\SemEval2016.xml");
            for (int j = 0; j < 2; j++)
            {

                StringBuilder sb = new StringBuilder(xmlHeader);
                foreach (var q in data)
                {
                    sb.AppendLine($"\t<Thread THREAD_SEQUENCE=\"Q{q.QID}_R16\">");
                    sb.AppendFormat("\t\t<RelQuestion RELQ_ID=\"Q{0}_R{1}\" RELQ_CATEGORY=\"{2}\" RELQ_DATE=\"{3}\" RELQ_USERID=\"U{4}\" RELQ_USERNAME=\"{5}\">\n", q.QID, 16, "Namzadi", q.QDATE.ToString("yyyy-MM-dd HH:mm:ss"), q.QUSERID, q.QUsername);
                    if (j == 0)
                    {
                        sb.AppendLine($"\t\t\t<RelQSubject>{q.QSubject}</RelQSubject>");
                        sb.AppendLine($"\t\t\t<RelQBody>{q.QBody}</RelQBody>");
                    }
                    if (j == 1)
                    {
                        try
                        {
                            sb.AppendLine($"\t\t\t<RelQSubject>{questions.FirstOrDefault(x => x.QuestionId == q.QuestionId).QSubject}</RelQSubject>");
                            sb.AppendLine($"\t\t\t<RelQBody>{questions.FirstOrDefault(x => x.QuestionId == q.QuestionId).QBody}</RelQBody>");
                        }
                        catch (Exception)
                        {
                            sb.AppendLine($"\t\t\t<RelQSubject>{ q.QSubject}</RelQSubject>");
                            sb.AppendLine($"\t\t\t<RelQBody>{q.QBody}</RelQBody>");
                        }
                    }
                    sb.Append("\t\t</RelQuestion>");
                    for (int i = 0; i < q.Comments.Count; i++)
                    {
                        var c = q.Comments.ToList();
                        sb.AppendLine($"\t\t<RelComment RELC_ID=\"Q{q.QuestionId}_R{16}_C{i+1}\" RELC_DATE=\"{commentDate[c[i].CommentId].ToString("yyyy-MM-dd HH:mm:ss")}\" RELC_USERID=\"U{c[i].CUSERID}\" RELC_USERNAME=\"{c[i].CUsername}\" RELC_RELEVANCE2RELQ=\"{c[i].CGOLD}\">");
                        if (j == 0)
                            sb.AppendLine($"\t\t\t<RelCText>{c[i].CBodyClean}</RelCText>");
                        if (j == 1)
                            sb.AppendLine($"\t\t\t<RelCText>{_comments.FirstOrDefault(x => x.CommentId == c[i].CommentId).CBodyClean}</RelCText>");
                        sb.AppendLine("\t\t</RelComment>");
                    }
                    sb.AppendLine("\t</Thread>");
                }
                sb.Append("</xml>");

                System.IO.File.WriteAllText($@"d:\x-{DateTime.Now.ToString("yyyy_MM_dd")}-{j}.xml", sb.ToString());
            }
            return Ok();
        }





        private static long qc = 0;
        private static long cc = 0;
        private static long uc = 0;
        static List<Question> questions = new List<Question>();
        static List<Comment> comments = new List<Comment>();
        static List<Comment> _comments = new List<Comment>();
        static Dictionary<long, DateTime> commentDate = new Dictionary<long, DateTime>();
        static Dictionary<string, long> usersDic = new Dictionary<string, long>();
        static Dictionary<long, string> usersIdDic = new Dictionary<long, string>();
        #region 03 Import Json To Database
        public static void ImportJsonToDB()
        {
            int[] forums = new int[] { 115, 116, 21 };
            int[] blockTopic = new int[] { 3298910, 1503527, 1662425, 3666349, 4432469, 2225778, 1449197 };

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
                                    string cleanText = message.TextClean;
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
                                        DateTime dt = new DateTime(_persianDate[0], _persianDate[1], _persianDate[2], 0, 0, 0, pc);
                                        cm.CDate = dt;
                                    }
                                    _comments.Add(cm);
                                }
                                var _cms = _comments.GroupBy(x => x.CommentId).Select(x => x.First()).ToList();
                                var _cms1 = _cms.GroupBy(x => x.CBodyClean).Select(x => x.First()).ToList();
                                comments.AddRange(_cms1);
                                questions.Add(q);
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
            //if (questions.Count > 1)
            //    ISemEvalRepository.AddQuestions(questions);
            ////Thread.Sleep(2 * 1000);
            //if (comments.Count > 1)
            //{
            _comments = comments.GroupBy(x => x.CommentId).Select(x => x.First()).ToList();
            commentDate = _comments.ToDictionary(x => x.CommentId, x => x.CDate.Value);
            //    ISemEvalRepository.AddComments(cms);
            //}
            int aaaaa = 55;
        }
        #endregion
    }
}