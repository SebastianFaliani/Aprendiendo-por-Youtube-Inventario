using Inventario.COMMON.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.COMMON.Interfaces
{
    public interface IManejadorVales:IManejadorGenerico<Vale>
    {
        List<Vale> ValesPorLiquidar();
        List<Vale> ValesEnIntervalo(DateTime inicio, DateTime fin);
    }
}
