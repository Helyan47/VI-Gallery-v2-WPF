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

namespace ProyectoWPF.NewFolders {
    /// <summary>
    /// Lógica de interacción para ChangeName.xaml
    /// </summary>
    public partial class ChangeName : Window {

        private string _rutaPadre = "";
        private string name = "";
        private string descripcion = "";
        private string img = "";
        private bool _isFolderMode = true;
        private bool _nameChanged = false;
        private ICollection<string> generos = new List<string>();
        public ChangeName(string rutaPadre, bool isFolderMode) {
            InitializeComponent();
            _rutaPadre = rutaPadre;
            _isFolderMode = isFolderMode;
            if (!_isFolderMode) {
                this.Height = 250;
                rowGeneros.Height = new GridLength(0, GridUnitType.Star);
                rowDescripcion.Height = new GridLength(0, GridUnitType.Star);
            } else {
                this.Height = 735;
                rowGeneros.Height = new GridLength(3, GridUnitType.Star);
                rowDescripcion.Height = new GridLength(2, GridUnitType.Star);
            }
            genderSelection.getAcceptButton().Click += hideGenderSelection;
            genderSelection.setMode(ActionPanel.MODIFY_FOLDER_MODE, null, null);
            bAccept.getButton().Click += BAceptar_Click;
        }

        private void BAceptar_Click(object sender, EventArgs e) {
            if (Title.Text.CompareTo("") != 0) {
                Regex containsABadCharacter = new Regex("[" + Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())) + "]");
                if (!containsABadCharacter.IsMatch(Title.Text)) {
                    if (!Lista.Contains(_rutaPadre + "/" + Title.Text)) {
                        if (!name.Equals(Title.Text)) {
                            _nameChanged = true;
                        }
                        name = Title.Text;
                        descripcion = DescBox.Text;
                        img = dirImg.Text;
                        generos = genderSelection.getGendersSelected().Keys.ToList<string>();

                        this.Close();
                    } else {
                        MessageBox.Show("Una carpeta con ese nombre ya existe");
                    }
                } else {
                    MessageBox.Show("El nombre contiene caractéres no permitidos: " + new string(System.IO.Path.GetInvalidFileNameChars()));
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

        public string getDescripcion() {
            return descripcion;
        }

        public void setDescripcion(string s) {
            DescBox.Text = s;
        }

        public string getDirImg() {
            return img;
        }

        public void setImg(string s) {
            dirImg.Text = s;
        }

        public ICollection<string> getGeneros() {
            return generos;
        }

        public bool isNameChanged() {
            return _nameChanged;
        }

        private void selectGender_Click(object sender, EventArgs e) {
            genderSelection.Visibility = Visibility.Visible;
            genderSelection.loadGenders();
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
        }

        public void changeGenderMode(long mode,string rutaCarpeta, Dictionary<string, bool> selectedGenders) {
            genderSelection.setMode(mode, rutaCarpeta, selectedGenders);
        }
    }
}
