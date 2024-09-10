using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebDuolingo.Models;

namespace WebDuolingo.Controllers
{
    [Authorize(Roles = "Admin")]   
    public class QuestionsController : Controller
    {
        private readonly WebDuolingoContext _context;

        public QuestionsController(WebDuolingoContext context)
        {
            _context = context;
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            var webDuolingoContext = _context.Questions.Include(q => q.CorrectAnsNavigation).Include(q => q.CreateByNavigation).Include(q => q.IdTypeNavigation).Include(q => q.LevelNavigation);
            return View(await webDuolingoContext.ToListAsync());
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.CorrectAnsNavigation)
                .Include(q => q.CreateByNavigation)
                .Include(q => q.IdTypeNavigation)
                .Include(q => q.LevelNavigation)
                .FirstOrDefaultAsync(m => m.IdQues == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

       
            public IActionResult Create()
            {
                ViewData["CorrectAns"] = new SelectList(_context.Answers.Where(a => a.IdQues == null), "IdAns", "NameAns");
                ViewData["CreateBy"] = new SelectList(_context.AspNetUsers, "Id", "Email");
                ViewData["IdType"] = new SelectList(_context.QuestionTypes, "IdType", "Name");
                ViewData["Level"] = new SelectList(_context.Levels, "IdLevel", "NameLevel");
                return View();
            }

        
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("IdQues,NameQues,Level,CorrectAns,IdType,CreateBy,CreateDate,UpdateDate")] Question question)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(question);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CorrectAns"] = new SelectList(_context.Answers, "IdAns", "NameAns", question.CorrectAns);
                ViewData["CreateBy"] = new SelectList(_context.AspNetUsers, "Id", "Email", question.CreateBy);
                ViewData["IdType"] = new SelectList(_context.QuestionTypes, "IdType", "Name", question.IdType);
                ViewData["Level"] = new SelectList(_context.Levels, "IdLevel", "NameLevel", question.Level);
                return View(question);
            }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["CorrectAns"] = new SelectList(_context.Answers, "IdAns", "NameAns", question.CorrectAns);
            ViewData["CreateBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", question.CreateBy);
            ViewData["IdType"] = new SelectList(_context.QuestionTypes, "IdType", "IdType", question.IdType);
            ViewData["Level"] = new SelectList(_context.Levels, "IdLevel", "IdLevel", question.Level);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("IdQues,NameQues,Level,CorrectAns,IdType,CreateBy,CreateDate,UpdateDate")] Question question)
            {
                if (id != question.IdQues)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(question);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!QuestionExists(question.IdQues))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CorrectAns"] = new SelectList(_context.Answers, "IdAns", "NameAns", question.CorrectAns);
                ViewData["CreateBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", question.CreateBy);
                ViewData["IdType"] = new SelectList(_context.QuestionTypes, "IdType", "IdType", question.IdType);
                ViewData["Level"] = new SelectList(_context.Levels, "IdLevel", "IdLevel", question.Level);
                return View(question);
            }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.CorrectAnsNavigation)
                .Include(q => q.CreateByNavigation)
                .Include(q => q.IdTypeNavigation)
                .Include(q => q.LevelNavigation)
                
                .FirstOrDefaultAsync(m => m.IdQues == id);
               
            

            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Questions == null)
            {
                return Problem("Entity set 'WebDuolingoContext.Questions'  is null.");
            }
            var question =  _context.Questions.Include(q => q.Answers).FirstOrDefault(q => q.IdQues == id);
            
            var log = _context.LogMistakes.Where(l => l.IdQues == question.IdQues).ToList();
            if (log.Count() > 0)
            {
                return Problem("Câu trả lời đã được ghi Log");
            }

            foreach (var item in question.Answers.ToList())
            {
                item.IdQuesNavigation = null;  
                _context.Answers.Remove(item);
                _context.SaveChanges();
            }

            question.Answers = null;
            if (question != null)
            {
                _context.Questions.Remove(question);
            }
       
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
          return (_context.Questions?.Any(e => e.IdQues == id)).GetValueOrDefault();
        }
    }
}
