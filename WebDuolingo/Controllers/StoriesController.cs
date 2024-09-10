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
   
    public class StoriesController : Controller
    {

        private readonly WebDuolingoContext _context;

        public StoriesController(WebDuolingoContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]  // GET: Stories
        public async Task<IActionResult> Index()
        {
            var webDuolingoContext = _context.Stories.Include(s => s.CreateByNavigation);
            return View(await webDuolingoContext.ToListAsync());
        }
        public async Task<IActionResult> IndexUser()
        {
            var webDuolingoContext = _context.Stories.Include(s => s.CreateByNavigation);
            return View(await webDuolingoContext.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Stories == null)
            {
                return NotFound();
            }

            var story = await _context.Stories
                .Include(s => s.CreateByNavigation)
                .FirstOrDefaultAsync(m => m.IdStr == id);
            if (story == null)
            {
                return NotFound();
            }

            return View(story);
        }


        // GET: Stories/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CreateBy"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Stories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStr,Title,Content,Image,CreateBy,CreateDate,UpdateDate")] Story story, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Step 1: Read the contents of the IFormFile into a byte array
                    using (var stream = new MemoryStream())
                    {
                        ImageFile.CopyTo(stream);
                        byte[] fileBytes = stream.ToArray();

                        // Step 2: Convert the byte array to a base64 string
                        string base64String = Convert.ToBase64String(fileBytes);

                        story.Image = base64String;
                    }
                }
                _context.Add(story);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreateBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", story.CreateBy);
            return View(story);
        }

        // GET: Stories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Stories == null)
            {
                return NotFound();
            }

            var story = await _context.Stories.FindAsync(id);
            if (story == null)
            {
                return NotFound();
            }
            ViewData["CreateBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", story.CreateBy);
            return View(story);
        }

        // POST: Stories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStr,Title,Content,Image,CreateBy,CreateDate,UpdateDate")] Story story)
        {
            if (id != story.IdStr)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(story);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoryExists(story.IdStr))
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
            ViewData["CreateBy"] = new SelectList(_context.AspNetUsers, "Id", "Id", story.CreateBy);
            return View(story);
        }

        // GET: Stories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Stories == null)
            {
                return NotFound();
            }

            var story = await _context.Stories
                .Include(s => s.CreateByNavigation)
                .FirstOrDefaultAsync(m => m.IdStr == id);
            if (story == null)
            {
                return NotFound();
            }

            return View(story);
        }

        // POST: Stories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Stories == null)
            {
                return Problem("Entity set 'WebDuolingoContext.Stories'  is null.");
            }
            var story = await _context.Stories.FindAsync(id);
            if (story != null)
            {
                _context.Stories.Remove(story);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoryExists(int id)
        {
          return (_context.Stories?.Any(e => e.IdStr == id)).GetValueOrDefault();
        }
    }
}
