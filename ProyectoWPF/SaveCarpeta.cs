using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF {
    [Serializable]
    class SaveCarpeta {
        private string name;
        private bool isFolder;
        private bool isSubFolder;
        private string descripcion;
        private string rutaPrograma;
        private string rutaArchivo;
        private string tipo;
        private string img;

        public SaveCarpeta(string name,bool isFolder,string descripcion,string rutaPrograma,string tipo, string dirImg) {
            this.name = name;
            this.isFolder = isFolder;
            this.descripcion = descripcion;
            this.rutaPrograma = rutaPrograma;
            this.tipo = tipo;
            this.img = dirImg;
        }
        public SaveCarpeta(string name, bool isFolder,bool isSubFolder, string rutaPrograma, string tipo, string img) {
            this.name = name;
            this.isFolder = isFolder;
            this.isSubFolder = isSubFolder;
            this.rutaPrograma = rutaPrograma;
            this.tipo = tipo;
            this.img = img;
        }
        public SaveCarpeta(string name, string rutaArchivo, bool isFolder, string rutaPrograma, string tipo) {
            this.name = name;
            this.isFolder = isFolder;
            this.rutaArchivo = rutaArchivo;
            this.rutaPrograma = rutaPrograma;
            this.tipo = tipo;
        }

        public string getName() {
            return name;
        }
        public string getDesc() {
            return descripcion;
        }
        public string getRutaPrograma() {
            return rutaPrograma;
        }
        public string getRutaArchivo() {
            return rutaArchivo;
        }
        public string getTipo() {
            return tipo;
        }

        public bool isCarpeta() {
            return isFolder;
        }

        public bool isSubCarpeta() {
            return isSubFolder;
        }

        public string getDirImg() {
            return img;
        }

    }
}
