using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF.Data {
    public class UsuarioClass {
        public long id { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string nick { get; set; }
        public string email { get; set; }

        public UsuarioClass(long id, string nombre, string apellidos, string nick, string email) {
            this.id = id;
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.nick = nick;
            this.email = email;
        }

        public UsuarioClass(string nombre, string apellidos, string nick, string email) {
            this.id = -1;
            this.nombre = nombre;
            this.apellidos = apellidos;
            this.nick = nick;
            this.email = email;
        }
    }
}
