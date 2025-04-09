using EdilitSoft.app.Data;
using EdilitSoft.app.Models;
using Microsoft.EntityFrameworkCore;

namespace EdilitSoft.app.ServiciosJuanPa
{
    public class ClienteService : IClienteService
    {
        private readonly Contexto _context;

        public ClienteService(Contexto context)
        {
            _context = context;
        }

        public async Task<List<Clientes>> ObtenerTodos()
        {
            return await _context.Cliente.ToListAsync();
        }

        public async Task<Clientes> ObtenerPorId(int id)
        {
            return await _context.Cliente.FirstOrDefaultAsync(c => c.IdCliente == id);
        }

        public async Task Crear(Clientes cliente)
        {
            cliente.Activo = true;
            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task Editar(Clientes cliente) // JuanPa: método necesario para Edit
        {
            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var cliente = await ObtenerPorId(id);
            if (cliente != null)
            {
                _context.Cliente.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }
    }
}
