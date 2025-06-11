using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucigrama.Modelos
{
    internal enum Rol
    {
        Jugador, Administrador
    }
    internal class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Contraseña { get; set; }
        public Rol Rol { get; set; }
        public Usuario() { }
        public Usuario(int id, string nombre, string contraseña, Rol rol)
        {
            Id = id;
            Nombre = nombre;
            Contraseña = contraseña;
            Rol = rol;
        }
    }
}
