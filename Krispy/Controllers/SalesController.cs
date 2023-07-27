using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Krispy.Data;
using Krispy.Models;
using NuGet.Packaging;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore.Storage;
using NuGet.Common;

namespace Krispy.Controllers
{
    public class SalesController : Controller
    {
        private readonly KrispyContext _context;

        public SalesController(KrispyContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var items = await _context.Donuts.ToListAsync();

            if (items != null)
            {
                var tmp = items.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });
                ViewBag.data = tmp;
            }
            return View();
        }

        public IActionResult Thanks (Sales sale)
        {
            var donut = _context.Donuts.ToList().Where(d => d.Id == Convert.ToInt32(sale.Type));
            ViewBag.Sale = sale;
            foreach(var item in donut)
            {
                ViewBag.Name = item.Name;
            }
            
            return View();
        }

        // GET: Sales
        public async Task<IActionResult> TotalSales ()
        {
            var items = await _context.Donuts.ToListAsync();

            if (items != null)
            {
                var donuts = items.Select(x => new Donuts
                {
                    Id = x.Id,
                    Name = x.Name
                });
                ViewBag.data = donuts;
            }

            return _context.Sales != null ?
                          View(await _context.Sales.ToListAsync()) :
                          Problem("Entity set 'KrispyContext.Sales'  is null.");
        }

        public async Task<IActionResult> Search(IFormCollection form)
        {
            string typeDonut = form["dropDonuts"].ToString();

            var amount = _context.Sales.ToList().Where(s => s.Type == Convert.ToInt32(typeDonut)).Sum(a => a.Amount);
            
            if(amount >= 10)
            {
                var items = _context.Donuts.ToList().Where(d => d.Id == Convert.ToInt32(typeDonut)).FirstOrDefault();

                if (items != null)
                {
                    ViewBag.Name = items.Name;
                    ViewBag.Amount = amount;
                }
                return View(_context.Sales.ToList().Where(d => d.Type == Convert.ToInt32(typeDonut)));
            }else
            {
                return View("NotEnoughSales");
            }
            //return View();
        }

        public IActionResult NotEnoughSales()
        {
            return View();
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var sales = await _context.Sales
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sales == null)
            {
                return NotFound();
            }

            return View(sales);
        }

        // GET: Sales/Create
        public async Task<IActionResult> Create()
        {
            var items = await _context.Donuts.ToListAsync();

            if (items != null)
            {
                var tmp = items.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(), 
                    Text = x.Name
                });
                ViewBag.data = tmp;
            }
            return View();
        }

        //public List<Sales> GetSales()

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,UserName,Direction,Amount")] Sales sale, IFormCollection form)
        {
            string strDonutValue = form["dropDonuts"].ToString();
            sale.Type = Convert.ToInt32(strDonutValue);
            if (ModelState.IsValid)
            {
                _context.Add(sale);
                await _context.SaveChangesAsync();
                return RedirectToAction("Thanks", sale);
            }
            return RedirectToAction("Thanks", sale);

        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var sales = await _context.Sales.FindAsync(id);
            if (sales == null)
            {
                return NotFound();
            }
            return View(sales);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Direction,Type,Amount")] Sales sales)
        {
            if (id != sales.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sales);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesExists(sales.Id))
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
            return View(sales);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var sales = await _context.Sales
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sales == null)
            {
                return NotFound();
            }

            return View(sales);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sales == null)
            {
                return Problem("Entity set 'KrispyContext.Sales'  is null.");
            }
            var sales = await _context.Sales.FindAsync(id);
            if (sales != null)
            {
                _context.Sales.Remove(sales);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesExists(int id)
        {
          return (_context.Sales?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
