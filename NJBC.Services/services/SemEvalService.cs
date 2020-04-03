using NJBC.DataLayer.IRepository;
//using NJBC.Models.SemEval;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NJBC.Services.Iservices;

namespace NJBC.Services.services
{

    public class SemEvalService : ISemEvalService
    {

        private readonly ISemEvalRepository SemEvalRepository;

        public SemEvalService()
        {
        }

        public SemEvalService(ISemEvalRepository SemEvalRepository)
        {
            this.SemEvalRepository = SemEvalRepository;
        }

        public async Task Add(NJBC.DataLayer.Models.OrgQuestion input, bool saveNow = true)
        {
            await SemEvalRepository.AddOrgQuestion(input , saveNow);
            
        }



    }
}
