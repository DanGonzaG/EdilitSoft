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
        public async Task<IActionResult> Index(string sortOrder, string currentFilter)
        {
            // Configura los parámetros de ordenamiento
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TituloSortParam"] = sortOrder == "titulo_asc" ? "titulo_desc" : "titulo_asc";
            ViewData["AutorSortParam"] = sortOrder == "autor_asc" ? "autor_desc" : "autor_asc";
            ViewData["FechaSortParam"] = sortOrder == "fecha_asc" ? "fecha_desc" : "fecha_asc";
            ViewData["ExistenciasSortParam"] = sortOrder == "existencias_asc" ? "existencias_desc" : "existencias_asc";
            ViewData["PrecioSortParam"] = sortOrder == "precio_asc" ? "precio_desc" : "precio_asc";

            var query = _context.Inventario.Include(i => i.Libros).AsQueryable();

            // Aplica el ordenamiento
            query = sortOrder switch
            {
                "titulo_asc" => query.OrderBy(i => i.Libros!.Titulo),
                "titulo_desc" => query.OrderByDescending(i => i.Libros!.Titulo),
                "autor_asc" => query.OrderBy(i => i.Libros!.Autor),
                "autor_desc" => query.OrderByDescending(i => i.Libros!.Autor),
                "fecha_asc" => query.OrderBy(i => i.Fecha),
                "fecha_desc" => query.OrderByDescending(i => i.Fecha),
                "existencias_asc" => query.OrderBy(i => i.Existencias),
                "existencias_desc" => query.OrderByDescending(i => i.Existencias),
                "precio_asc" => query.OrderBy(i => i.Precio),
                "precio_desc" => query.OrderByDescending(i => i.Precio),
                _ => query.OrderBy(i => i.IdArticulo) // Orden por defecto
            };

            return View(await query.ToListAsync());
        }

        // GET: Inventario/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Inventario/Create
        public IActionResult Create()
        {
            
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
            ViewData["IdLibro"] = new SelectList(_context.Libro, "IdLibro", "Titulo", inventario.IdLibro);
            return View(inventario);
        }

        // POST: Inventario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdArticulo,IdLibro,Existencias,Precio,Activo")] Inventario inventario)
        {
            if (id != inventario.IdArticulo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                inventario.Fecha = DateTime.Now;
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
