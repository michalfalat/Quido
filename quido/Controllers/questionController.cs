using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using quido.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Xml;
using System.Net;

namespace quido.Controllers
{
    public class questionController : Controller
    {

        public class AddQuestionModel
        {
            public string textQuestion { get; set; }
            public string descriptionQuestion { get; set; }
            public string questionCategory { get; set; }
            public string questionType { get; set; }
            public string [] answers { get; set; }

        }

        public class SendReportModel
        {
            public Guid questionId { get; set; }
            public string reason { get; set; }
            public string contact { get; set; }

        }

        public class UserQuestionModel
        {
            //public Question  question { get; set; }
            public Guid questionId { get; set; }
            public string questionText { get; set; }
            public string questionDescription { get; set; }
            public string questionCategory { get; set; }
            public string questionCreated { get; set; }
            public DateTime DTquestionCreated { get; set; }
            public int votesCount { get; set; }

        }


        public class AllQuestionModel
        {
            //public Question  question { get; set; }
            public Guid questionId { get; set; }
            public string questionAuthorUserName { get; set; }
            public Guid questionAuthorId { get; set; }
            public string questionText { get; set; }
            public string questionDescription { get; set; }
            public string questionCategory { get; set; }
            public string questionCreated { get; set; }
            public DateTime DTquestionCreated { get; set; }
            public int votesCount { get; set; }

        }


        public class DetailQuestionModel
        {
           
            public Guid questionId { get; set; }
            public string questionText { get; set; }
            public string questionDescription { get; set; }
            public string questionCategory { get; set; }
            public DateTime questionCreated { get; set; }
            public string questionType { get; set; }

            public string questionAuthorUserName { get; set; }
            public Guid questionAuthorId { get; set; }
            
            public ICollection<VotesCount> questionVotes { get; set; }
            public long allVotesCount { get; set; }
            public bool hasVoted { get; set; }

            public bool haveUserInfo { get; set; }
            public int userYearOfBirth { get; set; }
            public string userGender { get; set; }
            public string userCountryCode { get; set; }
            public List<int> myYears = Years.years.Keys.ToList();

            public ICollection<GenderCount> genderCounts { get; set; }
            public ICollection<AgeCount> ageCounts { get; set; }
            public ICollection<CountryCount> countryCounts { get; set; }



        }
        public class shortDetailQuestionModel
        {

            public Guid questionId { get; set; }
            public ICollection<VotesCount> questionVotes { get; set; }
            public long allVotesCount { get; set; }

            public ICollection<GenderCount> genderCounts { get; set; }
            public ICollection<AgeCount> ageCounts { get; set; }
            public ICollection<CountryCount> countryCounts { get; set; }

        }

        public class DetailUserQuestionModel
        {

            public Guid questionId { get; set; }
            public string questionText { get; set; }
            public string questionDescription { get; set; }
            public string questionCategory { get; set; }
            public DateTime questionCreated { get; set; }
            public string questionType { get; set; }          

           
            public List<int> myYears = Years.years.Keys.ToList();

            public ICollection<GenderCount> genderCounts { get; set; }
            public ICollection<AgeCount> ageCounts { get; set; }
            public ICollection<CountryCount> countryCounts { get; set; }


            public ICollection<VotesCount> questionVotes { get; set; }
            public long allVotesCount { get; set; }

        }


        public class VotesCount
        {
            public Answer questionAnswer { get; set; }
            public long questionAnswerCount { get; set; }
        }

        public class GenderCount
        {
            public string gender { get; set; }
            public long genderCount { get; set; }
        }

        public class AgeCount
        {
            public string age { get; set; }
            public long ageCount { get; set; }
        }

        public class CountryCount
        {
            public string country { get; set; }
            public long countryCount { get; set; }
        }

        public class SearchResultsModel
        {
            public List<AllQuestionModel> allQuestions {get;set;}
            public string searchedText { get; set; }
             public long resultsCount { get; set; }

        }



            // GET: question
            public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult getQuestionCategories()
        {
            var db = new ApplicationDbContext();            
                

            ICollection<QuestionCategory> myQuestionCats = db.questionCategories.ToList();

            return Json(myQuestionCats, JsonRequestBehavior.AllowGet);
        }

      

