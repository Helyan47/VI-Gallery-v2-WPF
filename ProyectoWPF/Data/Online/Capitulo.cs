using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleccionarProfile.Data.Online
{
    public class Capitulo
    {
        public long id { get; set; }
        public string nombre { get; set; }
        public string rutaWeb { get; set; }
        public int numEpisodio { get; set; }
        public long tiempoactual { get; set; }
        public DateTime fechaLanzamiento { get; set; }
        public long numVisitas { get; set; }
        public int fk_Temp { get; set; }

        public Capitulo(long id, string nombre, string rutaWeb, int numEpisodio, long tiempoactual, DateTime fechaLanzamiento, int numVisitas, int fk_Temp) {
            this.id = id;
            this.nombre = nombre;
            this.rutaWeb = rutaWeb;
            this.numEpisodio = numEpisodio;
            this.tiempoactual = tiempoactual;
            this.fechaLanzamiento = fechaLanzamiento;
            this.numVisitas = numVisitas;
            this.fk_Temp = fk_Temp;
        }
    }
}
