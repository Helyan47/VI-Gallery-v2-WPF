using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF.Data.Online
{
    public class Temporada
    {
        public long id { get; set; }
        public int numTemporada { get; set; }
        public int numCaps { get; set; }
        public string rutaWeb { get; set; }
        public int year { get; set; }
        public string img { get; set; }
        public int fk_Serie { get; set; }
    }
}
