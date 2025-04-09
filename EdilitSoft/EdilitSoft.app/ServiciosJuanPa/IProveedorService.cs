using EdilitSoft.app.Models;
using Microsoft.AspNetCore.Mvc;

namespace EdilitSoft.app.ServiciosJuanPa
{
    public interface IProveedorService
    {
        Task<List<Proveedores>> ObtenerTodos();
        Task<Proveedores> ObtenerPorId(int id);
        Task Crear(Proveedores proveedor);
        Task Editar(Proveedores proveedor);
        Task Eliminar(int id);
    }
}
