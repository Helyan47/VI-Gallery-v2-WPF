using Microsoft.Win32;
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

namespace ProyectoWPF {
    /// <summary>
    /// Lógica de interacción para AddCarpeta.xaml
    /// </summary>
    public partial class AddCarpeta : Window {

        private CarpetaClass carpeta;
        private Carpeta padre;
        private Button button;
        private bool created = false;
        public AddCarpeta(Carpeta p,Button b) {
            InitializeComponent();
            padre = p;
            carpeta = new CarpetaClass("", "",true);
            padre.setClass(carpeta);
            button = b;
        }

        private void BAceptar_Click(object sender, EventArgs e) {

            if (Title.Text.CompareTo("") != 0) {
                Regex containsABadCharacter = new Regex("[" + Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())) + "]");
                if (!containsABadCharacter.IsMatch(Title.Text)) {
                    if (!Lista.Contains(VIGallery._profile.nombre + "|C/" + button.Content + "/" + Title.Text)) {
                        ICollection<string> col = new List<string>();

                        UIElementCollection coleccion = ListGeneros.Children;
                        bool isCheked = false;
                        foreach (CheckBox cb in coleccion) {

                            if (cb.IsChecked == true) {
                                col.Add((string)cb.Content);
                                isCheked = true;
                            }
                        }

                        if (isCheked) {

                            if (!dirImg.Equals("")) {
                                carpeta = new CarpetaClass(Title.Text, DescBox.Text, dirImg.Text, col, true);
                                carpeta.idMenu = Lista.getMenuFromButton(button).id;
                            }
                        } else {
                            if (!dirImg.Equals("")) {
                                carpeta = new CarpetaClass(Title.Text, DescBox.Text, dirImg.Text, true);

                            }
                        }
                        carpeta.idMenu = Lista.getMenuFromButton(button).id;
                        carpeta.ruta = "C/" + button.Content + "/" + padre.getTitle();
                        padre.setClass(carpeta);
                        Lista.addCarpetaClass(carpeta);
                        created = true;
                        this.Close();
                    } else {
                        MessageBox.Show("Ya existe la carpeta. Introduce otro nombre");
                    }
                } else {
                    MessageBox.Show("El nombre contiene caractéres no permitidos: " + new string(System.IO.Path.GetInvalidFileNameChars()));
                }


            } else {
                MessageBox.Show("No has introducido ningun nombre");
            }

        }


        private void Button_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog f = new OpenFileDialog();
            if (f.ShowDialog() == true) {

                dirImg.Text = System.IO.Path.GetFullPath(f.FileName);
            }
        }

        public CarpetaClass GetSerie() {
            return this.carpeta;
        }

        public void setPadre(Carpeta padre1) {

            padre = padre1;
        }

        public bool createdSerie() {
            return created;
        }
    }
}
