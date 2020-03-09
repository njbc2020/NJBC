using NJBC.DataLayer.IRepository;
using NJBC.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NJBC.DataLayer.Repository
{
    public class SemEvalRepository : ISemEvalRepository
    {
        private readonly NJBC_DBContext dBContext;
        public SemEvalRepository(NJBC_DBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task AddOrgQuestion(OrgQuestion input, bool saveNow = true)
        {
           await  dBContext.OrgQuestion.AddAsync(input);

            if (saveNow)
                await dBContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual Task<OrgQuestion> GetByIdAsyncOrgQuestion(int  ids)
        {
            return  dBContext.OrgQuestion.FindAsync(ids);
        }



    }
}
