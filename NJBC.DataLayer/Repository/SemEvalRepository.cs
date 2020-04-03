﻿using NJBC.DataLayer.IRepository;
using NJBC.DataLayer.Models;
using NJBC.DataLayer.Models.Semeval2015;
using NJBC.Models.Crawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NJBC.DataLayer.Repository
{
    public class SemEvalRepository : ISemEvalRepository
    {
        #region init
        private readonly NJBC_DBContext dBContext;
        public SemEvalRepository(NJBC_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public SemEvalRepository()
        {
            this.dBContext = new NJBC_DBContext();
        }
        #endregion

        #region SemEval 2015
        public async Task AddQuestion(Topic topic)
        {
            try
            {
                Question q = new Question()
                {
                    QBody = topic.Description,
                    QSubject = topic.Question,
                    QCATEGORY = "116",
                    QDATE = DateTime.Now,
                    QGOLD_YN = "Not Applicable",
                    QID = topic.TopicId.ToString(),
                    QTYPE = "General",
                    QuestionId = topic.TopicId,
                    //QUSERID = dic,
                    QUsername = topic.NickName,
                    UserId = 2
                };

                dBContext.Questions.Add(q);
                dBContext.SaveChanges();

                List<Comment> comments = new List<Comment>();
                foreach (var message in topic.Messages)
                {
                    Comment cm = new Comment()
                    {
                        CBody = message.Text,
                        CBodyClean = message.TextClean,
                        CUsername = message.Name,
                        CGOLD = "",
                        CGOLD_YN = "",
                        CSubject = "",
                        //CUSERID = "",
                        ReplayCommentId = message.ReplayId,
                        QuestionId = q.QuestionId,
                        CID = message.MessageId.ToString()
                    };
                    comments.Add(cm);
                }

                dBContext.Comments.AddRange(comments);
                dBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

        }
        public async Task AddQuestion(Question input, bool saveNow = true)
        {
            await dBContext.Questions.AddAsync(input);

            if (saveNow)
                await dBContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task AddQuestions(List<Question> input, bool saveNow = true)
        {
            dBContext.Questions.AddRange(input);

            if (saveNow)
                dBContext.SaveChanges();//.ConfigureAwait(false);
        }

        public async Task<Question> GetQuestionByIdAsync(int id)
        {
            return await dBContext.Questions.FindAsync(id);
        }

        public async Task<List<Question>> GetQuestionByIdAsync(long[] ids)
        {
            return dBContext.Questions.Where(x => ids.Contains(x.QuestionId)).ToList();
        }

        public List<Question> QuestionSearchAsync(string category, string subject)
        {
            return dBContext.Questions.Where(x =>
                                                    (string.IsNullOrEmpty(category) || (!string.IsNullOrEmpty(category) && x.QCATEGORY == category)) &&
                                                    (string.IsNullOrEmpty(subject) || (!string.IsNullOrEmpty(subject) && x.QUsername == subject))
                                                    ).ToList();
        }

        public async Task AddComment(Comment input, bool saveNow = true)
        {
            await dBContext.Comments.AddAsync(input);

            if (saveNow)
                await dBContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task AddComments(List<Comment> input, bool saveNow = true)
        {
            if (true)
            {
                dBContext.Comments.AddRange(input);

                if (saveNow)
                    dBContext.SaveChanges();
            }
            if (false)
            {
                try
                {
                    for (int i = 0; i < (input.Count / 50000) + 1; i++)
                    {
                        int ssss = (input.Count / 50000);
                        var _temp = input.Skip(i * 50000).Take(50000).ToList();
                        var _temp11 = _temp.Select(x => x.CommentId).Distinct().ToList();
                        dBContext.Comments.AddRange(_temp);

                        if (saveNow)
                            dBContext.SaveChanges();
                    }
                    //.ConfigureAwait(false);

                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await dBContext.Comments.FindAsync(id);
        }

        public async Task<List<Comment>> GetCommentByIdAsync(long[] ids)
        {
            return dBContext.Comments.Where(x => ids.Contains(x.CommentId)).ToList();
        }

        public List<Comment> SearchCommentAsync(string subejct, string username)
        {
            return dBContext.
                Comments.Where(x =>
                              (string.IsNullOrEmpty(subejct) || (!string.IsNullOrEmpty(subejct) && x.CSubject == subejct)) &&
                              (string.IsNullOrEmpty(username) || (!string.IsNullOrEmpty(username) && x.CUsername == username))
                              ).ToList();
        }
        #endregion

        #region Org Question -------------
        public async Task AddOrgQuestion(OrgQuestion input, bool saveNow = true)
        {
            await dBContext.OrgQuestion.AddAsync(input);

            if (saveNow)
                await dBContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task AddOrgQuestion(List<OrgQuestion> input, bool saveNow = true)
        {
            await dBContext.OrgQuestion.AddRangeAsync(input);

            if (saveNow)
                await dBContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async virtual Task<OrgQuestion> GetByIdAsyncOrgQuestion(int ids)
        {
            return await dBContext.OrgQuestion.FindAsync(ids);
        }

        public async Task<OrgQuestion> GetOrgQuestionByIdAsync(int id)
        {
            return await dBContext.OrgQuestion.FindAsync(id);
        }

        public async Task<List<OrgQuestion>> GetOrgQuestionByIdAsync(int[] ids)
        {
            return dBContext.OrgQuestion.Where(x => ids.Contains(x.OrgqId)).ToList();
        }

        public List<OrgQuestion> OrgQuestionSearchAsync(string name, string subject)
        {
            return dBContext.OrgQuestion.Where(x =>
                                                    (string.IsNullOrEmpty(name) || (!string.IsNullOrEmpty(name) && x.OrgqIdName == name)) &&
                                                    (string.IsNullOrEmpty(subject) || (!string.IsNullOrEmpty(subject) && x.OrgQsubject == subject))
                                                    ).ToList();
        }

        #endregion

        #region Rel Comment -------------
        public async Task AddRelComment(RelComment input, bool saveNow = true)
        {
            await dBContext.RelComment.AddAsync(input);

            if (saveNow)
                await dBContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task AddRelComment(List<RelComment> input, bool saveNow = true)
        {
            await dBContext.RelComment.AddRangeAsync(input);

            if (saveNow)
                await dBContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<RelComment> GetRelCommentByIdAsync(int id)
        {
            return dBContext.RelComment.Where(x => x.RelcId == id).FirstOrDefault();
        }

        public async Task<List<RelComment>> GetRelCommentByIdAsync(int[] ids)
        {
            return dBContext.RelComment.Where(x => ids.Contains(x.RelcId)).ToList();
        }

        public async Task<List<RelComment>> SearchRelCommentAsync(string name, string text)
        {
            return dBContext.RelComment.Where(x =>
                                                    (string.IsNullOrEmpty(name) || (!string.IsNullOrEmpty(name) && x.RelcIdName == name)) &&
                                                    (string.IsNullOrEmpty(text) || (!string.IsNullOrEmpty(text) && x.RelCtext == text))
                                                    ).ToList();
        }
        #endregion

        #region Rel Question -------------
        public async Task AddRelQuestion(RelQuestion input, bool saveNow = true)
        {
            await dBContext.RelQuestion.AddAsync(input);

            if (saveNow)
                await dBContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task AddRelQuestion(List<RelQuestion> input, bool saveNow = true)
        {
            await dBContext.RelQuestion.AddRangeAsync(input);

            if (saveNow)
                await dBContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<RelQuestion> GetRelQuestionByIdAsync(int id)
        {
            return dBContext.RelQuestion.Find(id);
        }

        public async Task<RelQuestion> GetRelQuestionCategoryAsync(string category)
        {
            return dBContext.RelQuestion.Where(x => x.RelqCategory == category).FirstOrDefault();
        }

        public async Task<List<RelQuestion>> GetRelQuestionsByIdAsync(int[] ids)
        {
            return dBContext.RelQuestion.Where(x => ids.Contains(x.RelqId)).ToList();
        }

        public async Task<List<RelQuestion>> GetRelQuestionsByRelqIdNameAsync(string[] relqIdNames)
        {
            return dBContext.RelQuestion.Where(x => relqIdNames.Contains(x.RelqIdName)).ToList();
        }

        public async Task<List<RelQuestion>> GetRelQuestionsCategoryAsync(string category)
        {
            return dBContext.RelQuestion.Where(x => category.Contains(x.RelqCategory)).ToList();
        }

        public async Task<List<RelQuestion>> SearchRelQuestionAsync(string category, string subject)
        {
            return dBContext.RelQuestion.Where(x =>
                                                    (string.IsNullOrEmpty(category) || (!string.IsNullOrEmpty(category) && x.RelqCategory == category)) &&
                                                    (string.IsNullOrEmpty(subject) || (!string.IsNullOrEmpty(subject) && x.RelQsubject == subject))
                                                    ).ToList();
        }

        #endregion
    }
}