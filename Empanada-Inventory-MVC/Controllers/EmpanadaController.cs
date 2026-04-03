using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmpanadaInventory.Data;
using EmpanadaInventory.Models;

namespace EmpanadaInventory.Controllers
{
    public class EmpanadaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpanadaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Empanada
        public async Task<IActionResult> Index()
        {
            var empanadas = await _context.Empanadas.ToListAsync();
            return View(empanadas);
        }

        // GET: Empanada/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empanada/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Sabor,Precio,CantidadInventario")] Empanada empanada)
        {
            if (!ModelState.IsValid)
            {
                return View(empanada);
            }

            _context.Empanadas.Add(empanada);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Empanada/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var empanada = await _context.Empanadas.FindAsync(id);
            if (empanada == null) return NotFound();

            return View(empanada);
        }

        // POST: Empanada/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Sabor,Precio,CantidadInventario")] Empanada empanada)
        {
            if (id != empanada.Id) return NotFound();
            if (!ModelState.IsValid) return View(empanada);

            try
            {
                _context.Update(empanada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Empanadas.AnyAsync(e => e.Id == empanada.Id))
                    return NotFound();
                throw;
            }
        }

        // GET: Empanada/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var empanada = await _context.Empanadas.FirstOrDefaultAsync(m => m.Id == id);
            if (empanada == null) return NotFound();

            return View(empanada);
        }

        // POST: Empanada/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empanada = await _context.Empanadas.FindAsync(id);
            if (empanada != null)
            {
                _context.Empanadas.Remove(empanada);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}