        [HttpGet]
        public JsonResult getUserQuestions()
        {
            var db = new ApplicationDbContext();
            var userQuestions = new List<UserQuestionModel>();
            var currentUserId = User.Identity.GetUserId();
            // nen prihlaseny
            if (currentUserId == null)
                return Json(new { success = false, responseText = "Nejste přihlášen" }, JsonRequestBehavior.AllowGet);

            var currentUser = db.Users.Find(currentUserId);
            

            try
            { 

                foreach (Question q in db.questions.Include("QuestionCat").Include("Author").Where(q=>q.IsDeleted==false))
                {
                    if (q.Author.Id == currentUserId )
                    {
                        int count = 0;
                        foreach (Answer _a in db.answers.Include("ConcreteQuestion"))
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
                        userQuestions.Add(new UserQuestionModel()
                        {
                            questionId = q.Id,
                            questionCreated = timeSpent( q.QuestionCreated),
                            DTquestionCreated = q.QuestionCreated,
                            questionDescription = q.Description,
                            questionText = q.QuestionText,
                            questionCategory= questionCat.Name,
                            votesCount = count
                        });
                   
                    }
                }

                userQuestions = userQuestions.OrderByDescending(a => a.DTquestionCreated).ToList();
            }
            catch(Exception e)
            {

                return Json(new { success = false, responseText = e.Message +" INNER: " +e.InnerException }, JsonRequestBehavior.AllowGet);
            }

            var list = JsonConvert.SerializeObject(userQuestions,
    Newtonsoft.Json.Formatting.None,
    new JsonSerializerSettings()
    {
        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    });

            return Json(new { list, success=true }, JsonRequestBehavior.AllowGet);
        }

                

           
        

        
        [HttpPost]
        public JsonResult addQuestion(AddQuestionModel aqm)
        {
            var db = new ApplicationDbContext();
            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.Find(currentUserId);
            Question q = new Question()
            {
                QuestionText = aqm.textQuestion,
                Description = aqm.descriptionQuestion,
                QuestionCreated = DateTime.Now,
                QuestionType = aqm.questionType,
                Author = currentUser
            };
            foreach (QuestionCategory qc in db.questionCategories)
            {
                if (qc.Name == aqm.questionCategory)
                    q.QuestionCat = qc;
            }

            db.questions.Add(q);
            db.SaveChanges();

            var i = 0;
            if (aqm.answers != null)
            {
                foreach (String s in aqm.answers)
                {
                    Answer a1 = new Answer()
                    {
                        AnswerText = s,
                        ConcreteQuestion = q,
                        AnswerId = i
                    };
                    i++;
                    db.answers.Add(a1);
                    db.SaveChanges();
                }
            }
            

            return Json(new { success = true, responseText = "Otazka byla uložena" }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult removeQuestion(Guid? idQuestion)
        {
            if (idQuestion == null)
                return Json(new { success = false, responseText = "Otazka neexistuje" }, JsonRequestBehavior.AllowGet);

            var db = new ApplicationDbContext();
            var currentUserId = User.Identity.GetUserId();

            //nikto nieje pravdepodobne prihlaseny
            if (currentUserId == null)
                return Json(new { success = false, responseText = "Nejste přihlášen" }, JsonRequestBehavior.AllowGet);

            var currentUser = db.Users.Find(currentUserId);
            var questionToDelete = db.questions.Include("Author").FirstOrDefault(q => q.Id == idQuestion);

            //otazka nieje v databazi
            if(questionToDelete==null)
                return Json(new { success = false, responseText = "Otazka nenalezena" }, JsonRequestBehavior.AllowGet);

            //otazka s danym ID nieje  prihlaseneho autora
            if (questionToDelete.Author.Id != currentUser.Id && !User.IsInRole("AdminRole"))
                return Json(new { success = false, responseText = "Tohle neni vaše otázka!" }, JsonRequestBehavior.AllowGet);

            //vymazat otazku, vymazat odpovede a vymazat vsetky votes
            try
            {
                foreach (Answer a in db.answers.Include("ConcreteQuestion"))
                {
                    if (a.ConcreteQuestion.Id == questionToDelete.Id)
                    {
                        foreach (Vote v in db.votes.Include("votedAnswer"))
                        {
                            if (v.votedAnswer.Id == a.Id)
                            {
                                v.IsDeleted = true;
                            }
                        }
                        a.IsDeleted = true;
                    }
                }
                questionToDelete.IsDeleted = true;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                    return Json(new { success = false, responseText = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, responseText = "Otazka byla uspešne vymazana" }, JsonRequestBehavior.AllowGet);

        }




        [HttpGet]
        public JsonResult getAllQuestions()
        {
            var db = new ApplicationDbContext();
            var allQuestions = new List<AllQuestionModel>();
            try
            {
                foreach (Question q in db.questions.Include("QuestionCat").Where(q=>q.IsDeleted==false))
                {                    
                        int count = 0;
                        foreach (Answer _a in db.answers.Include("ConcreteQuestion").Where(a=>a.IsDeleted==false))
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
                    allQuestions.Add(new AllQuestionModel()
                    {
                        questionId = q.Id,
                        questionAuthorUserName = q.Author.UserName,
                        questionAuthorId = Guid.Parse(q.Author.Id),
                        questionCreated = timeSpent(q.QuestionCreated),
                        DTquestionCreated = q.QuestionCreated,
                        questionDescription = q.Description,
                            questionText = q.QuestionText,
                            questionCategory = questionCat.Name,
                            votesCount = count
                        });                        //zoradenie podla najnovšieho
                        allQuestions = allQuestions.OrderByDescending(a => a.DTquestionCreated).ToList();
                    }                
            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }

            var list = JsonConvert.SerializeObject(allQuestions,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings()
                {
                     ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                 });

             return Json(list, JsonRequestBehavior.AllowGet);
            
        }





        public ActionResult detail(Guid? id)
        {
            if (id == null) 
            {
                return RedirectToAction("index", "Home");

            }
            var idQuestion = id;
            DetailQuestionModel detailOfQuestion;
            ICollection<VotesCount> vc = new List<VotesCount>();
            ICollection<GenderCount> gc = new List<GenderCount>();
            long avc = 0;
            bool hasVoted = false;
            bool getVotedStateByUser = true;
            var currentUserId = User.Identity.GetUserId();

            ApplicationUser usr = null; 
            var db = new ApplicationDbContext();
            try
            {
                //ak je nejaky uzivatel prihlaseny , bude hladat ci hlasoval  podla toho
                //ak nie je prihlaseny bude hladat podla MAC adresy
                if (currentUserId != null)
                {
                    getVotedStateByUser = true;
                    usr = db.Users.Find(currentUserId);
                }
                else
                {
                    getVotedStateByUser = false;
                }

                var q = db.questions.Include("QuestionCat").Include("Author").Where(qq => qq.IsDeleted == false).FirstOrDefault(a => a.Id == idQuestion);
                if (q == null || q.IsDeleted == true)
                {
                    return RedirectToAction("index", "Home");

                }
                
                var genders = db.votes.Include("Author").Include("votedAnswer").Where(v => v.votedAnswer.ConcreteQuestion.Id == idQuestion).GroupBy(v => v.Gender).Select(gg => new GenderCount() { gender = gg.Key, genderCount = gg.Count() }).ToList();
                foreach(GenderCount _gc in genders)
                {
                    _gc.gender =  getGenderName(_gc.gender);
                }
                var ages = db.votes.Include("Author").Include("votedAnswer").Where(v => v.votedAnswer.ConcreteQuestion.Id == idQuestion).GroupBy(v => v.YearOfBirth).Select(gg => new AgeCount() { age= (DateTime.Now.Year - gg.Key).ToString(), ageCount = gg.Count() }).ToList();

                var countries = db.votes.Include("Author").Include("votedAnswer").Where(v => v.votedAnswer.ConcreteQuestion.Id == idQuestion).GroupBy(v => v.CountryCode).Select(gg => new CountryCount() { country = gg.Key, countryCount = gg.Count() }).ToList();
                foreach (CountryCount _cc in countries)
                {
                    _cc.country = getCountryName(_cc.country);
                }


                //}
                //najdenie odpovedi ku otazke a zistenie poctu hlasov
                foreach (Answer _a in db.answers.Include("ConcreteQuestion").Where(a => a.IsDeleted == false).Where(a => a.ConcreteQuestion.Id == idQuestion))
                {
                    if (getVotedStateByUser == true)
                    {
                        Vote _v = db.votes.Include("Author").Include("votedAnswer").Where(v => v.Author.Id == currentUserId && v.votedAnswer.Id == _a.Id).FirstOrDefault();
                        if (_v != null)
                        {
                            hasVoted = true;
                        }

                    }                    

                    var count = (from v in db.votes
                                 where v.votedAnswer.Id == _a.Id
                                 select v).Count();

                    avc += count;

                    //aby nezahrnalo aj otazku znovu
                    var finalAnswer = new Answer() { Id = _a.Id, AnswerText = _a.AnswerText ,AnswerId = _a.AnswerId};
                    vc.Add(new VotesCount() { questionAnswer = finalAnswer, questionAnswerCount = count });            


                }

                //zorad iba ak su tam ciselne hodnoty
                if (q.QuestionType == "addChoiceNumber")
                {
                    vc = vc.OrderBy(x => int.Parse(x.questionAnswer.AnswerText)).ToList();
                }
                else
                {
                    vc = vc.OrderBy(x => x.questionAnswer.AnswerId).ToList();
                }
                var cat = q.QuestionCat;
                var questionCat = db.questionCategories.FirstOrDefault(a => a.Id == cat.Id);
                detailOfQuestion = new DetailQuestionModel()
                {
                    questionId = q.Id,
                    questionAuthorId = Guid.Parse(q.Author.Id),
                    questionAuthorUserName = q.Author.UserName,
                    questionCreated = q.QuestionCreated,
                    questionDescription = q.Description,
                    questionText = q.QuestionText,
                    questionCategory = questionCat.Name,
                    questionType = q.QuestionType,
                    questionVotes = vc,
                    allVotesCount = avc,
                    ageCounts= ages,
                    countryCounts= countries,
                    genderCounts=genders

                };
                ViewBag.pageTitle = q.QuestionText;
                //ak je prihlaseny uzivatel
                if (getVotedStateByUser == true)
                {
                    detailOfQuestion.userGender = usr.Sex;
                    detailOfQuestion.userYearOfBirth = usr.YearOfBirth;
                    detailOfQuestion.haveUserInfo = true;
                    detailOfQuestion.userCountryCode = usr.Nationality;
                }

                //ak nieje, treba skontrolovat cookies
                else
                {
                    //kontrola ci sa id otazky nachadza v cookies
                    var cookieHasVoted = Request.Cookies[q.Id.ToString()];
                    if (cookieHasVoted == null)
                    {
                        hasVoted = false;
                    }
                    else
                    {
                        hasVoted = true;
                    }

                    var haveUserInfo = true;
                    var cookieGender = Request.Cookies["userInfo_gender"];
                    if (cookieGender != null)
                    {
                        detailOfQuestion.userGender = cookieGender.Value;
                    }
                    else
                    {
                        haveUserInfo = false;
                    }


                    var cookieYearOfBirth = Request.Cookies["userInfo_yearOfBirth"];
                    if (cookieYearOfBirth != null)
                    {
                        detailOfQuestion.userYearOfBirth = int.Parse(cookieYearOfBirth.Value);
                    }
                    else
                    {
                        haveUserInfo = false;
                    }

                    if(haveUserInfo==true)
                    {
                        detailOfQuestion.haveUserInfo = true;
                    }
                    else
                        detailOfQuestion.haveUserInfo = false;

                    //zistit stat podla IP
                    var countryISO = getStateFromCurrentIpAddress();
                    if(countryISO=="SK" || countryISO=="CZ")
                    {
                        detailOfQuestion.userCountryCode =  countryISO;
                    }
                    else
                    {
                        detailOfQuestion.userCountryCode = "Nedefinovano";
                    }
                    

                }

                detailOfQuestion.hasVoted = hasVoted;

                return View(detailOfQuestion);
            }
            catch (Exception e)
            {
                //presmerovanie na stranku ktora neexistuje
                //return Json(new { error = true });
                return RedirectToAction("index", "Home");
            }

        }


        public JsonResult getNewQuestionData(Guid? id)
        {
            if (id == null)
            {
                return Json(new { success = false, responseText = "Otazka neexistuje" }, JsonRequestBehavior.AllowGet);


            }
            var idQuestion = id;
            shortDetailQuestionModel shortDetailOfQuestion;
            ICollection<VotesCount> vc = new List<VotesCount>();
            long avc = 0;

            var db = new ApplicationDbContext();
            try
            {
                var q = db.questions.FirstOrDefault(a => a.Id == idQuestion);
                if (q == null || q.IsDeleted == true)
                {
                    return Json(new { success = false, responseText = "Otazka neexistuje" }, JsonRequestBehavior.AllowGet);


                }
                //najdenie odpovedi ku otazke a zistenie poctu hlasov
                foreach (Answer _a in db.answers.Include("ConcreteQuestion"))
                {
                    if (_a.ConcreteQuestion.Id == idQuestion)
                    {
                        var count = (from v in db.votes
                                     where v.votedAnswer.Id == _a.Id
                                     select v).Count();

                        avc += count;

                        //aby nezahrnalo aj otazku znovu
                        var finalAnswer = new Answer() { Id = _a.Id, AnswerText = _a.AnswerText };
                        vc.Add(new VotesCount() { questionAnswer = finalAnswer, questionAnswerCount = count });

                    }

                }

                var genders = db.votes.Include("Author").Include("votedAnswer").Where(v => v.votedAnswer.ConcreteQuestion.Id == idQuestion).GroupBy(v => v.Gender).Select(gg => new GenderCount() { gender = gg.Key, genderCount = gg.Count() }).ToList();
                foreach (GenderCount _gc in genders)
                {
                    _gc.gender = getGenderName(_gc.gender);
                }
                var ages = db.votes.Include("Author").Include("votedAnswer").Where(v => v.votedAnswer.ConcreteQuestion.Id == idQuestion).GroupBy(v => v.YearOfBirth).Select(gg => new AgeCount() { age = (DateTime.Now.Year - gg.Key).ToString(), ageCount = gg.Count() }).ToList();

                var countries = db.votes.Include("Author").Include("votedAnswer").Where(v => v.votedAnswer.ConcreteQuestion.Id == idQuestion).GroupBy(v => v.CountryCode).Select(gg => new CountryCount() { country = gg.Key, countryCount = gg.Count() }).ToList();
                foreach (CountryCount _cc in countries)
                {
                    _cc.country = getCountryName(_cc.country);
                }

                //zorad iba ak su tam ciselne hodnoty
                if (q.QuestionType == "addChoiceNumber")
                {
                    vc = vc.OrderBy(x => int.Parse(x.questionAnswer.AnswerText)).ToList();
                }
                shortDetailOfQuestion = new shortDetailQuestionModel()
                {
                    questionId = q.Id,
                    questionVotes = vc,
                    allVotesCount = avc,
                    ageCounts = ages,
                    countryCounts = countries,
                    genderCounts = genders

                };

               // return View(detailOfQuestion);
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText =  e.Message }, JsonRequestBehavior.AllowGet);

            }
            var list = JsonConvert.SerializeObject(shortDetailOfQuestion,
               Newtonsoft.Json.Formatting.None,
               new JsonSerializerSettings()
               {
                   ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
               });

            return Json(list, JsonRequestBehavior.AllowGet);

        }

        public JsonResult getFilteredData(Guid? id, string gender, string country)
        {
            if (id == null)
            {
                return Json(new { success = false, responseText = "Otazka neexistuje" }, JsonRequestBehavior.AllowGet);


            }
            var idQuestion = id;
            shortDetailQuestionModel shortDetailOfQuestion;
            ICollection<VotesCount> vc = new List<VotesCount>();
            long avc = 0;

            var db = new ApplicationDbContext();
            try
            {
                var q = db.questions.FirstOrDefault(a => a.Id == idQuestion);
                if (q == null || q.IsDeleted == true)
                {
                    return Json(new { success = false, responseText = "Otazka neexistuje" }, JsonRequestBehavior.AllowGet);


                }
                var count = 0;
                //najdenie odpovedi ku otazke a zistenie poctu hlasov
                foreach (Answer _a in db.answers.Include("ConcreteQuestion"))
                {
                    if (_a.ConcreteQuestion.Id == idQuestion)
                    {
                        if(gender=="0" && country == "0") { 
                           count = (from v in db.votes
                                     where v.votedAnswer.Id == _a.Id
                                     select v).Count();   
                        }
                        else if(gender != "0" && country != "0")
                        {
                            count = db.votes.Where(_v => _v.votedAnswer.Id == _a.Id && _v.Gender == gender && _v.CountryCode == country).Count();
                        }
                        else if (gender != "0")
                        {

                            count = db.votes.Where(_v => _v.votedAnswer.Id == _a.Id && _v.Gender == gender).Count();

                        }
                        else if (country != "0")
                        {

                            count = db.votes.Where(_v => _v.votedAnswer.Id == _a.Id && (_v.CountryCode == country )).Count();
                        }
                       
                        avc += count;
                        //aby nezahrnalo aj otazku znovu
                        var finalAnswer = new Answer() { Id = _a.Id, AnswerText = _a.AnswerText };
                        vc.Add(new VotesCount() { questionAnswer = finalAnswer, questionAnswerCount = count });

                    }

                }


               
                shortDetailOfQuestion = new shortDetailQuestionModel()
                {
                    questionId = q.Id,
                    questionVotes = vc,
                    allVotesCount = avc

                };

                // return View(detailOfQuestion);
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = e.Message }, JsonRequestBehavior.AllowGet);

            }
            var list = JsonConvert.SerializeObject(shortDetailOfQuestion,
               Newtonsoft.Json.Formatting.None,
               new JsonSerializerSettings()
               {
                   ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
               });

