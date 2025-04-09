using EdilitSoft.app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EdilitSoft.app.ServiciosJuanPa
{
    public class ProveedorService : IProveedorService
    {
        private readonly Contexto _context;

        public ProveedorService(Contexto context)
        {
            _context = context;
        }

        public async Task<List<Proveedores>> ObtenerTodos()
        {
            return await _context.Proveedor.ToListAsync();
        }

        public async Task<Proveedores> ObtenerPorId(int id)
        {
            return await _context.Proveedor.FirstOrDefaultAsync(p => p.IdProveedor == id);
        }

        public async Task Crear(Proveedores proveedor)
        {
            proveedor.Activo = true;
            _context.Proveedor.Add(proveedor);
            await _context.SaveChangesAsync();
        }

        public async Task Editar(Proveedores proveedor)
        {
            _context.Entry(proveedor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Eliminar(int id)
        {
            var proveedor = await ObtenerPorId(id);
            if (proveedor != null)
            {
                _context.Proveedor.Remove(proveedor);
                await _context.SaveChangesAsync();
            }
        }
    }
}
