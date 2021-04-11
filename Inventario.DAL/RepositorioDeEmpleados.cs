using Inventario.COMMON.Entidades;
using Inventario.COMMON.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.DAL
{
    public class RepositorioDeEmpleados : IRepositorio<Empleado>
    {
        private string DBName = @"C:\Users\Sebastian\source\repos\Inventario\DBInventario.db";
        private string TableName = "Empleados";

        public List<Empleado> Read
        {
            get 
            { 
                List<Empleado> datos = new List<Empleado>();
                using (var db = new LiteDatabase(DBName))
                {
                    datos = db.GetCollection<Empleado>(TableName).FindAll().ToList();
                }
                return datos;
            }
            
        }

        public bool Create(Empleado entidad)
        {
            entidad.Id = Guid.NewGuid().ToString(); //Esta instruccion genera un id(cadena) aleatorio de 37 caracteres que nunca se va a repetir
            try
            {
                using (var db=new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Empleado>(TableName);
                    coleccion.Insert(entidad);
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool Delete(string id)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Empleado>(TableName);
                    coleccion.Delete(id);
                 }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Empleado entidadModificada)
        {
            try
            {
                using (var db = new LiteDatabase(DBName))
                {
                    var coleccion = db.GetCollection<Empleado>(TableName);
                    coleccion.Update(entidadModificada);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
