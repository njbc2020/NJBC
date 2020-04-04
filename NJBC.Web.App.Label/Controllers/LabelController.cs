using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NJBC.DataLayer.IRepository;
using NJBC.DataLayer.Models;
using NJBC.DataLayer.Repository;
using NJBC.Models.DTO.Web;

namespace NJBC.Web.App.Label.Controllers
{
    public class LabelController : Controller
    {
        private readonly ISemEvalRepository SemEvalRepository;
        public LabelController(ISemEvalRepository SemEvalRepository)
        {
            this.SemEvalRepository = SemEvalRepository;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            ViewBag.UserId = id;
            
            var s = SemEvalRepository.GetActiveQuestion(id).Result;

            return View(s);
        }

        [HttpPost]
        public IActionResult SetLabel([FromBody]SetLabelCommentParam param)
        {
            var s = SemEvalRepository.SetLabelComment(param).Result;
            if (!s)
                return BadRequest();
            return Json(new { Message = "ثبت شد" });
        }
    }
}