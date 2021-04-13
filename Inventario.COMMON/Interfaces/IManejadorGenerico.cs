using Inventario.COMMON.Entidades;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.COMMON.Interfaces
{
    public interface IManejadorGenerico<T> where T:Base
    {
        bool Agregar(T entidad);
        List<T> Listar { get; }
        bool Eliminar(ObjectId id);
        bool Modificar(T entidad);
        T BuscarPorId(ObjectId id);
    }
}
