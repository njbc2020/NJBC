using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NJBC.DataLayer.IRepository;
using NJBC.DataLayer.Models;
using NJBC.DataLayer.Repository;

namespace NJBC.Web.App.Label.Controllers
{
    public class LabelController : Controller
    {
        private readonly ISemEvalRepository SemEvalRepository;
        public LabelController(ISemEvalRepository SemEvalRepository)
        {
            this.SemEvalRepository = SemEvalRepository;
        }

        public IActionResult Index(string id)
        {
            var s = SemEvalRepository.SearchRelQuestionAsync().Result.FirstOrDefault();

            var rng = new Random();
            return View();
        }
    }
}