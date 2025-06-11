using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucigrama.Modelos
{
    internal class Palabra
    {
        public int Id { get; set; }
        public string Enunciado { get; set; }
        public string  Texto { get; set; }

        public Palabra() { }
        public Palabra(int id, string enunciado, string texto)
        {
            Id = id;
            Enunciado = enunciado;
            Texto = texto;
        }
    }
}
