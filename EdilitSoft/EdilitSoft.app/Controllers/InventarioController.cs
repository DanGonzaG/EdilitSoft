using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EdilitSoft.Models;
using EdilitSoft.app.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EdilitSoft.app.Controllers
{
    public class InventarioController : Controller
    {
        private readonly Contexto _context;

        public InventarioController(Contexto context)
        {
            _context = context;
        }

        // GET: Inventario
        public async Task<IActionResult> Index()
        {
            var inventarios = await _context.Inventario
                             .Include(i => i.Libros)
                             .OrderBy(i => i.IdArticulo)  // Ordena por algún criterio
                             .ToListAsync();

            return View(inventarios);
        }

        // GET: Inventario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventario
                .Include(i => i.Cotizaciones)
                .Include(i => i.Libros)
                .FirstOrDefaultAsync(m => m.IdArticulo == id);
            if (inventario == null)
            {
                return NotFound();
            }

            return View(inventario);
        }

        // GET: Inventario/Create
        public IActionResult Create()
        {
            ViewData["IdArticulo"] = new SelectList(_context.Cotizacion, "IdCotizacion", "IdCotizacion");
            ViewData["IdLibro"] = new SelectList(_context.Libro, "IdLibro", "Titulo");
            return View();
        }

        // POST: Inventario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Inventario inventario) // Elimina el [Bind]
        {
            // Validación de libro existente
            if (await _context.Inventario.AnyAsync(i => i.IdLibro == inventario.IdLibro && i.Activo))
            {
                ModelState.AddModelError(string.Empty, "El libro ya existe en el inventario activo");
            }

            // Establece valores por defecto
            inventario.Fecha = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(inventario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdLibro"] = new SelectList(_context.Libro, "IdLibro", "Titulo", inventario.IdLibro);
            return View(inventario);
        }

        // GET: Inventario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventario.FindAsync(id);
            if (inventario == null)
            {
                return NotFound();
            }
            ViewData["IdArticulo"] = new SelectList(_context.Cotizacion, "IdCotizacion", "IdCotizacion", inventario.IdArticulo);
            ViewData["IdLibro"] = new SelectList(_context.Libro, "IdLibro", "Titulo", inventario.IdLibro);
            return View(inventario);
        }

        // POST: Inventario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdArticulo,IdLibro,Fecha,Existencias,Precio,Activo")] Inventario inventario)
        {
            if (id != inventario.IdArticulo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventarioExists(inventario.IdArticulo))
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
            ViewData["IdArticulo"] = new SelectList(_context.Cotizacion, "IdCotizacion", "IdCotizacion", inventario.IdArticulo);
            ViewData["IdLibro"] = new SelectList(_context.Libro, "IdLibro", "Autor", inventario.IdLibro);
            return View(inventario);
        }

        // GET: Inventario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventario = await _context.Inventario
                .Include(i => i.Libros)
                .FirstOrDefaultAsync(m => m.IdArticulo == id);
            if (inventario == null)
            {
                return NotFound();
            }

            return View(inventario);
        }

        // POST: Inventario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventario = await _context.Inventario.FindAsync(id);
            if (inventario != null)
            {
                _context.Inventario.Remove(inventario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventarioExists(int id)
        {
            return _context.Inventario.Any(e => e.IdArticulo == id);
        }
    }
}
