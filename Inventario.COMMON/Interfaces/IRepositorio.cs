using Inventario.COMMON.Entidades;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.COMMON.Interfaces
{
    public interface IRepositorio<T> where T:Base
    {
        bool Create(T entidad);
        List<T> Read { get; }
        bool Update(T entidadModificada);
        bool Delete(ObjectId id);
        
    }
}
