using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen.Entidades
{
    public class Tipo
    {

        public int TipoID { get; set; }
        public string NombreTipo { get; set; }
        public string TipoAudiencia { get; set; }

        // Propiedades de navegación
        public int AnimeID { get; set; } // Clave foránea
        public Anime Anime { get; set; } // Propiedad de navegación

    }
}
