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
    public class ClientesController : Controller
    {
        private readonly Contexto _context;

        public ClientesController(Contexto context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cliente.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientes = await _context.Cliente
                .FirstOrDefaultAsync(m => m.IdCliente == id);
            if (clientes == null)
            {
                return NotFound();
            }

            return View(clientes);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // JuanPa: este método crea clientes validando duplicados y fuerza el campo Activo en true
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Clientes clientes)
        {
            if (ModelState.IsValid)
            {
                // Validación: nombre duplicado
                bool existeNombre = await _context.Cliente
                    .AnyAsync(c => c.Nombre.ToLower() == clientes.Nombre.ToLower());

                // Validación: teléfono duplicado
                bool existeTelefono = await _context.Cliente
                    .AnyAsync(c => c.Telefono != null && c.Telefono.Trim() == clientes.Telefono.Trim());

                if (existeNombre)
                {
                    ModelState.AddModelError("Nombre", "Ya existe un cliente con ese nombre.");
                }

                if (existeTelefono)
                {
                    ModelState.AddModelError("Telefono", "Ya existe un cliente con ese número de teléfono.");
                }

                if (existeNombre || existeTelefono)
                {
                    return View(clientes);
                }

                clientes.Activo = true;
                _context.Add(clientes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clientes);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientes = await _context.Cliente.FindAsync(id);
            if (clientes == null)
            {
                return NotFound();
            }
            return View(clientes);
        }

        // POST: Clientes/Edit/5
        // JuanPa: este método valida duplicados antes de guardar cambios al editar cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Clientes clientes)
        {
            if (id != clientes.IdCliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Validar si existe otro cliente con el mismo nombre (excluyendo el actual)
                bool nombreDuplicado = await _context.Cliente
                    .AnyAsync(c => c.IdCliente != clientes.IdCliente && c.Nombre.ToLower() == clientes.Nombre.ToLower());

                bool telefonoDuplicado = await _context.Cliente
                    .AnyAsync(c => c.IdCliente != clientes.IdCliente && c.Telefono != null && c.Telefono.Trim() == clientes.Telefono.Trim());

                if (nombreDuplicado)
                {
                    ModelState.AddModelError("Nombre", "Ya existe otro cliente con ese nombre.");
                }

                if (telefonoDuplicado)
                {
                    ModelState.AddModelError("Telefono", "Ya existe otro cliente con ese número de teléfono.");
                }

                if (nombreDuplicado || telefonoDuplicado)
                {
                    return View(clientes);
                }

                try
                {
                    _context.Update(clientes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientesExists(clientes.IdCliente))
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

            return View(clientes);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientes = await _context.Cliente
                .FirstOrDefaultAsync(m => m.IdCliente == id);
            if (clientes == null)
            {
                return NotFound();
            }

            return View(clientes);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientes = await _context.Cliente.FindAsync(id);
            if (clientes != null)
            {
                _context.Cliente.Remove(clientes);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientesExists(int id)
        {
            return _context.Cliente.Any(e => e.IdCliente == id);
        }
    }
}
