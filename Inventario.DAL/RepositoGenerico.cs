using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.DAL
{
    public class RepositoGenerico<T> : IRepositorio<T> where T : Base //Esto lo hace cuando las clases heredan de Base
    {

        private MongoClient client;
        private IMongoDatabase db;

        public RepositoGenerico()
        {
            // Paso la cadena de conexion a la base de datos

            client = new MongoClient("mongodb+srv://nedase:chuset@nedasoft.8pook.mongodb.net/videosRedesPlus?retryWrites=true&w=majority");
            db = client.GetDatabase("Inventario");
        }

        // Metodo para obtener la coleccion de datos
        private IMongoCollection<T> Coleccion()
        {
             return db.GetCollection<T>(typeof(T).Name); 
        }


        public bool Create(T entidad)
        {
            try
            {
                Coleccion().InsertOne(entidad);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public List<T> Read => Coleccion().AsQueryable().ToList();

        public bool Delete(ObjectId id)
        {
            try
            {
                return Coleccion().DeleteOne(e => e.Id == id).DeletedCount == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool Update(T entidadModificada)
        {
            try
            {
                return Coleccion().ReplaceOne(e=> e.Id==entidadModificada.Id,entidadModificada).ModifiedCount==1;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
