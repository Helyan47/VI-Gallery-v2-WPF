using ProyectoWPF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Lógica de interacción para NewProfile.xaml
    /// </summary>
    public partial class NewProfile : Window {

        private bool addedProfile = false;
        private string name = "";
        public NewProfile() {
            InitializeComponent();
        }

        public void onAceptar(object sender, RoutedEventArgs e) {
            if (newProfileText.Text.CompareTo("") != 0) {
                if (!Lista.checkProfile(newProfileText.Text)) {
                    PerfilClass p = new PerfilClass(newProfileText.Text.ToString());
                    Lista.addProfile(p);
                    name = newProfileText.Text;
                    addedProfile = true;
                    this.Close();
                }
            } else {
                MessageBox.Show("No has introducido un nombre");
            }
        }

        private void onCancel(object sender, RoutedEventArgs e) {
            addedProfile = false;
            this.Close();
        }

        public bool isAdded() {
            return addedProfile;
        }

        public string getName() {
            return name;
        }
    }
}
