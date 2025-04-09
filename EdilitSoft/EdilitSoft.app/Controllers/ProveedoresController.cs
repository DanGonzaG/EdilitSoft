using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EdilitSoft.app.Models;

namespace EdilitSoft.app.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly Contexto _context;

        public ProveedoresController(Contexto context)
        {
            _context = context;
        }

        // GET: Proveedores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proveedor.ToListAsync());
        }

        // GET: Proveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var proveedor = await _context.Proveedor.FirstOrDefaultAsync(p => p.IdProveedor == id);
            if (proveedor == null)
                return NotFound();

            return View(proveedor);
        }

        // GET: Proveedores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Proveedores proveedor)
        {
            if (!Regex.IsMatch(proveedor.Telefono ?? "", @"^\d{4}-\d{4}$"))
                ModelState.AddModelError("Telefono", "El teléfono debe tener el formato NNNN-NNNN.");

            if (string.IsNullOrWhiteSpace(proveedor.Correo) || !proveedor.Correo.Contains("@"))
                ModelState.AddModelError("Correo", "El correo debe contener un '@'.");

            bool idDuplicado = await _context.Proveedor.AnyAsync(p => p.Identificacion == proveedor.Identificacion);
            if (idDuplicado)
                ModelState.AddModelError("Identificacion", "Ya existe un proveedor con esa identificación.");

            if (!ModelState.IsValid)
                return View(proveedor);

            proveedor.Activo = true;
            _context.Add(proveedor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Proveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var proveedor = await _context.Proveedor.FindAsync(id);
            if (proveedor == null)
                return NotFound();

            return View(proveedor);
        }

        // POST: Proveedores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Proveedores proveedor)
        {
            if (id != proveedor.IdProveedor)
                return NotFound();

            if (!Regex.IsMatch(proveedor.Telefono ?? "", @"^\d{4}-\d{4}$"))
                ModelState.AddModelError("Telefono", "El teléfono debe tener el formato NNNN-NNNN.");

            if (string.IsNullOrWhiteSpace(proveedor.Correo) || !proveedor.Correo.Contains("@"))
                ModelState.AddModelError("Correo", "El correo debe contener un '@'.");

            bool idDuplicado = await _context.Proveedor.AnyAsync(p => p.IdProveedor != proveedor.IdProveedor && p.Identificacion == proveedor.Identificacion);
            if (idDuplicado)
                ModelState.AddModelError("Identificacion", "Ya existe otro proveedor con esa identificación.");

            if (!ModelState.IsValid)
                return View(proveedor);

            try
            {
                var proveedorBD = await _context.Proveedor.FindAsync(proveedor.IdProveedor);
                if (proveedorBD == null)
                    return NotFound();

                proveedorBD.Nombre = proveedor.Nombre;
                proveedorBD.Identificacion = proveedor.Identificacion;
                proveedorBD.Telefono = proveedor.Telefono;
                proveedorBD.Correo = proveedor.Correo;
                proveedorBD.Activo = proveedor.Activo;

                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest("Error actualizando proveedor.");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Proveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var proveedor = await _context.Proveedor.FirstOrDefaultAsync(p => p.IdProveedor == id);
            if (proveedor == null)
                return NotFound();

            return View(proveedor);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedor = await _context.Proveedor.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedor.Remove(proveedor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProveedorExists(int id)
        {
            return _context.Proveedor.Any(p => p.IdProveedor == id);
        }
    }
}
