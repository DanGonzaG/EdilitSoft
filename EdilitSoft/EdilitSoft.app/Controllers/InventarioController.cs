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
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace EdilitSoft.app.Controllers
{
    public class InventarioController : Controller
    {
        private readonly Contexto _context;
        private object _logger;

        public InventarioController(Contexto context)
        {
            _context = context;
        }

        // GET: Inventario
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber,
            int? selectedId,
            int? deleteId,
            int? editId)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TituloSortParam"] = sortOrder == "titulo_asc" ? "titulo_desc" : "titulo_asc";
            ViewData["AutorSortParam"] = sortOrder == "autor_asc" ? "autor_desc" : "autor_asc";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var query = _context.Inventario.Include(i => i.Libros).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(i => i.Libros.Titulo.Contains(searchString)
                                      || i.Libros.Autor.Contains(searchString));
            }

            query = sortOrder switch
            {
                "titulo_asc" => query.OrderBy(i => i.Libros.Titulo),
                "titulo_desc" => query.OrderByDescending(i => i.Libros.Titulo),
                "autor_asc" => query.OrderBy(i => i.Libros.Autor),
                "autor_desc" => query.OrderByDescending(i => i.Libros.Autor),
                _ => query.OrderBy(i => i.IdArticulo)
            };

            // Resetear paneles
            ViewBag.ShowDetails = false;
            ViewBag.ShowEditPanel = false;
            ViewBag.ShowDeletePanel = false;


            if (selectedId.HasValue)
            {
                ViewBag.SelectedItem = await query.FirstOrDefaultAsync(i => i.IdArticulo == selectedId);
                ViewBag.ShowDetails = true;
                // Asegurarse de que los otros paneles estén cerrados
                ViewBag.ShowEditPanel = false;
                ViewBag.ShowDeletePanel = false;
            }
            else if (editId.HasValue)
            {
                ViewBag.ItemToEdit = await query.FirstOrDefaultAsync(i => i.IdArticulo == editId);
                ViewBag.ShowEditPanel = true;
                // Asegurarse de que los otros paneles estén cerrados
                ViewBag.ShowDetails = false;
                ViewBag.ShowDeletePanel = false;
            }
            else if (deleteId.HasValue)
            {
                ViewBag.ItemToDelete = await query.FirstOrDefaultAsync(i => i.IdArticulo == deleteId);
                ViewBag.ShowDeletePanel = true;
                // Asegurarse de que los otros paneles estén cerrados
                ViewBag.ShowDetails = false;
                ViewBag.ShowEditPanel = false;
            }
            else
            {
                // Si no hay ningún panel activo, asegurarse de que todos estén cerrados
                ViewBag.ShowDetails = false;
                ViewBag.ShowEditPanel = false;
                ViewBag.ShowDeletePanel = false;
            }

            int pageSize = 10; // Número de items por página
            return View(await PaginatedList<Inventario>.CreateAsync(query.AsNoTracking(), pageNumber ?? 1, pageSize));
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
        public async Task<IActionResult> Edit(int? id, string sortOrder, string currentFilter)
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

            ViewData["IdLibro"] = new SelectList(_context.Libro, "IdLibro", "Titulo", inventario.IdLibro);
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = currentFilter;

            return View(inventario);
        }


        // POST: Inventario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdArticulo,IdLibro,Existencias,Precio,Activo")] Inventario inventario)
        {
            

            if (ModelState.IsValid)
            {
                inventario.Fecha = DateTime.Now;
                try
                {
                    _context.Update(inventario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
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
            }

            // Si hay errores, recargar los datos necesarios
            ViewData["IdLibro"] = new SelectList(_context.Libro, "IdLibro", "Autor", inventario.IdLibro);
            

            return View(inventario);
        }

        // GET: Inventario/Delete/5
        // GET: Inventario/Delete/5
        public async Task<IActionResult> Delete(int? id, bool fromIndex = false)
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

            if (fromIndex)
            {
                // Redirige al Index con el parámetro deleteId
                return RedirectToAction(nameof(Index), new { deleteId = id });
            }

            return View(inventario);
        }

        // POST: Inventario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(IFormCollection form)
        {
            int id;
            if (!int.TryParse(form["IdArticulo"], out id) || id == 0)
            {
                return BadRequest("ID inválido");
            }

            var inventario = await _context.Inventario.FindAsync(id);
            if (inventario == null)
            {
                return NotFound();
            }

            _context.Inventario.Remove(inventario);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool InventarioExists(int id)
        {
            return _context.Inventario.Any(e => e.IdArticulo == id);
        }      
    }
}
