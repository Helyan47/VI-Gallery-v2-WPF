using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Reproductor {
    public class PlayerProperty {
        public Key value { get; set; }
        public string name { get; set; }
        public string descripcion { get; set; }

        public PlayerProperty(Key value, string name, string descripcion) {
            this.value = value;
            this.name = name;
            this.descripcion = descripcion;
        }
    }
}
