using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoWPF {
    [Serializable]
    public class SaveDataType {
        private string name;
        private bool isFolder;
        private bool isSubFolder;
        private bool isFile;
        private string descripcion;
        private string rutaPrograma;
        private string rutaArchivo;
        private string tipo;
        private string img;
        private string profile;

        public SaveDataType(string name,bool isFolder,string descripcion,string rutaPrograma,string tipo, string dirImg) {
            this.name = name;
            this.isFolder = isFolder;
            if (isFolder == true) {
                this.isSubFolder = false;
                this.isFile = false;
            }
            this.descripcion = descripcion;
            this.rutaPrograma = rutaPrograma;
            this.tipo = tipo;
            this.img = dirImg;
        }
        public SaveDataType(string name, bool isFolder,bool isSubFolder, string rutaPrograma, string tipo, string img) {
            this.name = name;
            this.isFolder = isFolder;
            this.isSubFolder = isSubFolder;
            this.rutaPrograma = rutaPrograma;
            this.tipo = tipo;
            this.img = img;
        }

        public SaveDataType(string name, string rutaArchivo, string rutaPrograma, string tipo, string img, bool isFile) {
            this.name = name;
            this.isFile = isFile;
            if (isFile) {
                this.isFolder = false;
                this.isSubFolder = false;
            }
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
