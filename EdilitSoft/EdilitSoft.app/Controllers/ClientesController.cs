using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EdilitSoft.app.Models;
using EdilitSoft.app.ServiciosJuanPa;

namespace EdilitSoft.app.Controllers
{
    public class ClientesController : Controller
    {
        // JuanPa: usamos el servicio en lugar del contexto directo
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteService.ObtenerTodos();
            return View(clientes);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var cliente = await _clienteService.ObtenerPorId(id.Value); // JuanPa
            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Clientes cliente)
        {
            if (ModelState.IsValid)
            {
                await _clienteService.Crear(cliente); // JuanPa
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var cliente = await _clienteService.ObtenerPorId(id.Value);
            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Clientes cliente)
        {
            if (id != cliente.IdCliente)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _clienteService.Editar(cliente); // JuanPa: ahora coincide con la interfaz
                }
                catch
                {
                    return BadRequest("Error actualizando cliente.");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var cliente = await _clienteService.ObtenerPorId(id.Value);
            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clienteService.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