            return Json(list, JsonRequestBehavior.AllowGet);

        }


        public ActionResult userDetail(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("index", "Home");

            }
            var idQuestion = id;
            DetailUserQuestionModel detailOfUserQuestion;
            ICollection<VotesCount> vc = new List<VotesCount>();
            long avc = 0;

            var db = new ApplicationDbContext();
            try
            {
                var q = db.questions.Include("QuestionCat").Include("Author").FirstOrDefault(a => a.Id == idQuestion);
                if (q == null || q.IsDeleted == true)
                {
                    return RedirectToAction("index", "Home");

                }

                var genders = db.votes.Include("Author").Include("votedAnswer").Where(v => v.votedAnswer.ConcreteQuestion.Id == idQuestion).GroupBy(v => v.Gender).Select(gg => new GenderCount() { gender = gg.Key, genderCount = gg.Count() }).ToList();
                foreach (GenderCount _gc in genders)
                {
                    _gc.gender = getGenderName(_gc.gender);
                }
                var ages = db.votes.Include("Author").Include("votedAnswer").Where(v => v.votedAnswer.ConcreteQuestion.Id == idQuestion).GroupBy(v => v.YearOfBirth).Select(gg => new AgeCount() { age = (DateTime.Now.Year - gg.Key).ToString(), ageCount = gg.Count() }).ToList();

                var countries = db.votes.Include("Author").Include("votedAnswer").Where(v => v.votedAnswer.ConcreteQuestion.Id == idQuestion).GroupBy(v => v.CountryCode).Select(gg => new CountryCount() { country = gg.Key, countryCount = gg.Count() }).ToList();
                foreach (CountryCount _cc in countries)
                {
                    _cc.country = getCountryName(_cc.country);
                }
                //najdenie odpovedi ku otazke a zistenie poctu hlasov
                foreach (Answer _a in db.answers.Include("ConcreteQuestion").Where(a=>a.IsDeleted==false))
                {
                    if (_a.ConcreteQuestion.Id == idQuestion)
                    {
                        var count = (from v in db.votes
                                     where v.votedAnswer.Id == _a.Id
                                     select v).Count();

                        avc += count;

                        //aby nezahrnalo aj otazku znovu
                        var finalAnswer = new Answer() { Id = _a.Id, AnswerText = _a.AnswerText };
                        vc.Add(new VotesCount() { questionAnswer = finalAnswer, questionAnswerCount = count });

                    }

                }
                if (q.QuestionType == "addChoiceNumber")
                {
                    vc = vc.OrderBy(x => int.Parse(x.questionAnswer.AnswerText)).ToList();
                }

                var cat = q.QuestionCat;
                var questionCat = db.questionCategories.FirstOrDefault(a => a.Id == cat.Id);
                detailOfUserQuestion = new DetailUserQuestionModel()
                {
                    questionId = q.Id,
                    questionCreated =  q.QuestionCreated,
                    questionDescription = q.Description,
                    questionText = q.QuestionText,
                    questionCategory = questionCat.Name,
                    questionType = q.QuestionType,
                    questionVotes = vc,
                    allVotesCount = avc,
                    ageCounts = ages,
                    countryCounts = countries,
                    genderCounts = genders
                };
                ViewBag.pageTitle = q.QuestionText ;

                return View(detailOfUserQuestion);
            }
            catch (Exception e)
            {
                return RedirectToAction("index", "Home");
            }

        }








        [HttpPost]
        public JsonResult sendVote(Guid? idAnswer, string yearOfBirth, string gender, string countryCode)
        {
            Vote v;
            var db = new ApplicationDbContext();
            try
            {
                ApplicationUser user=null;
                var currentUserId = User.Identity.GetUserId();
                if(currentUserId!=null)
                    user = db.Users.Find(currentUserId);

                var answ = db.answers.Include("ConcreteQuestion").FirstOrDefault(a => a.Id == idAnswer);
                if(answ==null)
                    return Json(new { success = false ,responseText="Odpoved nebyla najdena"});

                v = new Vote()
                {
                    votedAnswer = answ,
                    MacAddress = "undefined",
                    VoteCreated = DateTime.Now                   
                };

                if (currentUserId != null)
                {
                    v.Author = user;
                    v.YearOfBirth = user.YearOfBirth;
                    v.Gender = user.Sex;
                    v.CountryCode = user.Nationality;

                    var currentQAnswered = user.questionsAnswered;
                    ++currentQAnswered;
                    user.questionsAnswered = currentQAnswered;

                }
                else
                {
                    var q = answ.ConcreteQuestion;
                    //vytvorenie resp prepisanie cookiesov
                    var cookieGender = new HttpCookie("userInfo_gender", gender);
                    cookieGender.Expires = DateTime.Now.AddDays(365);
                    var cookieYearOfBirth = new HttpCookie("userInfo_yearOfBirth", yearOfBirth);
                    cookieYearOfBirth.Expires = DateTime.Now.AddDays(365);
                    var cookieQuestion = new HttpCookie(q.Id.ToString(), q.Id.ToString());
                    cookieQuestion.Expires = DateTime.Now.AddDays(365);
                    Response.AppendCookie(cookieGender);
                    Response.AppendCookie(cookieYearOfBirth);
                    Response.AppendCookie(cookieQuestion);

                    v.YearOfBirth = int.Parse(yearOfBirth);
                    v.Gender = gender;
                    v.CountryCode = countryCode == "Nedefinovano" ? "undefined" : countryCode;
                }


                db.votes.Add(v);
                db.SaveChanges();
            }
            catch (Exception e)
            {

                return Json(new { success = false, responseText= e.Message});
            }

            

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult sendAnsweredVote(Guid? idQuestion, string number, string yearOfBirth, string gender, string countryCode)
        {
            Vote v;
            var db = new ApplicationDbContext();
            try
            {
                var q = db.questions.FirstOrDefault(qq => qq.Id == idQuestion);
                if (q == null)
                {
                    return Json(new { success = false });
                }

                ApplicationUser user = null;
                var currentUserId = User.Identity.GetUserId();
                v = new Vote()
                {
                    MacAddress = "undefined",
                    //Author = user,
                    VoteCreated = DateTime.Now
                };
                if (currentUserId != null)
                {
                    user = db.Users.Find(currentUserId);
                    v.Author = user;
                    v.YearOfBirth = user.YearOfBirth;
                    v.Gender = user.Sex;
                    v.CountryCode = user.Nationality;

                    var currentQAnswered = user.questionsAnswered;
                    ++currentQAnswered;
                    user.questionsAnswered = currentQAnswered;

                }
                else
                {
                    //vytvorenie cookiesov
                    var cookieGender = new HttpCookie("userInfo_gender", gender);
                    cookieGender.Expires = DateTime.Now.AddDays(365);
                    var cookieYearOfBirth = new HttpCookie("userInfo_yearOfBirth", yearOfBirth);
                    cookieYearOfBirth.Expires = DateTime.Now.AddDays(365);
                    var cookieQuestion = new HttpCookie(q.Id.ToString(), q.Id.ToString());
                    cookieQuestion.Expires = DateTime.Now.AddDays(365);                   
                    Response.AppendCookie(cookieGender);
                    Response.AppendCookie(cookieYearOfBirth);
                    Response.AppendCookie(cookieQuestion);

                    v.YearOfBirth = int.Parse(yearOfBirth);
                    v.Gender = gender;
                    v.CountryCode = countryCode=="Nedefinovano" ? "undefined": countryCode;
                }

               

                var answ = db.answers.Include("ConcreteQuestion").FirstOrDefault(a => a.ConcreteQuestion.Id == idQuestion && a.AnswerText == number);
                if (answ == null)
                {
                    Answer a = new Answer()
                    {
                        AnswerText = number,
                        ConcreteQuestion = q

                    };

                    db.answers.Add(a);
                    db.SaveChanges();
                    v.votedAnswer = a;
                }
                else
                {
                    v.votedAnswer = answ;
                }

                db.votes.Add(v);
                db.SaveChanges();
                
            }
            catch (Exception e)
            {

                return Json(new { success = false, responseText = e.Message });
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }



        public ActionResult search(string id, int? page)
        {
            
            if(id == null || id == "" )
            {
                return RedirectToAction("index", "Home");
            }
            string searchedText = id.ToLower();
            List<String> keywords = new List<String>();
            string word = "";
            for(int i = 0;i< searchedText.Length;i++)
            {
                if(searchedText[i]=='&')
                {
                    keywords.Add(word);
                    word = "";
                }
                else
                {
                    word += searchedText[i];
                }

            }
            keywords.Add(word);
            var db = new ApplicationDbContext();
            SearchResultsModel sr = new SearchResultsModel();
            sr.allQuestions = new List<AllQuestionModel>();

            List<Question> searchedQuestions = db.questions.Include("QuestionCat").Include("Author").Where(q=>q.IsDeleted==false).Where(q => keywords.All(k => q.QuestionText.ToLower().Contains(k))).ToList();
            sr.resultsCount = searchedQuestions.Count;
            //id preto aby sa zachovala velkost povodnych pismen
            sr.searchedText = id.Replace("&", " ");
            try
            {
                foreach (Question q in searchedQuestions)
                {
                    
                        int count = 0;
                        foreach (Answer _a in db.answers.Include("ConcreteQuestion"))
                        {
                            if (_a.ConcreteQuestion.Id == q.Id && _a.IsDeleted==false)
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
                            questionAuthorId=Guid.Parse(q.Author.Id),
                            questionCreated = timeSpent(q.QuestionCreated),
                            DTquestionCreated = q.QuestionCreated,
                            questionDescription = q.Description,                            
                            questionCategory = questionCat.Name,
                            votesCount = count
                        };
                    if (q.QuestionText.Length > 40)
                    {
                        var str = q.QuestionText.Substring(0, 35);
                        str += "...";
                        aqm.questionText = str;
                    }
                    else
                    {
                        aqm.questionText = q.QuestionText;
                    }
                    sr.allQuestions.Add(aqm);
                    //zoradenie podla najnovšieho
                    sr.allQuestions = sr.allQuestions.OrderByDescending(a => a.DTquestionCreated).ToList();
                    }
                
            }
            catch (Exception e)
            {

                return RedirectToAction("index", "Home");
            }
            ViewBag.searchedText = sr.searchedText;
            ViewBag.resultsCount = sr.resultsCount;
            int pageNumber = (page?? 1);
            int pageSize = 16;
            PagedList<AllQuestionModel> model = new PagedList<AllQuestionModel>(sr.allQuestions, pageNumber, pageSize);
            return View(model);
        }



        [HttpPost]
        public JsonResult sendReport(SendReportModel sr)
        {
            var db = new ApplicationDbContext();
            var q = db.questions.Where(_q => _q.Id == sr.questionId && _q.IsDeleted==false).FirstOrDefault();
            if(q==null)
            {
                return Json(new { success = false, responseText = "Otazka neexistuje." }, JsonRequestBehavior.AllowGet);
            }
            Report r = new Report()
            {
                ContactAddress  =  sr.contact,
                ReportCreated = DateTime.Now,
                ReportedQuestion = q,
                ReportReason = "Reported By User",
                ReportText= sr.reason   
            };           

            db.reports.Add(r);
            db.SaveChanges();      
            
            return Json(new { success = true, responseText = "Report byl odeslan." }, JsonRequestBehavior.AllowGet);

        }






        //
        //
        //HELPER METOHODS
        //
        //

        private string getCountryName(string country)
        {
            if (country == "SK")
                return "Slovensko";
            else if (country == "CZ")
                return "Česko";
            else
                return "Nedefinováno";
        }

        private string getGenderName(string g)
        {
            if (g == "male")
                return "Muž";
            else if (g == "female")
                return "Žena";
            else
                return "??";
        }

        private string getMacAddress()
        {
            string macAddresses = "";

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }


        private string getStateFromCurrentIpAddress()
        {
            string state = "";
            string ip = GetVisitorIPAddress();

            if (ip == "undefined")
                return "Nedefinovano";
            else
            {
                XmlDocument doc = new XmlDocument();
                string getdetails = "http://www.freegeoip.net/xml/" + ip;
                doc.Load(getdetails);
                XmlNodeList nodeLstCountry = doc.GetElementsByTagName("CountryCode");
                state = nodeLstCountry[0].InnerText;
                return state;
            }
        }


        private string GetVisitorIPAddress()
        {
            bool GetLan = false;
            string visitorIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (String.IsNullOrEmpty(visitorIPAddress))
                visitorIPAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(visitorIPAddress))
                visitorIPAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(visitorIPAddress) || visitorIPAddress.Trim() == "::1")
            {
                //GetLan = true;
                visitorIPAddress = "undefined";
            }

            if (GetLan)
            {
                if (string.IsNullOrEmpty(visitorIPAddress))
                {
                    //This is for Local(LAN) Connected ID Address
                    string stringHostName = Dns.GetHostName();
                    //Get Ip Host Entry
                    IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
                    //Get Ip Address From The Ip Host Entry Address List
                    IPAddress[] arrIpAddress = ipHostEntries.AddressList;

                    try
                    {
                        visitorIPAddress = arrIpAddress[arrIpAddress.Length - 2].ToString();
                    }
                    catch
                    {
                        try
                        {
                            visitorIPAddress = arrIpAddress[0].ToString();
                        }
                        catch
                        {
                            try
                            {
                                arrIpAddress = Dns.GetHostAddresses(stringHostName);
                                visitorIPAddress = arrIpAddress[0].ToString();
                            }
                            catch
                            {
                                visitorIPAddress = "127.0.0.1";
                            }
                        }
                    }
                }
            }
            return visitorIPAddress;
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

