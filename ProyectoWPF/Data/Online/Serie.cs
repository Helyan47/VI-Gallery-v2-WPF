using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF.Data.Online
{
    public class Serie
    {
        public long id { get; set; }
        public string nombre { get; set; }
        public int numTemps { get; set; }
        public string descripcion { get; set; }
        public ICollection<string> generos { get; set; }
        public int fechaLanzamiento { get; set; }
        public string img { get; set; }

        public Serie(long id, string nombre, int numTemps, string descripcion, ICollection<string> generos, int fechaLanzamiento, string img) {
            this.id = id;
            this.nombre = nombre;
            this.numTemps = numTemps;
            this.descripcion = descripcion;
            this.generos = generos;
            this.fechaLanzamiento = fechaLanzamiento;
            this.img = img;
        }
    }
}
