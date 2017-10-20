using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using quido.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace quido.Controllers
{
    public class adminController : Controller
    {
        public class AdminDetail
        {
            public long activeUsersCount { get; set; }
            public long activeMalesCount { get; set; }
            public long activeFemalesCount { get; set; }
            public int todayUsersCount { get; set; }
            public int deletedUsersCount { get; set; }
            public List<UsersLastMonthCount> usersLastMonth { get; set; }
            public List<QuestionsLastMonthCount> questionsLastMonth { get; set; }


            public long activeQuestionsCount { get; set; }
            public int todayQuestionsCount { get; set; }
            public string mostPopularCategory { get; set; }

            public long activeVotesCount { get; set; }
        }

        public class UsersAdmin
        {
            public Guid userId { get; set; }
            public string  Username { get; set; }
            public string email { get; set; }
            public string gender { get; set; }
            public string nationality { get; set; }
            public int yearOfBirth { get; set; }
            public DateTime registrationTime { get; set; }
            public int questionCount { get; set; }
            public int reportsCount { get; set; }
            public bool isInAdminRole { get; set; }

        }
       
        public class QuestionsAdmin
        {
            public Guid questionId { get; set; }
            public string questionText { get; set; }
            public string AuthorName { get; set; }
            public long votesCount { get; set; }
            public long reportsCount { get; set; }


        }

        public class ReportsAdmin
        {
            public Guid reportId { get; set; }
            public Guid questionId { get; set; }
            public string questionText { get; set; }
            public string reason { get; set; }

            public string contact { get; set; }
             public DateTime reportCreated { get; set; }

        }

        public class UsersLastMonthCount
        {
            public string date { get; set; }
            public int count { get; set; }
        }

        public class QuestionsLastMonthCount
        {
            public string date { get; set; }
            public int count { get; set; }
        }

        // GET: admin
        public ActionResult Index()
        {
            var db = new ApplicationDbContext();
            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.Find(currentUserId);
            if (!User.IsInRole("AdminRole"))
            {
                return RedirectToAction("index", "Home");
            }
            
            AdminDetail ad = new AdminDetail();
            var d = DateTime.Now;

            ad.usersLastMonth = new List<UsersLastMonthCount>();
            ad.questionsLastMonth = new List<QuestionsLastMonthCount>();

            ad.activeUsersCount = db.Users.Where(u => u.isDeleted == false).Count();
            ad.activeFemalesCount = db.Users.Where(u => u.isDeleted == false && u.Sex=="female").Count();
            ad.activeMalesCount = db.Users.Where(u => u.isDeleted == false && u.Sex == "male").Count();
            ad.todayUsersCount = db.Users.Where(u => u.isDeleted == false &&
                                                u.RegistrationDate.Year == d.Year &&
                                                u.RegistrationDate.Month == d.Month &&
                                                u.RegistrationDate.Day == d.Day).Count();
            for(int i=0;i<30;i++)
            {
                var date = d.ToString("dd.MM.yyyy");
                var count = db.Users.Where(u => u.isDeleted == false &&
                                                u.RegistrationDate.Year == d.Year &&
                                                u.RegistrationDate.Month == d.Month &&
                                                u.RegistrationDate.Day == d.Day).Count();
                ad.usersLastMonth.Add(new UsersLastMonthCount() { date = date, count = count });
                d= d.AddDays(-1);
            }            
            ad.usersLastMonth = ad.usersLastMonth.OrderBy(_ord => DateTime.ParseExact( _ord.date, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture)).ToList();

            d = DateTime.Now;
            ad.activeQuestionsCount = db.questions.Where(q => q.IsDeleted == false).Count();
            ad.todayQuestionsCount = db.questions.Where(u => u.IsDeleted == false &&
                                                u.QuestionCreated.Year == d.Year &&
                                                u.QuestionCreated.Month == d.Month &&
                                                u.QuestionCreated.Day == d.Day).Count();

            for (int i = 0; i < 30; i++)
            {
                var date = d.ToString("dd.MM.yyyy");
                var count = db.questions.Where(u => u.IsDeleted == false &&
                                                u.QuestionCreated.Year == d.Year &&
                                                u.QuestionCreated.Month == d.Month &&
                                                u.QuestionCreated.Day == d.Day).Count();
                ad.questionsLastMonth.Add(new QuestionsLastMonthCount() { date = date, count = count });
                d = d.AddDays(-1);
            }

            ad.questionsLastMonth = ad.questionsLastMonth.OrderBy(_ord => DateTime.ParseExact(_ord.date, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture)).ToList();


            ad.activeVotesCount = db.votes.Where(v => v.IsDeleted == false).Count();
            

            return View(ad);
            
        }

        public ActionResult users(int? page)
        {
            var db = new ApplicationDbContext();
            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.Find(currentUserId);
            if (!User.IsInRole("AdminRole"))
            {
                return RedirectToAction("index", "Home");
            }
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var list = new List<UsersAdmin>();
            foreach (ApplicationUser u in db.Users.Where(_u=>_u.isDeleted==false))
            {
                UsersAdmin ua = new UsersAdmin();
                ua.email = u.Email;
                ua.gender = u.Sex;
                ua.nationality = u.Nationality;
                ua.registrationTime = u.RegistrationDate;
                ua.userId = Guid.Parse(u.Id);
                ua.yearOfBirth = u.YearOfBirth;
                ua.Username = u.UserName;
                ua.questionCount = db.questions.Include("Author").Where(_q => _q.Author.Id == u.Id && _q.IsDeleted==false).Count();
                ua.reportsCount = db.reports.Include("ReportedQuestion").Where(_r => _r.ReportedQuestion.Author.Id == u.Id).Count();
                list.Add(ua);
                if (UserManager.IsInRole(ua.userId.ToString(), "AdminRole"))
                {
                    ua.isInAdminRole = true;
                }
                else
                {
                    ua.isInAdminRole = false;
                }
            }
            list = list.OrderBy(_l => _l.Username).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            PagedList<UsersAdmin> model = new PagedList<UsersAdmin>(list, pageNumber, pageSize);
            return View(model);

           
        }

        public ActionResult questions(int? page)
        {
            var db = new ApplicationDbContext();
            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.Find(currentUserId);
            if (!User.IsInRole("AdminRole"))
            {
                return RedirectToAction("index", "Home");
            }
            var list = new List<QuestionsAdmin>();
            foreach (Question q in db.questions.Include("Author").Where(_q=>_q.IsDeleted==false))
            {
                QuestionsAdmin qa = new QuestionsAdmin();
                qa.AuthorName = q.Author.UserName;
                qa.questionId = q.Id;
                if (q.QuestionText.Length > 40)
                {
                    var str = q.QuestionText.Substring(0, 35);
                    str += "...";
                    qa.questionText = str;
                }
                else
                {
                    qa.questionText = q.QuestionText;
                }
                var avc = 0;
                foreach (Answer _a in db.answers.Include("ConcreteQuestion").Where(a => a.IsDeleted == false).Where(a => a.ConcreteQuestion.Id == q.Id))
                {
                    
                    var count = (from v in db.votes
                                 where v.votedAnswer.Id == _a.Id
                                 select v).Count();

                    avc += count;
                }
                qa.votesCount = avc;
                qa.reportsCount= db.reports.Include("ReportedQuestion").Where(a => a.ReportedQuestion.Id == q.Id).Count();
                
                list.Add(qa);
            }
            list = list.OrderByDescending(a => a.votesCount).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            PagedList<QuestionsAdmin> model = new PagedList<QuestionsAdmin>(list, pageNumber, pageSize);
            return View(model);
            
        }

        public ActionResult reports(int? page)
        {
            var db = new ApplicationDbContext();
            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.Find(currentUserId);
            if (!User.IsInRole("AdminRole"))
            {
                return RedirectToAction("index", "Home");
            }

            var list = new List<ReportsAdmin>();
            foreach (Report r in db.reports.Include("ReportedQuestion"))
            {
                ReportsAdmin ra = new ReportsAdmin();
                ra.contact = r.ContactAddress;
                ra.questionId = r.ReportedQuestion.Id;

                ra.reason = r.ReportText;
                ra.reportCreated = r.ReportCreated;
                ra.reportId = r.Id;
                
                if (r.ReportedQuestion.QuestionText.Length > 25)
                {
                    var str = ra.questionText.Substring(0, 20);
                    str += "...";
                    ra.questionText = str;
                }
                else
                {
                    ra.questionText = r.ReportedQuestion.QuestionText; 
                }
                
                list.Add(ra);
            }
            list = list.OrderByDescending(a => a.reportCreated).ToList();
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            PagedList<ReportsAdmin> model = new PagedList<ReportsAdmin>(list, pageNumber, pageSize);
            return View(model);
        }

        public ActionResult logs(int? page)
        {
            return View();
        }


        [HttpPost]
        public JsonResult removeUser(Guid? idUser)
        {
            var db = new ApplicationDbContext();
            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.Find(currentUserId);
            if (!User.IsInRole("AdminRole"))
            {
                return Json(new { success = false, responseText = "Nemate prava pro smazani" }, JsonRequestBehavior.AllowGet);
            }
            if (idUser == null)
                return Json(new { success = false, responseText = "Uživatel neexistuje" }, JsonRequestBehavior.AllowGet);

            var userToDelete = db.Users.Where(_u => _u.Id == idUser.ToString()).FirstOrDefault();
            if (userToDelete == null)
                return Json(new { success = false, responseText = "Uživatel neexistuje" }, JsonRequestBehavior.AllowGet);

            userToDelete.isDeleted = true;

          
            
            //vymazat vsetko uzivatelove
            try
            {
                foreach (Question q in db.questions.Include("Author").Where(_q => _q.Author.Id == idUser.ToString()))
                {
                  
                    foreach (Answer a in db.answers.Include("ConcreteQuestion").Where(_a => _a.ConcreteQuestion.Id == q.Id))
                    {
                            foreach (Vote v in db.votes.Include("votedAnswer").Where(_v=>_v.votedAnswer.Id==a.Id))
                            {                                
                                v.IsDeleted = true;                                
                            }
                            a.IsDeleted = true;                        
                    }
                    q.IsDeleted = true;
                    
                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, responseText = "uživatel byl uspešne smazan" }, JsonRequestBehavior.AllowGet);

        }

    }
}