using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.COMMON.Entidades
{
    public abstract class Base
    {
        //public string Id { get; set; }

        /* Para usar MongoDB todas las entidades o clase necesitan un ID
         lo importante es que en este caso ese ID debe se Object y no string 
        como estabamos usando con LiteDB
        
         Tambien debo cambiarlo a tipo ObjectId en IRepositorio

         */
        public ObjectId Id { get; set; }

        
    }
}
