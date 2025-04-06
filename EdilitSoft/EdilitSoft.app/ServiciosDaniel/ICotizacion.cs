using EdilitSoft.app.Models;

namespace EdilitSoft.app.ServiciosDaniel
{
    public interface ICotizacion
    {
        Task<bool> Crear(Cotizaciones con);

        Task<Cotizaciones> BuscarXid(int? id);

        Task<bool> Modificar(Cotizaciones con);

        Task<bool> Eliminar(int id);

        Task<IEnumerable<Cotizaciones>> Listar();

        Task<bool> EventoExists(int id);
    }
}
