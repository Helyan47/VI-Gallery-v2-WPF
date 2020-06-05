using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleccionarProfile.Data {
    public class UsuarioClass {
        public long id { get; set; }
        public string nick { get; set; }

        public UsuarioClass(long id, string nick) {
            this.id = id;
            this.nick = nick;
        }

        public UsuarioClass(string nick) {
            this.id = -1;
            this.nick = nick;
        }
    }
}
