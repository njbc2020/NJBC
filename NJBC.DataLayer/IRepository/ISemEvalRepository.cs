using NJBC.DataLayer.Models;
using NJBC.DataLayer.Models.Semeval2015;
using NJBC.Models.Crawler;
using NJBC.Models.DTO.Web;
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
        Task<Question> GetQuestionByIdAsync(long id);
        Task<Question> GetQuestionByQIDAsync(long id);
        Task<List<Question>> GetQuestionByIdAsync(long[] ids);
        List<Question> QuestionSearchAsync(string category, string subject);
        Task<Question> GetActiveQuestion(string username);
        Task<bool> RejectQuestion(long questionId);
        Task<bool> ActiveQuestion(long questionId);
        Task<bool> AdvQuestion(long questionId);
        Task<bool> SetLabelCompelete(CompleteQuestionParam questionId);
        Task<List<Question>> GetQuestionList(int count, int page);
        Task<int> GetQuestionsCount();

        Task AddComment(Comment input, bool saveNow = true);
        Task AddComments(List<Comment> input, bool saveNow = true);
        Task<Comment> GetCommentByIdAsync(long id);
        Task<List<Comment>> GetCommentByIdAsync(long[] ids);
        List<Comment> SearchCommentAsync(string name, string text);
        Task<bool> SetLabelComment(SetLabelCommentParam param);
        Task<bool> EditComment(long commentId, string cBodyClean);
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

        #region Auth
        Task<bool> Auth(string username, string password);
        #endregion
    }
}
