using ProyectoWPF.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProyectoWPF {
    class SaveData {

        private  string _archivoData;
        public SaveData(string archivoData) {
            this._archivoData = archivoData;
        }

        public void saveData(string s,string name) {
            StreamWriter sw = new StreamWriter(_archivoData, true);
            sw.WriteLine(s);
            sw.Close();
        }

        public void saveFolder(Carpeta c) {
            SaveDataType carpetaData = new SaveDataType(c.getSerie().getTitle(), true, c.getDescripcion(), c.getRutaPrograma(), c.getSerie().getTipo(),c.getSerie().getDirImg());
            IFormatter formatter=new BinaryFormatter();
            using (FileStream stream = new FileStream("ArchivoData.txt", FileMode.OpenOrCreate, FileAccess.Write)) {
                stream.Seek(stream.Length, SeekOrigin.Begin);
                formatter.Serialize(stream, carpetaData);
            }

            Conexion.uploadFolder(c);
        }

        public void saveSubFolder(SubCarpeta c) {
            SaveDataType carpetaData = new SaveDataType(c.getTitle(), false, true, c.getRutaPrograma(), c.getSerie().getTipo(),c.getDirImg());
            IFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream("ArchivoData.txt", FileMode.OpenOrCreate, FileAccess.Write)) {
                stream.Seek(stream.Length, SeekOrigin.Begin);
                formatter.Serialize(stream, carpetaData);
            }
            Conexion.uploadSubFolder(c);
        }

        public ICollection<SaveDataType> loadFolders() {
            ICollection<SaveDataType> ic = new List<SaveDataType>();
            ICollection<SaveDataType> objects = new List<SaveDataType>();
            IFormatter formatter = new BinaryFormatter();
            if (File.Exists("ArchivoData.txt")) {
                using (FileStream stream = new FileStream("ArchivoData.txt", FileMode.Open, FileAccess.Read, FileShare.None)) {
                    while (stream.Position < stream.Length) {
                        SaveDataType aux = (SaveDataType)formatter.Deserialize(stream);
                        objects.Add(aux);
                    }
                }

                foreach (SaveDataType c in objects) {
                    if (c.isCarpeta()) {
                        if (!c.isSubCarpeta()) {
                            ic.Add(c);
                        }
                    }
                    
                }
            }
            

            return ic;
        }

        public ICollection<SaveDataType> loadSubFolders() {
            ICollection<SaveDataType> ic = new List<SaveDataType>();
            ICollection<SaveDataType> objects = new List<SaveDataType>();
            IFormatter formatter = new BinaryFormatter();
            if (File.Exists("ArchivoData.txt")) {
                using (FileStream stream = new FileStream("ArchivoData.txt", FileMode.Open, FileAccess.Read, FileShare.None)) {
                    while (stream.Position < stream.Length) {
                        SaveDataType aux = (SaveDataType)formatter.Deserialize(stream);
                        objects.Add(aux);
                    }
                }

                foreach (SaveDataType c in objects) {
                    if (c.isSubCarpeta()) {
                        ic.Add(c);
                        Console.WriteLine(c.getRutaPrograma());
                    }

                }
            }

            return ic;
        }

        public ICollection<Button> loadButtons(ICollection<SaveDataType> ic) {
            ICollection<Button> botones = new List<Button>();
            ICollection<string> items = new List<string>();

            foreach (SaveDataType c in ic) {
                if (c.isCarpeta()) {
                    if (!c.isSubCarpeta()) {
                        string tipo = c.getTipo();
                        bool exist = false;
                        foreach(string s in items) {
                            if (s.Equals(tipo)) {
                                exist = true;
                            }
                        }
                        if (!exist) {
                            items.Add(tipo);
                        }
                    }
                }

            }
            foreach(string s in items) {
                Button b = new Button();
                b.Height = 100;
                b.FontSize = 40;
                b.BorderThickness = new System.Windows.Thickness(0);
                b.FontWeight = FontWeights.Bold;
                b.Foreground = Brushes.White;
                b.Visibility = Visibility.Visible;
                b.Content = s;
                b.Name = s;
                b.Style = (Style)Application.Current.Resources["CustomButtonStyle"];
                botones.Add(b);
            }

            return botones;
        }
        
    }
}
