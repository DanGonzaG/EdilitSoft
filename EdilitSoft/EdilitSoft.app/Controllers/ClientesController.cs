using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EdilitSoft.app.Models;
using EdilitSoft.app.ServiciosJuanPa;
using System.Text.RegularExpressions;

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

            var cliente = await _clienteService.ObtenerPorId(id.Value);
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
            // Validar formato teléfono
            if (!Regex.IsMatch(cliente.Telefono ?? "", @"^\d{4}-\d{4}$"))
                ModelState.AddModelError("Telefono", "El teléfono debe tener el formato NNNN-NNNN.");

            // Validar correo contiene @
            if (string.IsNullOrWhiteSpace(cliente.Correo) || !cliente.Correo.Contains("@"))
                ModelState.AddModelError("Correo", "El correo debe contener un '@'.");

            // Validar identificación duplicada
            bool identificacionDuplicada = (await _clienteService.ObtenerTodos())
                .Any(c => c.Identificacion == cliente.Identificacion);
            if (identificacionDuplicada)
                ModelState.AddModelError("Identificacion", "Ya existe un cliente con esa identificación.");

            if (!ModelState.IsValid)
                return View(cliente);

            await _clienteService.Crear(cliente);
            return RedirectToAction(nameof(Index));
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

            // Validar formato teléfono
            if (!Regex.IsMatch(cliente.Telefono ?? "", @"^\d{4}-\d{4}$"))
                ModelState.AddModelError("Telefono", "El teléfono debe tener el formato NNNN-NNNN.");

            // Validar correo contiene @
            if (string.IsNullOrWhiteSpace(cliente.Correo) || !cliente.Correo.Contains("@"))
                ModelState.AddModelError("Correo", "El correo debe contener un '@'.");

            // Validar identificación duplicada
            bool identificacionDuplicada = (await _clienteService.ObtenerTodos())
                .Any(c => c.IdCliente != cliente.IdCliente && c.Identificacion == cliente.Identificacion);
            if (identificacionDuplicada)
                ModelState.AddModelError("Identificacion", "Ya existe otro cliente con esa identificación.");

            if (!ModelState.IsValid)
                return View(cliente);

            try
            {
                await _clienteService.Editar(cliente);
            }
            catch
            {
                return BadRequest("Error actualizando cliente.");
            }
            return RedirectToAction(nameof(Index));
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
