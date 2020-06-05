using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleccionarProfile.Data {
    public class PerfilClass {
        public long id { get; set; }
        public string nombre { get; set; }
        public long numMenus { get; set; }
        public long mode { get; set; }


        public PerfilClass(long id,string nombre, long mode, long numMenus) {
            this.id = id;
            this.nombre = nombre;
            this.numMenus = numMenus;
            this.mode = mode;
        }

        public PerfilClass(string nombre,long mode) {
            this.nombre = nombre;
            this.numMenus = 0;
            this.mode = mode;
        }

        public PerfilClass(string nombre) {
            this.nombre = nombre;
            this.numMenus = 0;
            this.mode = 0;
        }

    }
}
