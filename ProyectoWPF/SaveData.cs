using System.IO;

namespace ProyectoWPF {
    class SaveData {

        string archivoData;
        public SaveData(string archivoData) {
            this.archivoData = archivoData;
        }

        public void saveData(string s,string name) {
            if (!File.Exists(this.archivoData)) {
                File.Create(this.archivoData);
            }
            StreamWriter sw = new StreamWriter(archivoData, true);
            sw.WriteLine(s);
            sw.Close();
        }

        
    }
}
