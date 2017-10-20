using PagedList;
using quido.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static quido.Controllers.questionController;

namespace quido.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? page)
        {
            var db = new ApplicationDbContext();
            var allQuestions = new List<AllQuestionModel>();


            try
            {

                foreach (Question q in db.questions.Include("QuestionCat").Include("Author").Where(q => q.IsDeleted == false))
                {

                    int count = 0;
                    foreach (Answer _a in db.answers.Include("ConcreteQuestion").Where(a => a.IsDeleted == false))
                    {
                        if (_a.ConcreteQuestion.Id == q.Id)
                        {
                            var _c = (from v in db.votes
                                      where v.votedAnswer.Id == _a.Id
                                      select v).Count();

                            count += _c;
                        }

                    }
                    var cat = q.QuestionCat;
                    var questionCat = db.questionCategories.FirstOrDefault(a => a.Id == cat.Id);
                    var aqm = new AllQuestionModel()
                    {
                        questionId = q.Id,
                        questionAuthorUserName = q.Author.UserName,
                        questionAuthorId = Guid.Parse(q.Author.Id),
                        questionCreated = timeSpent(q.QuestionCreated),
                        DTquestionCreated = q.QuestionCreated,
                        questionDescription = q.Description,
                        questionCategory = questionCat.Name,
                        votesCount = count
                    };
                    if(q.QuestionText.Length>40)
                    {
                        var str = q.QuestionText.Substring(0, 35);
                        str += "...";
                        aqm.questionText = str;
                    }
                    else
                    {
                        aqm.questionText = q.QuestionText;
                    }
                    
                    allQuestions.Add(aqm);
                    //zoradenie podla najnovšieho
                    allQuestions = allQuestions.OrderByDescending(a => a.DTquestionCreated).ToList();
                }

            }
            catch (Exception e)
            {

                //return Json(new { success = false });
            }
            ViewBag.resultsCount = allQuestions.Count;
            int pageNumber = (page ?? 1);
            int pageSize = 16;
            PagedList<AllQuestionModel> model = new PagedList<AllQuestionModel>(allQuestions, pageNumber, pageSize);
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Cookies()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public string timeSpent(DateTime dt)
        {
            string textTime = "";
            var currentDate = DateTime.Now;
            var datetime = dt;
            TimeSpan ts = currentDate - datetime;
            //alert(datetime);
            var diff = ts.TotalMilliseconds / 1000;

            var day_diff = Math.Floor(diff / 86400);
            if (diff < 0)
            {
                textTime = datetime.ToString("d.M.yyyy");
            }
            else if (diff < 60)
            {
                textTime = "Právě teď";
            }
            else if (diff < 120)
            {
                textTime = "Před minutou";
            }
            else if (diff < 3600)
            {
                textTime = "Před " + Math.Floor(diff / 60) + " minutami";
            }
            else if (diff < 7200)
            {
                textTime = "Před 1 hodinou";
            }
            else if (diff < 86400)
            {
                textTime = "Před " + Math.Floor(diff / 3600) + " hodinami";
            }
            else if (day_diff == 1)
            {
                textTime = "Včera";
            }
            else if (day_diff < 7)
            {
                textTime = "Před " + day_diff + " dny";
            }
            else if (day_diff < 14)
            {
                textTime = "Před týdnem";
            }
            else if (day_diff < 31)
            {
                textTime = "Před " + Math.Ceiling(day_diff / 7) + " týdny";
            }
            else if (day_diff >= 31)
            {
                textTime = datetime.ToString("d.M.yyyy");
            }
            return textTime;
        }
    }

    
}