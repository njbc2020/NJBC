using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NJBC.DataLayer.IRepository;
using Microsoft.AspNetCore.Mvc;
using NJBC.DataLayer.Models.Semeval2015;

namespace NJBC.Web.App.Label.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISemEvalRepository SemEvalRepository;
        public AdminController(ISemEvalRepository SemEvalRepository)
        {
            this.SemEvalRepository = SemEvalRepository;
        }

        public IActionResult QuestionsToken()
        {
            ViewBag.token = token;
            return View();
        }

        public IActionResult Questions(string id)
        {
            if (id == token)
            {
                var questions = SemEvalRepository.GetQuestionList(50).Result;
                return View(questions);
            }
            return Redirect("/");
        }

        [HttpPost]
        public IActionResult RejectQuestion(int id)
        {
            var s = SemEvalRepository.RejectQuestion(id).Result;
            if (!s)
                return BadRequest();
            return Ok("ok");
        }

        [HttpPost]
        public IActionResult ActiveQuestion(int id)
        {
            var s = SemEvalRepository.ActiveQuestion(id).Result;
            if (!s)
                return BadRequest();
            return Ok("ok");
        }

        private string token
        {
            get
            {
                int hour= DateTime.Now.Hour;

                if (hour % 2 == 0)
                    hour *= 13;
                else
                    hour *= 15;

                return (DateTime.Now.Day * DateTime.Now.Month * hour + hour).ToString();
            }
        }
    }
}