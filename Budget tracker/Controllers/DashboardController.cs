using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Budget_tracker.Data;
using Budget_tracker.Models;
using System.Globalization;

namespace Budget_tracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly BudgetDbContext _context;

        public DashboardController(BudgetDbContext context)
        {
            _context = context;
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<Transaction> Transactions = await _context.Transactions
                .Include(x => x.Category)
                
                .ToListAsync();

            //Total Income
            int TotalIncome = Transactions.Where(i => i.Category.Type == "Income")
                .Sum(j => j.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");

            //Total Expense
            int TotalExpense = Transactions
                .Where(i => i.Category.Type == "Expense")
                .Sum(j => j.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C0");

            //Balance
            int Balance = TotalIncome - TotalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C0}", Balance);


            return View(Transactions);
        }
        public JsonResult GetJson()
        {
            return Json(Index());
        }
        // GET: Dashboard/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title");
            return View();
        }

        // POST: Dashboard/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Amount,Note,Date")] Transaction transaction)
        {
            
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Title", transaction.CategoryId);
            return View(transaction);
        }
        public ActionResult ExpenseSummary()
        {
            return PartialView("_expenseReport");
        }
    }
}
