using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF.Data {
    public class PerfilClass {
        public long id { get; set; }
        public string nombre { get; set; }
        public long numMenus { get; set; }
        public long mode { get; set; }


        public PerfilClass(long id,string nombre,long numMenus, long mode) {
            this.id = id;
            this.nombre = nombre;
            this.numMenus = numMenus;
            this.mode = mode;
        }

        public PerfilClass(string nombre,long mode) {
            this.id = -1;
            this.nombre = nombre;
            this.mode = mode;
        }

    }
}
