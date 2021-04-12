using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.BIZ
{
    public class ManejadorVales : IManejadorVales
    {
        IRepositorio<Vale> repositorio;

        public ManejadorVales(IRepositorio<Vale> repositorio)
        {
            this.repositorio = repositorio;
        }

        public List<Vale> Listar => repositorio.Read;
        
        public bool Agregar(Vale entidad)
        {
            return repositorio.Create(entidad);
        }

        public IEnumerable BuscarNoEntregadoPorEmpleado(Empleado empleado)
        {
            return repositorio.Read.Where(e => e.Solicitante.Id == empleado.Id && e.FechaEntegaReal==null).OrderBy(e => e.FechaEntrega);
        }

        public Vale BuscarPorId(string id)
        {
            return Listar.Where(e=> e.Id==id).SingleOrDefault();
        }

        public bool Eliminar(string id)
        {
            return repositorio.Delete(id);
        }

        public bool Modificar(Vale entidad)
        {
            return repositorio.Update(entidad);
        }

        public List<Vale> ValesEnIntervalo(DateTime inicio, DateTime fin)
        {
            return Listar.Where(e => e.FechaEntegaReal  >= inicio && e.FechaEntegaReal<= fin).ToList();
        }

        public List<Vale> ValesPorLiquidar()
        {
            return Listar.Where(e => e.FechaEntegaReal == null).ToList();
        }
    }
}
