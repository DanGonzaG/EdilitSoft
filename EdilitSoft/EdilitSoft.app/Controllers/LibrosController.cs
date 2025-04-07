using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EdilitSoft.Models;
using EdilitSoft.app.Models;

namespace EdilitSoft.app.Controllers
{
    public class LibrosController : Controller
    {
        private readonly Contexto _context;

        public LibrosController(Contexto context)
        {
            _context = context;
        }

        // GET: Libros
        public async Task<IActionResult> Index()
        {
            var contexto = _context.Libro.Include(l => l.Categoria).Include(l => l.Editorial);
            return View(await contexto.ToListAsync());
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libros = await _context.Libro
                .Include(l => l.Categoria)
                .Include(l => l.Editorial)
                .FirstOrDefaultAsync(m => m.IdLibro == id);
            if (libros == null)
            {
                return NotFound();
            }

            return View(libros);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "IdCategoria", "NombreCategoria");
            ViewData["EditorialId"] = new SelectList(_context.Editorial, "IdEditorial", "NombreEditorial");
            return View();
        }

        // POST: Libros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLibro,ISBN,CategoriaId,EditorialId,Titulo,Autor,Sinopsis,Anyo,Activo")] Libros libros)
        {
            bool existeISBN = await _context.Libro
            .AnyAsync(d => d.ISBN.ToLower() == libros.ISBN.ToLower());

            if(existeISBN)
            {
                ModelState.AddModelError("ISBN", "Este ISBN ya esta registrado");
            }

            if (ModelState.IsValid)
            {
                libros.Activo = true;
                _context.Add(libros);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "IdCategoria", "NombreCategoria", libros.CategoriaId);
            ViewData["EditorialId"] = new SelectList(_context.Editorial, "IdEditorial", "NombreEditorial", libros.EditorialId);
            return View(libros);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libros = await _context.Libro.FindAsync(id);
            if (libros == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "IdCategoria", "NombreCategoria", libros.CategoriaId);
            ViewData["EditorialId"] = new SelectList(_context.Editorial, "IdEditorial", "NombreEditorial", libros.EditorialId);
            return View(libros);
        }

        // POST: Libros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLibro,ISBN,CategoriaId,EditorialId,Titulo,Autor,Sinopsis,Anyo,Activo")] Libros libros)
        {
            if (id != libros.IdLibro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libros);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibrosExists(libros.IdLibro))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "IdCategoria", "NombreCategoria", libros.CategoriaId);
            ViewData["EditorialId"] = new SelectList(_context.Editorial, "IdEditorial", "NombreEditorial", libros.EditorialId);
            return View(libros);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libros = await _context.Libro
                .Include(l => l.Categoria)
                .Include(l => l.Editorial)
                .FirstOrDefaultAsync(m => m.IdLibro == id);
            if (libros == null)
            {
                return NotFound();
            }

            return View(libros);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libros = await _context.Libro.FindAsync(id);
            if (libros != null)
            {
                _context.Libro.Remove(libros);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibrosExists(int id)
        {
            return _context.Libro.Any(e => e.IdLibro == id);
        }
    }
}
