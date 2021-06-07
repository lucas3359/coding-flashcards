using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CodingFlashcard.Data;
using CodingFlashcard.Models;

namespace CodingFlashcard.Controllers
{
    public class FlashcardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlashcardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Flashcard
        public async Task<IActionResult> Index()
        {
            return View(await _context.Flashcard.ToListAsync());
        }



        // GET: Flashcard/ShowRandom
        public async Task<IActionResult> ShowRandom()
        {

            var flashcardCount = _context.Flashcard.Count();

            Random r = new Random();
            int rIndex = r.Next(0, flashcardCount);

            return View( _context.Flashcard.Skip(rIndex).FirstOrDefault());
        }

        // POST: Flashcard/ShowRandom/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowRandom(int? cardId, bool known )
        {
            var flashcard = await _context.Flashcard.FindAsync(cardId);

            if (flashcard == null)
            {
                return NotFound();
            }

            flashcard.Timeseen += 1;
            flashcard.Known = known;
            await _context.SaveChangesAsync();

            return await ShowRandom();
        }




        // GET: Flashcard/ShowRandomResult
        public async Task<IActionResult> ShowRandomResult(int? cardId)
        {
            var result = _context.Flashcard.Where(j => j.Id == cardId).FirstOrDefault();

            return View(result);
        }




        // GET: Flashcard/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flashcard = await _context.Flashcard
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flashcard == null)
            {
                return NotFound();
            }

            return View(flashcard);
        }

        // GET: Flashcard/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flashcard/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,Answer,Timeseen,Known,Difficulty")] Flashcard flashcard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flashcard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flashcard);
        }

        // GET: Flashcard/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flashcard = await _context.Flashcard.FindAsync(id);
            if (flashcard == null)
            {
                return NotFound();
            }
            return View(flashcard);
        }

        // POST: Flashcard/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer,Timeseen,Known,Difficulty")] Flashcard flashcard)
        {
            if (id != flashcard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flashcard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlashcardExists(flashcard.Id))
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
            return View(flashcard);
        }

        // GET: Flashcard/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flashcard = await _context.Flashcard
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flashcard == null)
            {
                return NotFound();
            }

            return View(flashcard);
        }

        // POST: Flashcard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flashcard = await _context.Flashcard.FindAsync(id);
            _context.Flashcard.Remove(flashcard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlashcardExists(int id)
        {
            return _context.Flashcard.Any(e => e.Id == id);
        }
    }
}
