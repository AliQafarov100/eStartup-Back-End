using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eStartup_Back_End.DAL;
using eStartup_Back_End.Models;
using eStartup_Back_End.FileExtensions;
using eStartup_Back_End.Utilities;
using Microsoft.AspNetCore.Hosting;

namespace eStartup_Back_End.Areas.eStartupAdmin.Controllers
{
    [Area("eStartupAdmin")]
    public class CardsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CardsController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

      
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cards.ToListAsync());
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

       
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Card card)
        {
            if (ModelState.IsValid)
            {
                if(card.Photo != null)
                {
                    if (card.Photo.IsOkay(1))
                    {
                        card.Image = await card.Photo.PathFile(_env.WebRootPath, @"assets\Image\Cards");
                    }

                }
                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(card);
        }

      
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            return View(card);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Card card)
        {
            Card existed = await _context.Cards.FirstOrDefaultAsync(c => c.Id == id);
            if (id != card.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(card.Photo != null)
                    {
                        if (card.Photo.IsOkay(1))
                        {
                            string path = _env.WebRootPath + @"assets\Image\Cards" + existed.Image;
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }

                            existed.Image = await card.Photo.PathFile(_env.WebRootPath, @"assets\Image\Cards");
                        }
                    }

                    existed.Description = card.Description;
                    existed.Data = card.Data;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(card.Id))
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
            return View(card);
        }

      
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.Id == id);
        }
    }
}
