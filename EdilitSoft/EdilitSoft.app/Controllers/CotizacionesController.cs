using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EdilitSoft.app.Models;
using EdilitSoft.app.ServiciosDaniel;


namespace EdilitSoft.app.Controllers
{
    public class CotizacionesController : Controller
    {
        private readonly Contexto _context;
        private readonly ICotizacion _cotizacion;

        public CotizacionesController(Contexto context, ICotizacion cotizacion )
        {
            _context = context;
            _cotizacion = cotizacion;
        }

        // GET: Cotizaciones
        public async Task<IActionResult> Index()
        {
            //var contexto = _context.Cotizacion.Include(c => c.Cliente).Include(c => c.Proveedor);
            return View(await _cotizacion.Listar());
        }

        // GET: Cotizaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var coti = await _cotizacion.BuscarXid(id);
            return View(coti);
        }

        // GET: Cotizaciones/Create
        public IActionResult Create()
        {
            ViewData["IdCliente"] = new SelectList(_context.Cliente, "IdCliente", "Nombre");
            ViewData["IdProovedor"] = new SelectList(_context.Proveedor, "IdProveedor", "Nombre");
            return View();
        }

        // POST: Cotizaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCotizacion,IdArticulo,IdProovedor,IdCliente,IdUsuario,Transporte,OtrosCostos,Ganancia,Total,Activo")] Cotizaciones cotizaciones)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(cotizaciones);
                //await _context.SaveChangesAsync();
                await _cotizacion.Crear(cotizaciones);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCliente"] = new SelectList(_context.Cliente, "IdCliente", "Correo", cotizaciones.IdCliente);
            ViewData["IdProovedor"] = new SelectList(_context.Proveedor, "IdProveedor", "Correo", cotizaciones.IdProovedor);
            return View(cotizaciones);
        }

        // GET: Cotizaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var cotizaciones = await _context.Cotizacion.FindAsync(id);
            var cotizaciones = await _cotizacion.BuscarXid(id);
            if (cotizaciones == null)
            {
                return NotFound();
            }
            ViewData["IdCliente"] = new SelectList(_context.Cliente, "IdCliente", "Correo", cotizaciones.IdCliente);
            ViewData["IdProovedor"] = new SelectList(_context.Proveedor, "IdProveedor", "Correo", cotizaciones.IdProovedor);
            return View(cotizaciones);
        }

        // POST: Cotizaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCotizacion,IdArticulo,IdProovedor,IdCliente,IdUsuario,Transporte,OtrosCostos,Ganancia,Total,Activo")] Cotizaciones cotizaciones)
        {
            if (id != cotizaciones.IdCotizacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(cotizaciones);
                    //await _context.SaveChangesAsync();
                    await _cotizacion.Modificar(cotizaciones);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (/*!CotizacionesExists(cotizaciones.IdCotizacion)*/
                        !await _cotizacion.EventoExists(cotizaciones.IdCotizacion))
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
            ViewData["IdCliente"] = new SelectList(_context.Cliente, "IdCliente", "Correo", cotizaciones.IdCliente);
            ViewData["IdProovedor"] = new SelectList(_context.Proveedor, "IdProveedor", "Correo", cotizaciones.IdProovedor);
            return View(cotizaciones);
        }

        // GET: Cotizaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cotizaciones = await _cotizacion.BuscarXid(id);
            if (cotizaciones == null)
            {
                return NotFound();
            }

            return View(cotizaciones);
        }

        // POST: Cotizaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var cotizaciones = await _context.Cotizacion.FindAsync(id);
            /*if (cotizaciones != null)
            {
                _context.Cotizacion.Remove(cotizaciones);
            }*/

            await _cotizacion.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }        
    }
}
