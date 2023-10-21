using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoFront.Data;
using TodoFront.Models;

namespace TodoFront.Controllers
{
    public class TodoController : Controller
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }

        // GET: Todo
        public async Task<IActionResult> Index()
        {
              return _context.TodoItem != null 
                ? View(await _context.TodoItem.ToListAsync()) 
                : Problem("TODO-kohtia ei löytynyt");
        }

        // GET: Todo/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null || _context.TodoItem == null) {
                return NotFound();
            }

            var todoItem = await _context.TodoItem.FirstOrDefaultAsync(m => m.Id == id);

            if (todoItem == null) {
                return NotFound();
            }

            return View(todoItem);
        }

        // GET: Todo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,isCompleted")] TodoItem todoItem) {
            if (ModelState.IsValid) {
                _context.Add(todoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todoItem);
        }

        // GET: Todo/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null || _context.TodoItem == null) {
                return NotFound();
            }

            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem == null) {
                return NotFound();
            }
            return View(todoItem);
        }

        // POST: Todo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,isCompleted")] TodoItem todoItem) {
            if (id != todoItem.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(todoItem);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!TodoItemExists(todoItem.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todoItem);
        }

        // GET: Todo/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null || _context.TodoItem == null) {
                return NotFound();
            }

            var todoItem = await _context.TodoItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoItem == null) {
                return NotFound();
            }

            return View(todoItem);
        }

        // POST: Todo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TodoItem == null)
            {
                return Problem("Entity set 'TodoContext.TodoItem'  is null.");
            }
            var todoItem = await _context.TodoItem.FindAsync(id);
            if (todoItem != null)
            {
                _context.TodoItem.Remove(todoItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoItemExists(int id)
        {
          return (_context.TodoItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
