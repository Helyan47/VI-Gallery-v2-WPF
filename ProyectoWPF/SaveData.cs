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

        string archivoData;
        public SaveData(string archivoData) {
            this.archivoData = archivoData;
        }

        public void saveData(string s,string name) {
            StreamWriter sw = new StreamWriter(archivoData, true);
            sw.WriteLine(s);
            sw.Close();
        }

        public void saveFolder(Carpeta c) {
            SaveCarpeta carpetaData = new SaveCarpeta(c.getSerie().getTitle(), true, c.getDescripcion(), c.getRutaPrograma(), c.getSerie().getTipo(),c.getSerie().getDirImg());
            IFormatter formatter=new BinaryFormatter();
            using (FileStream stream = new FileStream("ArchivoData.txt", FileMode.OpenOrCreate, FileAccess.Write)) {
                stream.Seek(stream.Length, SeekOrigin.Begin);
                formatter.Serialize(stream, carpetaData);
            }
        }

        public void saveSubFolder(SubCarpeta c) {
            SaveCarpeta carpetaData = new SaveCarpeta(c.getSerie().getTitle(), true, true, c.getRutaPrograma(), c.getSerie().getTipo(),c.getDirImg());
            IFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream("ArchivoData.txt", FileMode.OpenOrCreate, FileAccess.Write)) {
                stream.Seek(stream.Length, SeekOrigin.Begin);
                formatter.Serialize(stream, carpetaData);
            }
        }

        public ICollection<SaveCarpeta> loadFolders() {
            ICollection<SaveCarpeta> ic = new List<SaveCarpeta>();
            ICollection<SaveCarpeta> objects = new List<SaveCarpeta>();
            IFormatter formatter = new BinaryFormatter();
            if (File.Exists("ArchivoData.txt")) {
                using (FileStream stream = new FileStream("ArchivoData.txt", FileMode.Open, FileAccess.Read, FileShare.None)) {
                    while (stream.Position < stream.Length) {
                        SaveCarpeta aux = (SaveCarpeta)formatter.Deserialize(stream);
                        objects.Add(aux);
                    }
                }

                foreach (SaveCarpeta c in objects) {
                    if (c.isCarpeta()) {
                        if (!c.isSubCarpeta()) {
                            ic.Add(c);
                        }
                    }
                    
                }
            }
            

            return ic;
        }

        public ICollection<SaveCarpeta> loadSubFolders() {
            ICollection<SaveCarpeta> ic = new List<SaveCarpeta>();
            ICollection<SaveCarpeta> objects = new List<SaveCarpeta>();
            IFormatter formatter = new BinaryFormatter();
            if (File.Exists("ArchivoData.txt")) {
                using (FileStream stream = new FileStream("ArchivoData.txt", FileMode.Open, FileAccess.Read, FileShare.None)) {
                    while (stream.Position < stream.Length) {
                        SaveCarpeta aux = (SaveCarpeta)formatter.Deserialize(stream);
                        objects.Add(aux);
                    }
                }

                foreach (SaveCarpeta c in objects) {
                    if (c.isCarpeta()) {
                        if (c.isSubCarpeta()) {
                            ic.Add(c);
                            Console.WriteLine(c.getRutaPrograma());
                        }
                    }

                }
            }

            return ic;
        }

        public ICollection<Button> loadButtons(ICollection<SaveCarpeta> ic) {
            ICollection<Button> botones = new List<Button>();
            ICollection<string> items = new List<string>();

            foreach (SaveCarpeta c in ic) {
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
                //Height="100" FontSize="40" BorderThickness="0" FontWeight="Bold" Foreground="White" Click="onClickButtonMenu" BorderBrush="#FF252525"
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

        public void saveFile() {

        }

        public string getButton(string linea) {
            string name = "";
            string[] names=linea.Split('/');
            name=names[2];
            return name;
        }

        public string getNameFolder(string linea) {
            string name = "";
            string[] names = linea.Split('/');
            string lastName = names[names.Length - 1];
            if (!lastName.Contains("^")) {
                name = lastName;
            } else {
                string[] nameAndFile = lastName.Split('^');
                name = nameAndFile[0];
            }
            return name;
        }

        public bool isCarpeta(string linea) {
            bool comp= false;
            string[] names = linea.Split('/');
            if (names.Length == 3) {
                comp = true;
            }
            return comp;
        }

        public string getNameCarpeta(string linea) {
            string name = "";
            string[] names = linea.Split('/');
            if (names.Length == 3) {
                name = names[3];
            }
            return name;
        }
        
    }
}
