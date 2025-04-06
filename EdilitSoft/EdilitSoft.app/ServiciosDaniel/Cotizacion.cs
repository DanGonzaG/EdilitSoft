using EdilitSoft.app.Models;
using Microsoft.EntityFrameworkCore;

namespace EdilitSoft.app.ServiciosDaniel
{
    public class Cotizacion : ICotizacion
    {
        public readonly Contexto _context;

        public Cotizacion (Contexto context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Cotizaciones>> Listar()
        {
            var contexto = await _context.Cotizacion.Include(c => c.Cliente).Include(c => c.Proveedor).ToListAsync();
            return contexto;
        }
        public async Task<Cotizaciones> BuscarXid(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var cotizaciones = await _context.Cotizacion
                .Include(c => c.Cliente)
                .Include(c => c.Proveedor)
                .FirstOrDefaultAsync(m => m.IdCotizacion == id);
            if (cotizaciones == null)
            {
                return null;
            }

            return cotizaciones;
        }

        public async Task<bool> Crear(Cotizaciones con)
        {
            try
            {
                _context.Add(con);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
            
           
        }

        public async Task<bool> Modificar(Cotizaciones con)
        {
            try
            {
                _context.Update(con);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
           
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var coti = await BuscarXid(id);
                if (coti != null)
                {
                    _context.Cotizacion.Remove(coti);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }  

       
        public async Task<bool> EventoExists(int id)
        {
            throw new NotImplementedException();
        }
    }
}
