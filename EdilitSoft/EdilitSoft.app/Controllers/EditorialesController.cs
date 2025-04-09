using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EdilitSoft.app.Models;

namespace EdilitSoft.app.Controllers
{
    public class EditorialesController : Controller
    {
        private readonly Contexto _context;

        public EditorialesController(Contexto context)
        {
            _context = context;
        }

        // GET: Editoriales
        public async Task<IActionResult> Index()
        {
            return View(await _context.Editorial.ToListAsync());
        }

        // GET: Editoriales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editoriales = await _context.Editorial
                .FirstOrDefaultAsync(m => m.IdEditorial == id);
            if (editoriales == null)
            {
                return NotFound();
            }

            return View(editoriales);
        }

        // GET: Editoriales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Editoriales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEditorial,NombreEditorial,Telefono,SitioWeb,Activo")] Editoriales editoriales)
        {

            bool existeNombre = await _context.Editorial
            .AnyAsync(e => e.NombreEditorial.ToLower() == editoriales.NombreEditorial.ToLower());

            if (existeNombre)
            {
                ModelState.AddModelError("NombreEditorial", "Ya existe una editorial con ese nombre.");
            }


            if (!string.IsNullOrEmpty(editoriales.Telefono))
            {
                bool existeTelefono = await _context.Editorial
                    .AnyAsync(e => e.Telefono != null && e.Telefono.Trim() == editoriales.Telefono.Trim());

                if (existeTelefono)
                {
                    ModelState.AddModelError("Telefono", "Ya existe una editorial con ese número de teléfono.");
                }
            }



            if (ModelState.IsValid)
            {
                editoriales.Activo = true;
                _context.Add(editoriales);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(editoriales);
        }

        // GET: Editoriales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editoriales = await _context.Editorial.FindAsync(id);
            if (editoriales == null)
            {
                return NotFound();
            }
            return View(editoriales);
        }

        // POST: Editoriales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEditorial,NombreEditorial,Telefono,SitioWeb,Activo")] Editoriales editoriales)
        {
            if (id != editoriales.IdEditorial)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editoriales);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EditorialesExists(editoriales.IdEditorial))
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
            return View(editoriales);
        }

        // GET: Editoriales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editoriales = await _context.Editorial
                .FirstOrDefaultAsync(m => m.IdEditorial == id);
            if (editoriales == null)
            {
                return NotFound();
            }

            return View(editoriales);
        }

        // POST: Editoriales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var editoriales = await _context.Editorial.FindAsync(id);
            if (editoriales != null)
            {
                _context.Editorial.Remove(editoriales);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EditorialesExists(int id)
        {
            return _context.Editorial.Any(e => e.IdEditorial == id);
        }
    }
}
