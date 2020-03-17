using NJBC.DataLayer.IRepository;
using NJBC.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

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

        public List<OrgQuestion> OrgQuestionhAsync(string name, string subject)
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