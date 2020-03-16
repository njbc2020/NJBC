using NJBC.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NJBC.DataLayer.IRepository
{
    public interface ISemEvalRepository
    {
        Task AddOrgQuestion(OrgQuestion input, bool saveNow = true);
        Task AddOrgQuestion(List<OrgQuestion> input, bool saveNow = true);
        Task<OrgQuestion> GetOrgQuestionByIdAsync(int id);
        Task<List<OrgQuestion>> GetOrgQuestionByIdAsync(int[] ids);
        List<OrgQuestion> OrgQuestionhAsync(string name, string subject);

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

        
    }
}
