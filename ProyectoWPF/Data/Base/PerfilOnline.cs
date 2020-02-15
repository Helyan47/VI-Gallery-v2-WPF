using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF.Data {
    
    public class PerfilOnline : PerfilClass {
        public long idUsuario { get; set; }

        public PerfilOnline(string nombre, long mode, long idUsuario) : base(nombre,mode) {
            this.idUsuario = idUsuario;
        }

        public PerfilOnline(long id,string nombre, long numMenus, long mode, long idUsuario) : base(id, nombre, numMenus, mode) {
            this.idUsuario = idUsuario;
        }
    }
}
