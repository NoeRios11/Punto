using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Punto
{
    public class Conexion
    {
        private string cadenaConexion = "Server=localhost; Database=PuntoDB; Uid=root; Pwd=; Port=3306;";

        public MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }


    }
}
