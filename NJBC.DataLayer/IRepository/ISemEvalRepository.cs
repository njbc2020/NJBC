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

         Task<OrgQuestion> GetByIdAsyncOrgQuestion(int ids);

    }
}
