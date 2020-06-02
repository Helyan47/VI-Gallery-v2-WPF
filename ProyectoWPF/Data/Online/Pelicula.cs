using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF.Data.Online
{
    public class Pelicula
    {
        public long id { get; set; }
        public string nombre { get; set; }
        public string rutaWeb { get; set; }
        public string descripcion { get; set; }
        public ICollection<string> generos { get; set; }
        public long tiempoactual { get; set; }
        public DateTime fechaLanzamiento { get; set; }
        public int numVisitas { get; set; }
        public string img { get; set; }

        public Pelicula(long id, string nombre, string rutaWeb, string descripcion, ICollection<string> generos, long tiempoactual, DateTime fechaLanzamiento, int numVisitas, string img) {
            this.id = id;
            this.nombre = nombre;
            this.rutaWeb = rutaWeb;
            this.descripcion = descripcion;
            this.generos = generos;
            this.tiempoactual = tiempoactual;
            this.fechaLanzamiento = fechaLanzamiento;
            this.numVisitas = numVisitas;
            this.img = img;
        }
    }
}
