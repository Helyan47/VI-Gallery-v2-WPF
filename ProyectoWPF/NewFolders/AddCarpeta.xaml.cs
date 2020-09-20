using Microsoft.Win32;
using ProyectoWPF.Components;
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
        private ComboBoxItem button;
        private bool created = false;
        public AddCarpeta(Carpeta p,ComboBoxItem b) {
            InitializeComponent();
            padre = p;
            carpeta = new CarpetaClass("", "",true);
            padre.setClass(carpeta);
            genderSelection.getAcceptButton().Click += hideGenderSelection;
            genderSelection.setMode(ActionPanel.NEW_FOLDER_GENDER_MODE, null,null);
            bAccept.getButton().Click += BAceptar_Click;
            button = b;
        }

        private void BAceptar_Click(object sender, EventArgs e) {

            if (Title.Text.CompareTo("") != 0) {
                Regex containsABadCharacter = new Regex("[" + Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())) + "]");
                if (!containsABadCharacter.IsMatch(Title.Text)) {
                    if (!Lista.Contains(VIGallery._profile.nombre + "|C/" + button.Content + "/" + Title.Text)) {

                        List<string> genders = genderSelection.getGendersSelected().Keys.ToList<string>();

                        if (genders != null) {
                            carpeta = new CarpetaClass(Title.Text, DescBox.Text, dirImg.Text, genders, true);
                            carpeta.idMenu = Lista.getMenuFromText(button.Content.ToString()).id;
                        } else {
                            carpeta = new CarpetaClass(Title.Text, DescBox.Text, dirImg.Text, true);
                            carpeta.idMenu = Lista.getMenuFromText(button.Content.ToString()).id;
                        }
                        carpeta.idMenu = Lista.getMenuFromText(button.Content.ToString()).id;
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

        private void selectGender_Click(object sender, EventArgs e) {
            genderSelection.Visibility = Visibility.Visible;
            genderSelection.loadGenders();
        }

        public void hideGenderSelection(object sender, EventArgs e) {
            genderSelection.Visibility = Visibility.Hidden;
            List<string> genders = genderSelection.getGendersSelected().Keys.ToList<string>();
            string cadena = "";
            foreach(string s in genders) {
                cadena += s + ", ";
            }
            cadena = cadena.Substring(0, cadena.Length - 2);
            genderText.Text = cadena;
        }
    }
}
