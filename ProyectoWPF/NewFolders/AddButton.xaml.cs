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
    /// Lógica de interacción para AddButton.xaml
    /// </summary>
    public partial class AddButton : Window {
        private Button aux;
        private bool added = false;
        public AddButton(Button b) {
            InitializeComponent();
            aux = b;
        }

        private void onClickAccept(object sender, EventArgs e) {
            if (Title.Text.CompareTo("") != 0) {
                Regex containsABadCharacter = new Regex("[" + Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())) + "]");
                if (!containsABadCharacter.IsMatch(Title.Text)) {
                    aux.Content = Title.Text;
                    added = true;
                    this.Close();
                } else {
                    MessageBox.Show("El nombre contiene caractéres no permitidos: " + new string(System.IO.Path.GetInvalidFileNameChars()));
                }
            } else {
                MessageBox.Show("No has introducido un titulo");
            }
        }

        private void onClickCancel(object sender, EventArgs e) {
            this.Close();
        }

        public bool isAdded() {
            return added;
        }
    }
}
