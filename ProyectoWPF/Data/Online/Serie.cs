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
        public string rutaWeb { get; set; }
        public int year { get; set; }
        public ICollection<string> generos { get; set; }
        public int fechaLanzamiento { get; set; }
        public string img { get; set; }
    }
}
