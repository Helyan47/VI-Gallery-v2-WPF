using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF.Data {
    public class PerfilClassOnline : PerfilClass{
        public long idUsuario { get; set; }

        public PerfilClassOnline(long id, string nombre, long mode, long numMenus, long idUsuario) : base(id, nombre, mode, numMenus) {
            this.idUsuario = idUsuario;
        }

        public PerfilClassOnline (string nombre, long mode, long idUsuario) : base(nombre, mode) {
            this.idUsuario = idUsuario;
        }

        public PerfilClassOnline(string nombre, long idUsuario) : base(nombre) {
            this.idUsuario = idUsuario;
        }

    }
}
