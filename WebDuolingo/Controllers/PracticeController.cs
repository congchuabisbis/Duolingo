using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebDuolingo.DTO;
using WebDuolingo.Models;

namespace WebDuolingo.Controllers
{
    [Authorize]
    public class PracticeController : Controller
    {
        private readonly WebDuolingoContext _context;

        public PracticeController(WebDuolingoContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MulticChoice(int level, bool tryMistake)
        {
            List<Question> questionList = new List<Question>();
            if (!tryMistake)
                questionList = _context.Questions.Include(q => q.Answers).Where(q => q.Level == level).ToList();
            else
            {
                var emailUser = this.User.Identity.Name;
                var user = _context.AspNetUsers.FirstOrDefault(u => u.Email == emailUser);

                if (user != null)
                {
                    var logMistakeId = _context.LogMistakes.Include(l => l.IdQuesNavigation).Where(l => l.IdUser == user.Id).Select(l => l.IdQues).ToList();
                    questionList = _context.Questions.Include(q => q.Answers).Where(q => q.Level == level && logMistakeId.Contains(q.IdQues)).ToList();

                }
            }
                
            return View(questionList);
        }

        [HttpPost]
        public async Task<IActionResult> CheckAnwser(IFormCollection form)
        {
            List<GetAnswer> getAnwsers = new List<GetAnswer>();



            foreach (var key in form.Keys)
            {
                var ans = form[key];
                string itemID = key;
                string ansID = ans;

                int itemIDValue, ansIDValue;

                if (int.TryParse(itemID, out itemIDValue) && int.TryParse(ansID, out ansIDValue))
                {
                    GetAnswer getAnwser = new GetAnswer();
                    getAnwser.id_anwser = ansIDValue;
                    getAnwser.id_ques = itemIDValue;

                    getAnwsers.Add(getAnwser);
                }
                
            }

            var questions = _context.Questions.Include(q => q.Answers).ToList();
            CheckAnswer checkAns = new CheckAnswer();
            checkAns.Total = getAnwsers.Count();

            foreach (var question in questions)
            {
                var ans = getAnwsers.FirstOrDefault(g => g.id_ques == question.IdQues);

                if (ans != null )
                {
                    if(ans.id_anwser == question.CorrectAns)
                        checkAns.Correct += 1;
                    else
                    {
                        CorrectAnswer correctAnswer = new CorrectAnswer();

                        var answer = _context.Answers.FirstOrDefault(a => a.IdAns == question.CorrectAns);

                        correctAnswer.question = question;
                        correctAnswer.answer = answer;

                        checkAns.InCorrect += 1;
                        checkAns.WrongAnswer.Add(correctAnswer);
                    }
                }
            }

            checkAns.Score = (float)(checkAns.Correct * 10) / (float)checkAns.Total;

            var emailUser = this.User.Identity.Name;
            var user = _context.AspNetUsers.FirstOrDefault(u => u.Email == emailUser);

            if (user != null) 
            foreach (var item in checkAns.WrongAnswer)
            {
                var logMistake = _context.LogMistakes.FirstOrDefault(l => l.IdQues == item.question.IdQues && l.IdUser == user.Id);
                if (logMistake == null)
                {
                    var newMistake = new LogMistake();
                    newMistake.IdQues = item.question.IdQues;
                    newMistake.IdUser = user.Id;
                    newMistake.DatetimeQues = DateTime.Now;
                    newMistake.ContQues = 1;
                    await _context.LogMistakes.AddAsync(newMistake);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    logMistake.ContQues = logMistake.ContQues +1;
                    logMistake.DatetimeQues= DateTime.Now;
                    await _context.SaveChangesAsync();
                }

            }

            return View(checkAns);
        }
    }
}
