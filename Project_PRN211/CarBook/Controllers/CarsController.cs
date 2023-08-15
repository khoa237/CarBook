using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarBook.Models;

namespace CarBook.Controllers
{
    public class CarsController : Controller
    {
        private readonly CarBookContext _context;

        public CarsController(CarBookContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var carBookContext = _context.Cars.Include(c => c.Category).Include(c => c.Feature).Include(c => c.Property).Include(c => c.Owner);
            //ViewData["abc"] = carBookContext.ToList();
            ViewData["abc"] = carBookContext.ToList().Count;
            return View(await carBookContext.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Category)
                .Include(c => c.Feature)
                .Include(c => c.Owner)
                .Include(c => c.Property)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["FeatureID"] = new SelectList(_context.Features, "Id", "Id");
            ViewData["OwnerID"] = new SelectList(_context.Set<AppUser>(), "Id", "Id");
            ViewData["PropertyID"] = new SelectList(_context.Properties, "Id", "Id");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Image,Price,CategoryID,PropertyID,FeatureID,OwnerID")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "Id", "Name", car.CategoryID);
            ViewData["FeatureID"] = new SelectList(_context.Features, "Id", "Id", car.FeatureID);
            ViewData["OwnerID"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", car.OwnerID);
            ViewData["PropertyID"] = new SelectList(_context.Properties, "Id", "Id", car.PropertyID);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "Id", "Name", car.CategoryID);
            ViewData["FeatureID"] = new SelectList(_context.Features, "Id", "Id", car.FeatureID);
            ViewData["OwnerID"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", car.OwnerID);
            ViewData["PropertyID"] = new SelectList(_context.Properties, "Id", "Id", car.PropertyID);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Image,Price,CategoryID,PropertyID,FeatureID,OwnerID")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
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
            ViewData["CategoryID"] = new SelectList(_context.Categories, "Id", "Name", car.CategoryID);
            ViewData["FeatureID"] = new SelectList(_context.Features, "Id", "Id", car.FeatureID);
            ViewData["OwnerID"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", car.OwnerID);
            ViewData["PropertyID"] = new SelectList(_context.Properties, "Id", "Id", car.PropertyID);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Category)
                .Include(c => c.Feature)
                .Include(c => c.Owner)
                .Include(c => c.Property)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cars == null)
            {
                return Problem("Entity set 'CarBookContext.Cars'  is null.");
            }
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
          return (_context.Cars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
