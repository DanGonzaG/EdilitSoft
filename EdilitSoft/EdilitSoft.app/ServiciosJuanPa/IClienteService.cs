using EdilitSoft.app.Models;

namespace EdilitSoft.app.ServiciosJuanPa
{
    public interface IClienteService
    {
        Task<List<Clientes>> ObtenerTodos();
        Task<Clientes> ObtenerPorId(int id);
        Task Crear(Clientes cliente);
        Task Editar(Clientes cliente); // JuanPa: renombrado desde 'Actualizar' para coincidir con el controlador
        Task Eliminar(int id);
    }
}
