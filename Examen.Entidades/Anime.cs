using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Entidades
{
    public class Anime
    {

        public int AnimeID { get; set; }
        public string NombreAnime { get; set; }
        public DateTime FechaEstreno { get; set; }
        public List<Tipo> Tipo { get; set; } // Relación uno a muchos


    }
}
