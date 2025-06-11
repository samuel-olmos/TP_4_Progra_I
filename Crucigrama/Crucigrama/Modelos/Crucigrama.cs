using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crucigrama.Modelos
{
    internal class Crucigrama
    {
        public int Id { get; set; }
        public List<Palabra> PalabrasVertical;
        public List<Palabra> PalabrasHorizontal;

        public Crucigrama() 
        {
            PalabrasVertical = new List<Palabra>();
            PalabrasHorizontal = new List<Palabra>();
        }
        public Crucigrama(int id, List<Palabra> palabrasVertical, List<Palabra> palabrasHorizontal)
        {
            Id = id;
            PalabrasVertical = palabrasVertical;
            PalabrasHorizontal = palabrasHorizontal;
        }
    }
}
