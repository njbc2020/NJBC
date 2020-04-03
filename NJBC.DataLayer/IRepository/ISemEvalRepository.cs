using NJBC.DataLayer.Models;
using NJBC.DataLayer.Models.Semeval2015;
using NJBC.Models.Crawler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NJBC.DataLayer.IRepository
{
    public interface ISemEvalRepository
    {
        #region SemEval 2015
        Task AddQuestion(Topic topic);
        Task AddQuestion(Question input, bool saveNow = true);
        Task AddQuestions(List<Question> input, bool saveNow = true);
        Task<Question> GetQuestionByIdAsync(int id);
        Task<List<Question>> GetQuestionByIdAsync(long[] ids);
        List<Question> QuestionSearchAsync(string category, string subject);
        Task<Question> GetActiveQuestion(int userId);
        Task<bool> RejectQuestion(long questionId);
        Task<bool> SetLabelCompelete(long questionId);

        Task AddComment(Comment input, bool saveNow = true);
        Task AddComments(List<Comment> input, bool saveNow = true);
        Task<Comment> GetCommentByIdAsync(int id);
        Task<List<Comment>> GetCommentByIdAsync(long[] ids);
        List<Comment> SearchCommentAsync(string name, string text);
        Task<bool> SetLabelComment(long commentId, string label, int userId);
        #endregion

        #region SemEval 2016
        Task AddOrgQuestion(OrgQuestion input, bool saveNow = true);
        Task AddOrgQuestion(List<OrgQuestion> input, bool saveNow = true);
        Task<OrgQuestion> GetOrgQuestionByIdAsync(int id);
        Task<List<OrgQuestion>> GetOrgQuestionByIdAsync(int[] ids);
        List<OrgQuestion> OrgQuestionSearchAsync(string name, string subject);

        Task AddRelQuestion(RelQuestion input, bool saveNow = true);
        Task AddRelQuestion(List<RelQuestion> input, bool saveNow = true);
        Task<RelQuestion> GetRelQuestionByIdAsync(int id);
        Task<List<RelQuestion>> GetRelQuestionsByIdAsync(int[] ids);
        Task<List<RelQuestion>> GetRelQuestionsByRelqIdNameAsync(string[] relqIdNames);
        Task<RelQuestion> GetRelQuestionCategoryAsync(string category);
        Task<List<RelQuestion>> GetRelQuestionsCategoryAsync(string category);
        Task<List<RelQuestion>> SearchRelQuestionAsync(string category, string subject);

        Task AddRelComment(RelComment input, bool saveNow = true);
        Task AddRelComment(List<RelComment> input, bool saveNow = true);
        Task<RelComment> GetRelCommentByIdAsync(int id);
        Task<List<RelComment>> GetRelCommentByIdAsync(int[] ids);
        Task<List<RelComment>> SearchRelCommentAsync(string name, string text);

        #endregion
    }
}
