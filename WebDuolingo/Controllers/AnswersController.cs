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
    public class AnswersController : Controller
    {
        private readonly WebDuolingoContext _context;

        public AnswersController(WebDuolingoContext context)
        {
            _context = context;
        }

        // GET: Answers
        public async Task<IActionResult> Index()
        {
            var webDuolingoContext = _context.Answers.Include(a => a.IdQuesNavigation);
            return View(await webDuolingoContext.ToListAsync());
        }

        // GET: Answers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Answers == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.IdQuesNavigation)
                .FirstOrDefaultAsync(m => m.IdAns == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: Answers/Create
        public IActionResult Create()
        {
            ViewData["IdQues"] = new SelectList(_context.Questions, "IdQues", "NameQues");
            return View();
        }

        // POST: Answers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAns,NameAns,IdQues,ImageAns")] Answer answer)
        {
            if (ModelState.IsValid)
            {
              

                    _context.Add(answer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
             }
                ViewData["IdQues"] = new SelectList(_context.Questions, "IdQues", "NameQues", answer.IdQues);
                return View(answer);
            
        }

        // GET: Answers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Answers == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            ViewData["IdQues"] = new SelectList(_context.Questions, "IdQues", "NameQues", answer.IdQues);
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAns,NameAns,IdQues,ImageAns")] Answer answer)
        {
            if (id != answer.IdAns)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(answer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerExists(answer.IdAns))
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
            ViewData["IdQues"] = new SelectList(_context.Questions, "IdQues", "NameQues", answer.IdQues);
            return View(answer);
        }

        // GET: Answers/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null || _context.Answers == null)
                {
                    return NotFound();
                }

                var answer = await _context.Answers
                    .Include(a => a.IdQuesNavigation)
                    .FirstOrDefaultAsync(m => m.IdAns == id);
                if (answer == null)
                {
                    return NotFound();
                }

                return View(answer);
            }

            // POST: Answers/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                if (_context.Answers == null)
                {
                    return Problem("Entity set 'WebDuolingoContext.Answers'  is null.");
                }
                var answer = await _context.Answers.FindAsync(id);
                var question = _context.Questions.FirstOrDefault(q => q.CorrectAns == answer.IdAns);
                if (question != null) {
                    question.CorrectAns = null;
                    _context.SaveChanges();
                }
                if (answer != null)
                {
                    _context.Answers.Remove(answer);
                }
            
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool AnswerExists(int id)
            {
              return (_context.Answers?.Any(e => e.IdAns == id)).GetValueOrDefault();
            }
    }
}
