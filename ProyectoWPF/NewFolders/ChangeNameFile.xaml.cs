using Microsoft.Win32;
using SeleccionarProfile.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SeleccionarProfile.NewFolders {
    /// <summary>
    /// Lógica de interacción para ChangeNameFile.xaml
    /// </summary>
    public partial class ChangeNameFile : Window {
        List<ArchivoClass> _archivos;
        private string name = null;
        private string img = "";
        public ChangeNameFile(List<ArchivoClass> archivos) {
            InitializeComponent();
            _archivos = archivos;
        }

        private void BAceptar_Click(object sender, EventArgs e) {
            if (Title.Text.CompareTo("") != 0) {
                Regex containsABadCharacter = new Regex("[" + Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())) + "]");
                if (!containsABadCharacter.IsMatch(Title.Text)) {
                    if (_archivos != null) {
                        bool exist = false;
                        foreach (ArchivoClass ac in _archivos) {
                            if (ac.nombre.Equals(Title.Text)) {
                                exist = true;
                            }
                        }
                        if (!exist) {
                            name = Title.Text;
                            img = dirImg.Text;
                            this.Close();
                        } else {
                            MessageBox.Show("Un archivo con ese nombre ya existe");
                        }

                    } else {
                        MessageBox.Show("Se ha producido un error al renombrar el archivo");
                    }
                }
            } else {
                MessageBox.Show("No has introducido ningún nombre para la carpeta");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog f = new OpenFileDialog();
            if (f.ShowDialog() == true) {

                dirImg.Text = System.IO.Path.GetFullPath(f.FileName);
            }
        }

        public string getNewName() {
            return name;
        }

        public void setName(string s) {
            Title.Text = s;
        }

        public string getDirImg() {
            return img;
        }

        public void setImg(string s) {
            dirImg.Text = s;
        }
    }
}
