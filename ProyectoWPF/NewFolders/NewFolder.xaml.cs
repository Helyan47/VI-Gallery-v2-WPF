using Microsoft.Win32;
using ProyectoWPF.Components;
using ProyectoWPF.Data;
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

namespace ProyectoWPF.NewFolders {
    /// <summary>
    /// Lógica de interacción para NewCarpeta.xaml
    /// </summary>
    public partial class NewFolder : UserControl {

        private CarpetaClass carpeta;
        private Carpeta padre;
        private ComboBoxItem button;
        private Dictionary<string, bool> gendersSelected;
        public ActionPanel _actionPanel = null;

        public NewFolder() {
            InitializeComponent();
            genderSelection.setMode(ActionPanel.NEW_FOLDER_GENDER_MODE, null, null);
        }

        public void setData(Carpeta p, ComboBoxItem b) {
            carpeta = new CarpetaClass("", "", true);
            this.padre = p;
            this.button = b;
            padre.setClass(carpeta);
        }

        public Carpeta getCarpeta() {
            return this.padre;
        }

        private void bAccept_Click(object sender, RoutedEventArgs e) {
            if(genderSelection.Visibility == Visibility.Hidden) {
                Metodos.notifyNewFolder();
            }
        }

        private void bCancel_Click(object sender, RoutedEventArgs e) {
            if (genderSelection.Visibility == Visibility.Hidden) {
                Metodos.notifyCanceled();
            }
        }

        public bool checkNewData() {
            if (Title.Text.CompareTo("") != 0) {
                Regex containsABadCharacter = new Regex("[" + Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())) + "]");
                if (!containsABadCharacter.IsMatch(Title.Text)) {
                    if (!Lista.Contains(VIGallery._profile.nombre + "|C/" + button.Content + "/" + Title.Text)) {
                        return true;
                    } else {
                        MessageBox.Show("Ya existe la carpeta. Introduce otro nombre");
                        return false;
                    }
                } else {
                    MessageBox.Show("El nombre contiene caractéres no permitidos: " + new string(System.IO.Path.GetInvalidFileNameChars()));
                    return false;
                }


            } else {
                MessageBox.Show("No has introducido ningun nombre");
                return false;
            }
        }

        public Carpeta getBuildedFolder() {
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
            if (_actionPanel != null) {
                _actionPanel.close();
            }
            return padre;
        }

        public void clearData() {
            padre = null;
            carpeta = null;
            button = null;
            gendersSelected = null;
            genderSelection.clearData();
            genderSelection.Visibility = Visibility.Hidden;
            Title.Text = "";
            DescBox.Text = "";
            dirImg.Text = "";
            genderText.Text = "";
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

        private void selectGender_Click(object sender, EventArgs e) {
            genderSelection.Visibility = Visibility.Visible;
            if (gendersSelected != null) {
                genderSelection.setMode(ActionPanel.NEW_FOLDER_GENDER_MODE, null, gendersSelected);
            }
            genderSelection.loadGenders();
        }

        public void hideGenderSelection() {
            genderSelection.Visibility = Visibility.Hidden;
            gendersSelected = genderSelection.getGendersSelected();
            List<string> genders = genderSelection.getGendersSelected().Keys.ToList<string>();
            string cadena = "";
            foreach (string s in genders) {
                cadena += s + ", ";
            }
            try { 
            cadena = cadena.Substring(0, cadena.Length - 2);
            }catch(Exception){
                cadena = "";
            }
            genderText.Text = cadena;
        }

        
    }
}
