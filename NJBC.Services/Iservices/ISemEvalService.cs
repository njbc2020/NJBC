using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NJBC.Services.Iservices
{
    public interface ISemEvalService
    {
        Task Add(NJBC.DataLayer.Models.OrgQuestion input, bool saveNow = true);
    }
}
