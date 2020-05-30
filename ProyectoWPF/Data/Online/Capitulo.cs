using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF.Data.Online
{
    public class Capitulo
    {
        public long id { get; set; }
        public string nombre { get; set; }
        public string rutaWeb { get; set; }
        public int numEpisodio { get; set; }
        public long tiempoactual { get; set; }
        public DateTime fechaLanzamiento { get; set; }
        public int numVisitas { get; set; }
        public int fk_Temp { get; set; }
    }
}
