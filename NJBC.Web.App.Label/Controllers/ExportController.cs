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

        public IActionResult Index1()
        {
            List<string> answertrain = new List<string>();
            List<string> answertest = new List<string>();
            //ImportJsonToDB();
            bool cleanData = true;
            List<Question> data = SemEvalRepository.GetLabeledQuestions().Result;
            int spilit = (int)Math.Round(data.Count() * 0.70);
            var train = data.Take(spilit);
            IEnumerable<Question> test = data.Skip(spilit);

            int trainCount = train.Count();
            int testCount = test.Count();
            int sumTestTrain = testCount + trainCount;
            int total = data.Count - sumTestTrain;

            foreach (var item in train)
            {
                answertrain.AddRange(item.Comments.Select(x => x.CBodyClean).ToList());
            }

            foreach (var item in data.Skip(spilit))
            {
                answertest.AddRange(item.Comments.Select(x => x.CBodyClean).ToList());
            }

            int answertrainCount = answertrain.Count();
            int answertestCount = answertest.Count();

            int potentialTest = 0;
            int potentialTrain = 0;

            int badTest = 0;
            int badTrain = 0;

            int goodTest = 0;
            int goodTrain = 0;


            foreach (var item in test)
            {
                potentialTest += item.Comments.Where(x => x.CGOLD == "Potential").Count();
                goodTest += item.Comments.Where(x => x.CGOLD == "Good").Count();
                badTest += item.Comments.Where(x => x.CGOLD == "Bad").Count();
            }

            foreach (var item in train)
            {
                potentialTrain += item.Comments.Where(x => x.CGOLD == "Potential").Count();
                goodTrain += item.Comments.Where(x => x.CGOLD == "Good").Count();
                badTrain += item.Comments.Where(x => x.CGOLD == "Bad").Count();
            }

            string xmlHeader = System.IO.File.ReadAllText(@"c:\sem\SemEval2016.xml");
            for (int k = 0; k < 2; k++)
            {
                for (int j = 0; j < 2; j++)
                {
                    StringBuilder sb = new StringBuilder(xmlHeader);
                    List<Question> _data = new List<Question>();
                    if (k == 0)
                        _data = test.ToList();
                    else
                        _data = train.ToList();
                    foreach (var q in _data)
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
                            if (!questions.Any() ||
                                    questions.FirstOrDefault(x => x.QuestionId == q.QuestionId) == null ||
                                    string.IsNullOrEmpty(questions.FirstOrDefault(x => x.QuestionId == q.QuestionId).QSubject)
                                    )
                            {
                                sb.AppendLine($"\t\t\t<RelQSubject>{ q.QSubject}</RelQSubject>");
                                sb.AppendLine($"\t\t\t<RelQBody>{q.QBody}</RelQBody>");
                            }
                            else
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

                        }
                        sb.Append("\t\t</RelQuestion>");
                        for (int i = 0; i < q.Comments.Count; i++)
                        {
                            var c = q.Comments.ToList();
                            sb.AppendLine($"\t\t<RelComment RELC_ID=\"Q{q.QuestionId}_R{16}_C{i + 1}\" RELC_DATE=\"{commentDate[c[i].CommentId].ToString("yyyy-MM-dd HH:mm:ss")}\" RELC_USERID=\"U{c[i].CUSERID}\" RELC_USERNAME=\"{c[i].CUsername}\" RELC_RELEVANCE2RELQ=\"{c[i].CGOLD}\">");
                            if (j == 0)
                                sb.AppendLine($"\t\t\t<RelCText>{c[i].CBodyClean}</RelCText>");
                            if (j == 1)
                                sb.AppendLine($"\t\t\t<RelCText>{_comments.FirstOrDefault(x => x.CommentId == c[i].CommentId).CBodyClean}</RelCText>");
                            sb.AppendLine("\t\t</RelComment>");
                        }
                        sb.AppendLine("\t</Thread>");
                    }
                    sb.Append("</xml>");
                    if (k == 0)
                        System.IO.File.WriteAllText($@"c:\sem\Test-{DateTime.Now.ToString("yyyy_MM_dd")}-{j}.xml", sb.ToString());
                    else
                        System.IO.File.WriteAllText($@"c:\sem\Train-{DateTime.Now.ToString("yyyy_MM_dd")}-{j}.xml", sb.ToString());
                }
            }
            return Ok();
        }

        public IActionResult Index2()
        {
            List<string> answertrain = new List<string>();
            List<string> answertest = new List<string>();
            ImportJsonToDB();
            bool cleanData = true;
            List<Question> data = SemEvalRepository.GetLabeledQuestions().Result;
            int spilit = (int)Math.Round(data.Count() * 0.75);
            var train = data.Take(spilit);
            IEnumerable<Question> test = data.Skip(spilit);

            int trainCount = train.Count();
            int testCount = test.Count();
            int sumTestTrain = testCount + trainCount;
            int total = data.Count - sumTestTrain;

            foreach (var item in train)
            {
                answertrain.AddRange(item.Comments.Select(x => x.CBodyClean).ToList());
            }

            foreach (var item in data.Skip(spilit))
            {
                answertest.AddRange(item.Comments.Select(x => x.CBodyClean).ToList());
            }

            int answertrainCount = answertrain.Count();
            int answertestCount = answertest.Count();



            string xmlHeader = System.IO.File.ReadAllText(@"c:\sem\SemEval2016.xml");
            for (int k = 0; k < 2; k++)
            {
                for (int j = 0; j < 2; j++)
                {
                    StringBuilder sb = new StringBuilder();
                    List<Question> _data = new List<Question>();
                    if (k == 0)
                        _data = test.ToList();
                    else
                        _data = train.ToList();
                    sb.AppendLine("qid,question,answer,label");
                    foreach (var q in _data)
                    {
                        for (int i = 0; i < q.Comments.Count; i++)
                        {
                            var c = q.Comments.ToList();
                            if (j == 0)
                                sb.AppendLine($"{q.QID},{q.QBody},{c[i].CBodyClean},{c[i].CGOLD}");
                            if (j == 1)
                                sb.AppendLine($"{q.QID},{q.QBody},{_comments.FirstOrDefault(x => x.CommentId == c[i].CommentId).CBodyClean},{c[i].CGOLD}");
                        }
                    }
                    if (k == 0)
                        System.IO.File.WriteAllText($@"c:\sem\Test-{j}.csv", sb.ToString());
                    else
                        System.IO.File.WriteAllText($@"c:\sem\Train-{j}.csv", sb.ToString());
                }
            }
            return Ok();
        }


        public IActionResult Index21()
        {
            List<question> questions = new List<question>();
            List<string> answertrain = new List<string>();
            //ImportJsonToDB();
            bool cleanData = true;
            List<Question> data = SemEvalRepository.GetLabeledQuestions().Result;

            var train = data;

            foreach (var item in train)
            {
                answertrain.AddRange(item.Comments.Select(x => x.CBodyClean).ToList());
            }

            for (int j = 0; j < 1; j++)
            {
                StringBuilder sb = new StringBuilder();
                List<Question> _data = new List<Question>();
                _data = train.ToList();
                List<QuestionExport> questionExports = new List<QuestionExport>();
                List<QuestionExport> questionExports1 = new List<QuestionExport>();
                sb.AppendLine("qid,question,answer,label");
                foreach (var q in _data)
                {
                    QuestionExport _question = new QuestionExport();

                    List<Answer> _answers = new List<Answer>();
                    for (int i = 0; i < q.Comments.Count; i++)
                    {
                        var c = q.Comments.ToList();
                        if (j == 0)
                        {
                            sb.AppendLine($"{q.QID},{q.QBody},{c[i].CBodyClean},{c[i].CGOLD}");
                            _answers.Add(new Answer { CommentId = c[i].CommentId, CID = c[i].CID, AnswerText = c[i].CBodyClean, Label = c[i].CGOLD });
                        }
                        if (j == 1)
                        {
                            string cText = _comments.FirstOrDefault(x => x.CommentId == c[i].CommentId).CBodyClean;
                            sb.AppendLine($"{q.QID},{q.QBody},{cText},{c[i].CGOLD}");
                            _answers.Add(new Answer { CommentId = c[i].CommentId, CID = c[i].CID, AnswerText = cText, Label = c[i].CGOLD });
                        }
                    }
                    _question.QuestionText = q.QBody;
                    _question.QID = int.Parse(q.QID);

                    _question.GoodAnswers = new List<Answer>();
                    if (_answers.Where(x => x.Label == "Good").Any())
                        _question.GoodAnswers.AddRange(_answers.Where(x => x.Label == "Good"));

                    _question.BadAnswers = new List<Answer>();
                    if (_answers.Where(x => x.Label == "Bad").Any())
                        _question.BadAnswers.AddRange(_answers.Where(x => x.Label == "Bad"));

                    _question.PotentialAnswers = new List<Answer>();
                    if (_answers.Where(x => x.Label == "Potential").Any())
                        _question.PotentialAnswers.AddRange(_answers.Where(x => x.Label == "Potential"));

                    questionExports.Add(_question);

                }
                System.IO.File.WriteAllText($@"c:\sem\NiniAll-{j}.csv", sb.ToString(), Encoding.UTF8);
                System.IO.File.WriteAllText($@"c:\sem\NiniAll-{j}.Json", JsonConvert.SerializeObject(questionExports, Formatting.Indented));
                foreach (var itemm in questionExports.Randomize())
                {
                    questionExports1.Add(itemm);
                }
                System.IO.File.WriteAllText($@"c:\sem\NiniAll_Radom-{j}.Json", JsonConvert.SerializeObject(questionExports1, Formatting.Indented));
            }
            return Ok();
        }

        public IActionResult Data702010()
        {
            List<question> questions = new List<question>();
            Dictionary<int, string> dataType = new Dictionary<int, string>()
            {
                {0, "Train" },
                {1, "Test" },
                {2, "Dev" }
            };
            //List<string> answertrain = new List<string>();
            //ImportJsonToDB();
            bool cleanData = true;
            List<Question> data1 = new List<Question>();
            var pathRandom = $@"c:\sem\NiniAll-Random.Json";
            if (System.IO.File.Exists(pathRandom))
            {
                var fileContent = System.IO.File.ReadAllText(pathRandom, Encoding.UTF8);
                data1 = JsonConvert.DeserializeObject<List<Question>>(fileContent);
            }
            else
            {
                List<Question> data0 = SemEvalRepository.GetLabeledQuestions().Result;
                data1 = data0.Randomize().ToList();
                //var fileContent = JsonConvert.SerializeObject(data1.ToArray());
                //System.IO.File.WriteAllText(pathRandom, fileContent, Encoding.UTF8);
            }

            int spilitTrain = (int)Math.Round(data1.Count() * 0.70);
            int spilitTest = (int)Math.Round(data1.Count() * 0.20);
            int spilitDev = spilitTrain + spilitTest;

            var trainData = data1.Take(spilitTrain).ToList();
            var testData = data1.Skip(spilitTrain).Take(spilitTest).ToList();
            var devData = data1.Skip(spilitDev).ToList();

            List<List<Question>> data = new List<List<Question>>();
            data.Add(trainData);
            data.Add(testData);
            data.Add(devData);

            List<int> _questionLenght = new List<int>();
            List<int> _allLenght = new List<int>();
            List<int> _goodLenght = new List<int>();
            List<int> _badLenght = new List<int>();
            List<int> _potentialLenght = new List<int>();
            int _questionCount = 0;
            int _allCount = 0;
            int _goodCount = 0;
            int _badCount = 0;
            int _potentialCount = 0;


            //foreach (var item in train)
            //{
            //    answertrain.AddRange(item.Comments.Select(x => x.CBodyClean).ToList());
            //}
            for (int d = 0; d < data.Count(); d++)
            {
                for (int j = 0; j < 1; j++)
                {

                    StringBuilder sb = new StringBuilder();
                    List<Question> _data = new List<Question>();
                    _data = data[d];
                    int allCount = 0;
                    int goodCount = 0;
                    int badCount = 0;
                    int potentialCount = 0;
                    List<int> questionLenght = new List<int>();
                    List<int> allLenght = new List<int>();
                    List<int> goodLenght = new List<int>();
                    List<int> badLenght = new List<int>();
                    List<int> potentialLenght = new List<int>();
                    //var Data_Test=
                    List<QuestionExport> questionExports = new List<QuestionExport>();
                    List<QuestionExport> questionExports1 = new List<QuestionExport>();
                    sb.AppendLine("qid,question,answer,label");
                    foreach (var q in _data)
                    {
                        QuestionExport _question = new QuestionExport();

                        List<Answer> _answers = new List<Answer>();
                        for (int i = 0; i < q.Comments.Count; i++)
                        {
                            var c = q.Comments.ToList();
                            if (j == 0)
                            {
                                sb.AppendLine($"{q.QID},{q.QBody},{c[i].CBodyClean},{c[i].CGOLD}");
                                _answers.Add(new Answer { CommentId = c[i].CommentId, CID = c[i].CID, AnswerText = string.IsNullOrEmpty(c[i].CBodyClean) ? "" : c[i].CBodyClean, Label = c[i].CGOLD });
                            }
                            if (j == 1)
                            {
                                string cText = _comments.FirstOrDefault(x => x.CommentId == c[i].CommentId).CBodyClean;
                                sb.AppendLine($"{q.QID},{q.QBody},{cText},{c[i].CGOLD}");
                                _answers.Add(new Answer { CommentId = c[i].CommentId, CID = c[i].CID, AnswerText = cText, Label = c[i].CGOLD });
                            }
                        }
                        _question.QuestionText = q.QBody;
                        _question.QID = int.Parse(q.QID);

                        questionLenght.Add(q.QBody.Length);
                        _questionLenght.Add(q.QBody.Length);

                        _question.GoodAnswers = new List<Answer>();
                        if (_answers.Where(x => x.Label == "Good").Any())
                        {
                            var good = _answers.Where(x => x.Label == "Good");
                            _question.GoodAnswers.AddRange(good);
                            allCount += good.Count();
                            goodCount += good.Count();
                            allLenght.AddRange(good.Select(x => x.AnswerText.Length));
                            goodLenght.AddRange(good.Select(x => x.AnswerText.Length));
                        }

                        _question.BadAnswers = new List<Answer>();
                        if (_answers.Where(x => x.Label == "Bad").Any())
                        {
                            var bad = _answers.Where(x => x.Label == "Bad");
                            _question.BadAnswers.AddRange(bad);
                            allCount += bad.Count();
                            badCount += bad.Count();
                            allLenght.AddRange(bad.Select(x => x.AnswerText.Length));
                            badLenght.AddRange(bad.Select(x => x.AnswerText.Length));
                        }


                        _question.PotentialAnswers = new List<Answer>();
                        if (_answers.Where(x => x.Label == "Potential").Any())
                        {
                            var potential = _answers.Where(x => x.Label == "Potential");
                            _question.PotentialAnswers.AddRange(potential);
                            allCount += potential.Count();
                            potentialCount += potential.Count();
                            allLenght.AddRange(potential.Select(x => x.AnswerText.Length));
                            potentialLenght.AddRange(potential.Select(x => x.AnswerText.Length));
                        }
                        questionExports.Add(_question);
                        ++_questionCount;

                    }

                    StringBuilder detail = new StringBuilder();
                    detail.AppendLine($"Count");
                    detail.AppendLine($"Question,{questionExports.Count}");
                    detail.AppendLine($"Answer,{allCount}");
                    detail.AppendLine($"Good,{goodCount}");
                    detail.AppendLine($"Bad,{badCount}");
                    detail.AppendLine($"Potential,{potentialCount}");

                    detail.AppendLine($"Lenght");
                    detail.AppendLine($"Question Avg,{questionLenght.Average()}");
                    detail.AppendLine($"Question Median,{Median(questionLenght)}");
                    detail.AppendLine($"All Avg,{allLenght.Average()}");
                    detail.AppendLine($"All Median,{Median(allLenght)}");
                    detail.AppendLine($"Good Avg,{goodLenght.Average()}");
                    detail.AppendLine($"Good Median,{Median(goodLenght)}");
                    detail.AppendLine($"Bad Avg,{badLenght.Average()}");
                    detail.AppendLine($"Bad Median,{Median(badLenght)}");
                    detail.AppendLine($"Potential Avg,{potentialLenght.Average()}");
                    detail.AppendLine($"Potential Median,{Median(potentialLenght)}");
                    string detailContent = detail.ToString();

                    System.IO.File.WriteAllText($@"c:\sem\Nini-{dataType[d]}.csv", sb.ToString(), Encoding.UTF8);
                    System.IO.File.WriteAllText($@"c:\sem\Nini-{dataType[d]}.Json", JsonConvert.SerializeObject(questionExports, Formatting.Indented));
                    System.IO.File.WriteAllText($@"c:\sem\Details\Nini-{dataType[d]}-Detail.csv", detailContent);
                    System.IO.File.WriteAllText($@"c:\sem\Lenghts\{dataType[d]}\Nini-{dataType[d]}--Lenght_AllLabel.csv", string.Join("\n", allLenght));
                    System.IO.File.WriteAllText($@"c:\sem\Lenghts\{dataType[d]}\Nini-{dataType[d]}--Lenght_Good.csv", string.Join("\n", goodLenght));
                    System.IO.File.WriteAllText($@"c:\sem\Lenghts\{dataType[d]}\Nini-{dataType[d]}--Lenght_Bad.csv", string.Join("\n", badLenght));
                    System.IO.File.WriteAllText($@"c:\sem\Lenghts\{dataType[d]}\Nini-{dataType[d]}--Lenght_Potential.csv", string.Join("\n", potentialLenght));
                    _allLenght.AddRange(allLenght);
                    _goodLenght.AddRange(goodLenght);
                    _badLenght.AddRange(badLenght);
                    _potentialLenght.AddRange(potentialLenght);
                    _allCount += allCount;
                    _goodCount += goodCount;
                    _badCount += badCount;
                    _potentialCount += potentialCount;
                }

                StringBuilder detail1 = new StringBuilder();
                detail1.AppendLine($"Count");
                detail1.AppendLine($"Question,{_questionCount}");
                detail1.AppendLine($"Answer,{_allCount}");
                detail1.AppendLine($"Good,{_goodCount}");
                detail1.AppendLine($"Bad,{_badCount}");
                detail1.AppendLine($"Potential,{_potentialCount}");

                detail1.AppendLine($"Lenght");
                detail1.AppendLine($"Question Avg,{_questionLenght.Average()}");
                detail1.AppendLine($"Question Median,{Median(_questionLenght)}");
                detail1.AppendLine($"All Avg,{_allLenght.Average()}");
                detail1.AppendLine($"All Median,{Median(_allLenght)}");
                detail1.AppendLine($"Good Avg,{_goodLenght.Average()}");
                detail1.AppendLine($"Good Median,{Median(_goodLenght)}");
                detail1.AppendLine($"Bad Avg,{_badLenght.Average()}");
                detail1.AppendLine($"Bad Median,{Median(_badLenght)}");
                detail1.AppendLine($"Potential Avg,{_potentialLenght.Average()}");
                detail1.AppendLine($"Potential Median,{Median(_potentialLenght)}");
                System.IO.File.WriteAllText($@"c:\sem\Details\NiniAll-Detail.csv", detail1.ToString());
                System.IO.File.WriteAllText($@"c:\sem\Lenghts\NiniAll--Lenght_AllLabel.csv", string.Join("\n", _allLenght));
                System.IO.File.WriteAllText($@"c:\sem\Lenghts\NiniAll--Lenght_Good.csv", string.Join("\n", _goodLenght));
                System.IO.File.WriteAllText($@"c:\sem\Lenghts\NiniAll--Lenght_Bad.csv", string.Join("\n", _badLenght));
                System.IO.File.WriteAllText($@"c:\sem\Lenghts\NiniAll--Lenght_Potential.csv", string.Join("\n", _potentialLenght));

            }

            return Ok();
        }

        public IActionResult DataLenghts()
        {
            List<Question> data = SemEvalRepository.GetLabeledQuestions().Result;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbGood = new StringBuilder();
            StringBuilder sbPotential = new StringBuilder();
            StringBuilder sbBad = new StringBuilder();
            foreach (var q in data)
            {
                var _answers = q.Comments.ToList();
                int lenQ = q.QBody.Length;
                if (_answers.Where(x => x.CGOLD == "Good").Any())
                {
                    var good = _answers.Where(x => x.CGOLD == "Good");
                    List<string> ls = new List<string>();
                    foreach (var x in good)
                    {
                        if (x.CBodyClean == null)
                        {
                            ls.Add($"{lenQ}\t0");
                        }
                        else
                        {
                            ls.Add($"{lenQ}\t{x.CBodyClean.Length}");
                        }
                    }
                    sb.AppendLine(string.Join("\n", ls));
                    sbGood.AppendLine(string.Join("\n", ls));
                }
                if (_answers.Where(x => x.CGOLD == "Bad").Any())
                {
                    var bad = _answers.Where(x => x.CGOLD == "Bad");
                    List<string> ls = new List<string>();
                    foreach (var x in bad)
                    {
                        if (x.CBodyClean == null)
                        {
                            ls.Add($"{lenQ}\t0");
                        }
                        else
                        {
                            ls.Add($"{lenQ}\t{x.CBodyClean.Length}");
                        }
                    }
                    sb.AppendLine(string.Join("\n", ls));
                    sbBad.AppendLine(string.Join("\n", ls));
                }


                if (_answers.Where(x => x.CGOLD == "Potential").Any())
                {
                    var potential = _answers.Where(x => x.CGOLD == "Potential");
                    List<string> ls = new List<string>();
                    foreach (var x in potential)
                    {
                        if (x.CBodyClean == null)
                        {
                            ls.Add($"{lenQ}\t0");
                        }
                        else
                        {
                            ls.Add($"{lenQ}\t{x.CBodyClean.Length}");
                        }
                    }
                    sb.AppendLine(string.Join("\n", ls));
                    sbPotential.AppendLine(string.Join("\n", ls));
                }
            }
            var sAll = sb.ToString();
            var sGood = sbGood.ToString();
            var sBad = sbBad.ToString();
            var sPotential = sbPotential.ToString();
            return Ok();
        }

        public IActionResult Index3()
        {
            List<string> answertrain = new List<string>();
            List<string> answertest = new List<string>();
            ImportJsonToDB();
            bool cleanData = true;
            List<Question> data = SemEvalRepository.GetLabeledQuestions().Result;
            int spilit = (int)Math.Round(data.Count() * 0.75);
            var train = data.Take(spilit);
            IEnumerable<Question> test = data.Skip(spilit);

            int trainCount = train.Count();
            int testCount = test.Count();
            int sumTestTrain = testCount + trainCount;
            int total = data.Count - sumTestTrain;

            foreach (var item in train)
            {
                answertrain.AddRange(item.Comments.Select(x => x.CBodyClean).ToList());
            }

            foreach (var item in data.Skip(spilit))
            {
                answertest.AddRange(item.Comments.Select(x => x.CBodyClean).ToList());
            }

            int answertrainCount = answertrain.Count();
            int answertestCount = answertest.Count();



            string xmlHeader = System.IO.File.ReadAllText(@"c:\sem\SemEval2016.xml");
            for (int k = 0; k < 2; k++)
            {

                for (int j = 0; j < 1; j++)
                {
                    List<jj> train1 = new List<jj>();
                    StringBuilder sb = new StringBuilder();
                    List<Question> _data = new List<Question>();
                    List<answer> _answer = new List<answer>();
                    List<question> questions = new List<question>();
                    long kk = 0;
                    if (k == 0)
                        _data = test.ToList();
                    else
                        _data = train.ToList();
                    sb.AppendLine("qid,question,answer,label");
                    foreach (var q in _data)
                    {

                        var c = q.Comments.ToList();
                        questions.Add(new question
                        {
                            text = q.QBody,
                            answers = q.Comments.Select(x => new answer { id = kk++, comment = x.CBodyClean }).ToArray()
                        });
                    }
                    if (k == 0)
                    {
                        System.IO.File.WriteAllText($@"c:\sem\Test-{j}.json", JsonConvert.SerializeObject(questions.Select(x => new { answer = x.answers.Select(y => y.id).ToArray(), question = x.text })));
                        System.IO.File.WriteAllText($@"c:\sem\test_answer-{j}.json", JsonConvert.SerializeObject(questions.Select(x => x.answers.Select(y => y.comment))));
                    }
                    else
                    {
                        System.IO.File.WriteAllText($@"c:\sem\Train-{j}.json", JsonConvert.SerializeObject(questions.Select(x => new { answer = x.answers.Select(y => y.id).ToArray(), question = x.text })));
                        List<string> _answer2 = new List<string>();
                        var _answers1 = questions.Select(x => x.answers.Select(y => y.comment)).ToList().Select(x => x).ToList().Select(x => x).ToList();
                        foreach (var item in _answers1)
                        {
                            _answer2.AddRange(item);
                        }
                        System.IO.File.WriteAllText($@"c:\sem\Train_answer-{j}.json", JsonConvert.SerializeObject(_answer2));
                    }
                }
            }
            return Ok();
        }


        public IActionResult Index4()
        {
            List<Question> data = SemEvalRepository.GetLabeledQuestions().Result;


            string xmlHeader = System.IO.File.ReadAllText(@"c:\sem\SemEval2016.xml");


            StringBuilder sb = new StringBuilder(xmlHeader);
            List<Question> _data = new List<Question>();

            foreach (var q in data)
            {
                sb.AppendLine($"\t<Thread THREAD_SEQUENCE=\"Q{q.QID}_R16\">");
                sb.AppendFormat("\t\t<RelQuestion RELQ_ID=\"Q{0}_R{1}\" RELQ_CATEGORY=\"{2}\" RELQ_DATE=\"{3}\" RELQ_USERID=\"U{4}\" RELQ_USERNAME=\"{5}\">\n", q.QID, 16, "Namzadi", q.QDATE.ToString("yyyy-MM-dd HH:mm:ss"), q.QUSERID, q.QUsername);

                sb.AppendLine($"\t\t\t<RelQSubject>{q.QSubject}</RelQSubject>");
                sb.AppendLine($"\t\t\t<RelQBody>{q.QBody}</RelQBody>");

                sb.Append("\t\t</RelQuestion>");
                for (int i = 0; i < q.Comments.Count; i++)
                {
                    var c = q.Comments.ToList();
                    sb.AppendLine($"\t\t<RelComment RELC_ID=\"Q{q.QuestionId}_R{16}_C{i + 1}\" RELC_DATE=\"{c[i].CDate}\" RELC_USERID=\"U{c[i].CUSERID}\" RELC_USERNAME=\"{c[i].CUsername}\" RELC_RELEVANCE2RELQ=\"{c[i].CGOLD}\">");

                    sb.AppendLine($"\t\t\t<RelCText>{c[i].CBodyClean}</RelCText>");
                    sb.AppendLine("\t\t</RelComment>");
                }
                sb.AppendLine("\t</Thread>");
            }
            sb.Append("</xml>");
            System.IO.File.WriteAllText($@"c:\sem\Data_XML_SemEval_Ninisite.xml", sb.ToString(), Encoding.UTF8);

            return Ok();
        }

        public IActionResult Index5()
        {
            List<Question> data = SemEvalRepository.GetLabeledQuestions().Result;

            string xmlHeader = System.IO.File.ReadAllText(@"c:\sem\SemEval2016.xml");

            StringBuilder sb = new StringBuilder();
            IEnumerable<Question> _data = new List<Question>();
            _data = data.Randomize();

            sb.AppendLine("qid,question,answer,label");
            foreach (var q in _data)
            {
                for (int i = 0; i < q.Comments.Count; i++)
                {
                    var c = q.Comments.ToList();
                    sb.AppendLine($"{q.QID},{q.QBody},{c[i].CBodyClean},{c[i].CGOLD}");
                }
            }
            System.IO.File.WriteAllText($@"c:\sem\Data_CSV_Random.csv", sb.ToString(), Encoding.UTF8);

            return Ok();
        }


        private int Median(List<int> numbers)
        {
            int numberCount = numbers.Count();
            int halfIndex = numbers.Count() / 2;
            var sortedNumbers = numbers.OrderBy(n => n);
            int median;
            if ((numberCount % 2) == 0)
            {
                median = (sortedNumbers.ElementAt(halfIndex) + sortedNumbers.ElementAt(halfIndex - 1)) / 2;
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }
            return median;
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
                string path1 = Path.Combine(@"c:\sem\", $"{forum}.zip");
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
    class jj
    {
        public long[] answers { get; set; }
        public string question { get; set; }
    }
    class answer
    {
        public long id { get; set; }
        public string comment { get; set; }
    }

    class question
    {
        public answer[] answers { get; set; }
        public string text { get; set; }
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
    public static class IEnumerableExtensions
    {

        public static IEnumerable<t> Randomize<t>(this IEnumerable<t> target)
        {
            Random r = new Random();

            return target.OrderBy(x => (r.Next()));
        }
    }
}