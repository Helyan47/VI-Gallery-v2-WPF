using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleccionarProfile.Data {
    public class MenuClass {
        public long id { get; set; }
        public string nombre { get; set; }
        public long numCarps { get; set; }
        public long idPerfil { get; set; }
        

        public MenuClass(long id, string nombre, long idPerfil) {
            this.id = id;
            this.nombre = nombre;
            this.idPerfil = idPerfil;
            this.numCarps = 0;
        }

        public MenuClass(string nombre, long idPerfil) {
            this.id = -1;
            this.nombre = nombre;
            this.idPerfil = idPerfil;
            this.numCarps = 0;
        }

        public MenuClass(long id, string nombre, long numCarps, long idPerfil) {
            this.id = id;
            this.nombre = nombre;
            this.idPerfil = idPerfil;
            this.numCarps = numCarps;
        }
    }
}
