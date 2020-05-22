using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF.Data {
    public class ArchivoClass {
        public long id { get; set; }
        public string nombre { get; set; }
        public string rutaPrograma { get; set; }
        public TimeSpan tiempoActual { get; set; }
        public string img { get; set; }
        public long idCarpeta { get; set; }

        public ArchivoClass(long id, string nombre, string rutaPrograma, TimeSpan tiempoActual, string img, long carpeta) {
            this.id = id;
            this.nombre = nombre;
            this.rutaPrograma = rutaPrograma;
            this.tiempoActual = tiempoActual;
            this.img = img;
            this.idCarpeta = carpeta;
        }

        public ArchivoClass(string nombre, string rutaPrograma, TimeSpan tiempoActual, string img, long carpeta) {
            this.id = id;
            this.nombre = nombre;
            this.rutaPrograma = rutaPrograma;
            this.tiempoActual = tiempoActual;
            this.img = img;
            this.idCarpeta = carpeta;
        }

        public ArchivoClass(long id, string nombre, string rutaPrograma, string tiempoActual, string img, long carpeta) {
            this.id = id;
            this.nombre = nombre;
            this.rutaPrograma = rutaPrograma;
            try {
                this.tiempoActual = TimeSpan.Parse(tiempoActual);
            } catch (FormatException) {
                Console.WriteLine("{0}: Bad Format", tiempoActual);
                this.tiempoActual = new TimeSpan(0, 0, 0);
            } catch (OverflowException) {
                Console.WriteLine("{0}: Overflow", tiempoActual);
                this.tiempoActual = new TimeSpan(0, 0, 0);
            }
            this.img = img;
            this.idCarpeta = carpeta;
        }

        public ArchivoClass(string nombre, string rutaPrograma, string tiempoActual, string img, long carpeta) {
            this.nombre = nombre;
            this.rutaPrograma = rutaPrograma;
            try {
                this.tiempoActual = TimeSpan.Parse(tiempoActual);
            } catch (FormatException) {
                Console.WriteLine("{0}: Bad Format", tiempoActual);
                this.tiempoActual = new TimeSpan(0, 0, 0);
            } catch (OverflowException) {
                Console.WriteLine("{0}: Overflow", tiempoActual);
                this.tiempoActual = new TimeSpan(0, 0, 0);
            }
            this.img = img;
            this.idCarpeta = carpeta;
        }
    }
}
