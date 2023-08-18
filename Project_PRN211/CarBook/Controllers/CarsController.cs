using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarBook.Models;
using Microsoft.AspNetCore.Identity;


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
        public async Task<IActionResult> Index(string currentFilter, string searchString, int? pageNumber, string electricCar, int? categoryId, string sortOrder)
        {
            IQueryable<Car> carsQuery = _context.Cars.Include(c => c.Category).Include(c => c.Feature).Include(c => c.Property).Include(c => c.Owner);

            if (!string.IsNullOrEmpty(searchString))
            {
                carsQuery = carsQuery.Where(c => c.Name.Contains(searchString) || c.Category.Name.Contains(searchString) || c.Price.Contains(searchString));
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var categoryList = await _context.Categories.ToListAsync();
            ViewData["CategoryList"] = categoryList;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["SortPriceD"] = String.IsNullOrEmpty(sortOrder) ? "pricedesc" : "pricedesc";
            ViewData["SortPriceA"] = sortOrder == "priceasc" ? "priceasc" : "priceasc";

            // Lọc theo electricCar 
            if (electricCar != null)
            {
                carsQuery = carsQuery.Where(c => c.Property.Fuel == "Electricity");
                ViewData["CarElectric"] = "Electricity";
            }
            // Lọc theo CategoryID 
            if (categoryId != null)
            {
                carsQuery = carsQuery.Where(c => c.CategoryID == categoryId);
                ViewData["CurrentCategory"] = categoryId;
            }
            switch (sortOrder)
            {
                case "priceasc":
                    carsQuery = carsQuery.OrderBy(c => c.Price);
                    break;
                case "pricedesc":
                    carsQuery = carsQuery.OrderByDescending(c => c.Price);
                    break;
                default:                
                    break;
            }

            int totalCount = await carsQuery.CountAsync();
            int dividedValue = (int)Math.Ceiling(totalCount / (double)9);
            ViewData["TotalCount"] = dividedValue;
            int pageSize = 9;
            return View(await PaginatedList<Car>.CreateAsync(carsQuery.AsNoTracking(), pageNumber ?? 1, pageSize));
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

			var carsWithSameCategory = await _context.Cars
			   .Include(p => p.Category)
			   .Where(p => p.Category.Id == car.Category.Id && p.Id != id)
			   .ToListAsync();

			ViewData["CartsWithSameCategory"] = carsWithSameCategory;
			return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["FeatureID"] = new SelectList(_context.Features, "Id", "Id");
            ViewData["OwnerID"] = new SelectList(_context.Set<IdentityUser>(), "Id", "Id");
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
            ViewData["OwnerID"] = new SelectList(_context.Set<IdentityUser>(), "Id", "Id", car.OwnerID);
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
