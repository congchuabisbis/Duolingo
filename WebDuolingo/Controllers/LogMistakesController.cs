using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDuolingo.Models;

namespace WebDuolingo.Controllers
{
    
    public class LogMistakesController : Controller
    {
        private readonly WebDuolingoContext _context;

        public LogMistakesController(WebDuolingoContext context)
        {
            _context = context;
        }
        
        public IActionResult Index(int level)
        {
            if (level==0)
            {
                var emailUser = this.User.Identity.Name;
                var user = _context.AspNetUsers.FirstOrDefault(u => u.Email == emailUser);
                var logMistake = _context.LogMistakes.Include(l => l.IdQuesNavigation).Where(l => l.IdUser == user.Id);
                return View(logMistake);
            }
            else
            {
                var emailUser = this.User.Identity.Name;
                var user = _context.AspNetUsers.FirstOrDefault(u => u.Email == emailUser);
                var logMistake = _context.LogMistakes.Include(l => l.IdQuesNavigation).Where(l => l.IdUser == user.Id && l.IdQuesNavigation.Level == level);
                return View(logMistake);
            }
            
        }
        public IActionResult GetAll()
        {
            return View();
        }
    }
}
