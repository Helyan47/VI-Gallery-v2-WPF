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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoWPF.NewFolders {
    /// <summary>
    /// Lógica de interacción para ChangeName.xaml
    /// </summary>
    public partial class ChangeName : UserControl {
        Carpeta _folder;
        bool _mode;
        bool _nameChanged = false;
        Dictionary<string, bool> _generos = null;
        public ChangeName() {
            InitializeComponent();
            _nameChanged = false;
            genderSelection.addAcceptButtonEvent(hideGenderSelection);
            genderSelection.addCancelButtonEvent(cancelGenderSelection);
        }

        public void setCarpeta(Carpeta c) {
            this._folder = c;
            Title.Text = c.getClass().nombre;
            DescBox.Text = c.getClass().desc;
            dirImg.Text = c.getClass().img;
            genderText.Text = c.getClass().getGeneros();
            genderSelection.setMode(ActionPanel.MODIFY_FOLDER_MODE, c.getClass().ruta, null);
        }

        public void setFolderMode(bool mode) {
            this._mode = mode;
            if (this._mode) {
                rowGeneros.Height = new GridLength(3, GridUnitType.Star);
                rowDescripcion.Height = new GridLength(2, GridUnitType.Star);
            } else {
                rowGeneros.Height = new GridLength(0, GridUnitType.Star);
                rowDescripcion.Height = new GridLength(0, GridUnitType.Star);
            }
            
        }

        private void bAccept_Click(object sender, RoutedEventArgs e) {
            if (Title.Text.CompareTo("") != 0 && _folder != null) {
                Regex containsABadCharacter = new Regex("[" + Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())) + "]");
                if (!containsABadCharacter.IsMatch(Title.Text)) {
                    if (!_folder.getClass().nombre.Equals(Title.Text)) {
                        _nameChanged = true;
                    }
                    CarpetaClass c = new CarpetaClass(Title.Text, DescBox.Text, dirImg.Text, genderSelection.getGendersSelected().Keys.ToList<string>(), _folder.getClass().isFolder);

                    Metodos.notifyEndFolderUpdate(true, c);
                } else {
                    MessageBox.Show("El nombre contiene caractéres no permitidos: " + new string(System.IO.Path.GetInvalidFileNameChars()));
                }

            } else {
                MessageBox.Show("No has introducido ningún nombre para la carpeta");
            }

        }

        private void bCancel_Click(object sender, RoutedEventArgs e) {
            clearData();
            if (genderSelection.Visibility == Visibility.Hidden) {
                Metodos.notifyCanceled();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog f = new OpenFileDialog();
            if (f.ShowDialog() == true) {

                dirImg.Text = System.IO.Path.GetFullPath(f.FileName);
            }
        }

        public void hideGenderSelection(object sender, EventArgs e) {
            genderSelection.Visibility = Visibility.Hidden;
            List<string> genders = genderSelection.getGendersSelected().Keys.ToList<string>();
            string cadena = "";
            foreach (string s in genders) {
                cadena += s + ", ";
            }
            try {
                cadena = cadena.Substring(0, cadena.Length - 2);
            } catch (Exception) {
                cadena = "";
            }
            genderText.Text = cadena;
            _generos = genderSelection.getGendersSelected();
        }

        public void cancelGenderSelection(object sender, EventArgs e) {
            genderSelection.Visibility = Visibility.Hidden;
            genderSelection.clearData();
        }

        private void selectGender_Click(object sender, EventArgs e) {
            genderSelection.Visibility = Visibility.Visible;
            if (_generos != null) {
                genderSelection.setMode(ActionPanel.NEW_FOLDER_GENDER_MODE, null, _generos);
            }
            genderSelection.loadGenders();
        }

        public void changeGenderMode(long mode, string rutaCarpeta, Dictionary<string, bool> selectedGenders) {
            genderSelection.setMode(mode, rutaCarpeta, selectedGenders);
        }

        public void clearData() {
            Title.Text = "";
            DescBox.Text = "";
            dirImg.Text = "";
            genderText.Text = "";
            _generos = null;
            genderSelection.clearData();
        }
    }
}
