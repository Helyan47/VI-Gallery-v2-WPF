using ProyectoWPF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF {
    public class CarpetaClass {
        public long id { get; set; }
        public string nombre { get; set; }
        public int numSubCarps { get; set; }
        public int numArchivos { get; set; }
        public string ruta { get; set; }
        public string rutaPadre { get; set; }
        public string desc { get; set; }
        public ICollection<string> generos { get; set; }
        public string img { get; set; }
        public bool isFolder { get; set; }
        public long idMenu { get; set; }

        public CarpetaClass(Int64 id, string nombre, string ruta, string rutaPadre, Int64 numSubCarps, Int64 numArchivos, string desc, string img, string generos, bool isFolder, Int64 idMenu) {
            this.id = id;
            this.nombre = nombre;
            this.numSubCarps = (int)numSubCarps;
            this.numArchivos = (int)numArchivos;
            this.ruta = ruta;
            this.rutaPadre = rutaPadre;
            this.desc = desc;
            this.img = img;
            this.generos = new List<string>();
            this.isFolder = isFolder;
            this.idMenu = idMenu;
        }

        public CarpetaClass(Int64 id, string nombre, string ruta, string rutaPadre, Int64 numSubCarps, Int64 numArchivos, string desc, string img, string generos, Int64 isFolder, Int64 idMenu) {
            this.id = id;
            this.nombre = nombre;
            this.numSubCarps = (int)numSubCarps;
            this.numArchivos = (int)numArchivos;
            this.ruta = ruta;
            this.rutaPadre = rutaPadre;
            this.desc = desc;
            this.img = img;
            this.generos = new List<string>();
            if (isFolder == 0) {
                this.isFolder = false;
            } else {
                this.isFolder = true;
            }
            this.idMenu = idMenu;
        }

        public CarpetaClass(string title, string desc, string dirImg, ICollection<string> generos, bool esCarpeta) {

            this.nombre = title;
            this.desc = desc;
            this.img = dirImg;
            this.generos = generos;
            this.numSubCarps = 0;
            this.numArchivos = 0;
            this.isFolder = esCarpeta;
        }
        public CarpetaClass(string title, string desc, string dirImg, bool esCarpeta) {

            this.nombre = title;
            this.desc = desc;
            this.img = dirImg;
            this.generos = new List<string>();
            this.numSubCarps = 0;
            this.numArchivos = 0;
            this.isFolder = esCarpeta;
        }
        public CarpetaClass(string title, string desc, bool esCarpeta) {

            this.nombre = title;
            this.desc = desc;
            this.img = "";
            this.generos = new List<string>();
            this.numSubCarps = 0;
            this.numArchivos = 0;
            this.isFolder = esCarpeta;
        }

        public CarpetaClass(long idMenu) {
            this.nombre = "";
            this.desc = "";
            this.img = "";
            this.generos = new List<string>();
            this.numSubCarps = 0;
            this.numArchivos = 0;
            this.isFolder = false;
        }

        public void increaseSubCarpetas() {
            this.numSubCarps++;
        }

        public void decreaseSubCarpetas() {
            this.numSubCarps--;
        }

        public void increaseArchivos() {
            this.numArchivos++;
        }

        public void decraseArchivos() {
            this.numArchivos--;
        }

    }
}